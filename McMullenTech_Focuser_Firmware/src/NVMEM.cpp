#include "NVMEM.hpp"


NVMEM::NVMEM(){ 

    // Initialize NVS
    esp_err_t err = nvs_flash_init();
    if (err == ESP_ERR_NVS_NO_FREE_PAGES || err == ESP_ERR_NVS_NEW_VERSION_FOUND) {
        // NVS partition was truncated and needs to be erased
        // Retry nvs_flash_init
        ESP_ERROR_CHECK(nvs_flash_erase());
        err = nvs_flash_init();
    }
    ESP_ERROR_CHECK( err );

}


NVMEM::~NVMEM(){
    xTimerDelete(timer_handle,0);
    endNVS();
}


void NVMEM::timerCallback(TimerHandle_t xTimer){

    NVMEM* nvs = static_cast<NVMEM*> (pvTimerGetTimerID(xTimer));

    if(!nvs->isStored()){
        nvs->writePosSteps(nvs->currentPosSteps);
    }
    
}


bool NVMEM::init(){ // replace with init() function, don't use a task
    OK = false;

    // init NVS
    if(startNVS()){
        OK = true;
    }else{
        return OK;
    }

    // init timer
    timer_handle = xTimerCreate("posStoreTimer",                // string name
                                pdMS_TO_TICKS(NVS_POS_TIMEOUT), // duration
                                pdFALSE,                        // auto reload
                                this,                           // (timer ID) reference to this object to cast in callback
                                &timerCallback);                // callback function

    // read initial position
    currentPosSteps = readPosSteps();

    return OK;
}


uint8_t NVMEM::startNVS(){
    uint8_t ret = 0;
    esp_err_t err = nvs_open("storage", NVS_READWRITE, &nvs_handle);
    if (err = ESP_OK) {
        // opened successfully
        ret = 1;
    } else {
        // error opening nvs handle
        ret = 0;
    }
    return ret;
}


uint8_t NVMEM::endNVS(){
    uint8_t ret = 1;
    nvs_close(nvs_handle);
    return ret;
}


bool NVMEM::nvs_OK(){
    return OK;
}


uint8_t NVMEM::writePosSteps(int32_t posInSteps){
    uint8_t ret = 0;
    esp_err_t err = nvs_set_i32(nvs_handle, "posInSteps", posInSteps);
    if(err == ESP_OK){
        ret = 1;
    } else {
        ret = 0;
        return ret;
    }
    err = nvs_commit(nvs_handle);
    if(err == ESP_OK){
        ret = 1;
    } else {
        ret = 0;
    }
    return ret;
}


int32_t NVMEM::readPosSteps(){
    int32_t posInSteps = 0; // value will default to 0, if not set yet in NVS
    esp_err_t err = nvs_get_i32(nvs_handle, "posInSteps", &posInSteps);
    switch (err) {
        case ESP_OK:
            // done
            break;
        case ESP_ERR_NVS_NOT_FOUND:
            // The value is not initialized yet
            break;
        default :
            // "Error reading
            break;
    }

    return posInSteps;
}


int32_t NVMEM::getCurrentPosSteps(){
    return currentPosSteps;
}


int32_t NVMEM::getStoredPosSteps(){
    if(!isStored()){
        return readPosSteps();
    }else{
        return currentPosSteps;
    }
}


void NVMEM::storePosSteps(int32_t posSteps){
    currentPosSteps = posSteps;
    writePosSteps(currentPosSteps);
}

void NVMEM::updatePosSteps(int32_t posSteps){
    currentPosSteps = posSteps;
    if(xTimerIsTimerActive(timer_handle)){
        xTimerReset(timer_handle,0);
    }else{
        xTimerStart(timer_handle,0);
    }
}


bool NVMEM::isStored(){
    return (readPosSteps() == currentPosSteps);
}