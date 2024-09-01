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

void NVMEM::run(void *pvParameter){
    
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


uint8_t NVMEM::writePosInSteps(int32_t posInSteps){
    uint8_t ret = 0;
    esp_err_t err = nvs_set_i32(nvs_handle, "posInSteps", posInSteps);
    if(err == ESP_OK){
        ret = 1;
    } else {
        ret = 0;
        return ret;
    }
    // Commit written value.
    // After setting any values, nvs_commit() must be called to ensure changes are written
    // to flash storage. Implementations may write to storage at other times,
    // but this is not guaranteed.
    err = nvs_commit(nvs_handle);
    if(err == ESP_OK){
        ret = 1;
    } else {
        ret = 0;
    }
    return ret;
}


int32_t NVMEM::readPosInSteps(){
    int32_t posInSteps = 0; // value will default to 0, if not set yet in NVS
    esp_err_t err = nvs_get_i32(nvs_handle, "posInSteps", &posInSteps);
    /*
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
    */

    return posInSteps;
}