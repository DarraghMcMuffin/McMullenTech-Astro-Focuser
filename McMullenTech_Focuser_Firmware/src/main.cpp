#include "config.hpp"
#include "COMPort.hpp"
#include "weather.hpp"
#include "stepper.hpp"
#include "buttons.hpp"
#include "NVMEM.hpp"
#include "main.hpp"



// request command from ser and digitalInput in main
// command requests data from other sensors or actions for other tasks, pass back response 
// ser handles conversions and parsing

void Master::run(void *pvParameter){
  Master* master = static_cast<Master*> (pvParameter);

  pinMode(LED_BUILTIN, OUTPUT);

  xTaskCreatePinnedToCore(stepper::run, "stepperTask", 8000, (void*)master->motor, 8, NULL, 1);
  vTaskDelay(pdMS_TO_TICKS(50));
  
  if(master->nvs->init()){
    master->motor->setCurrentPosSteps(master->nvs->getPosSteps());
  }else{
    master->motor->setCurrentPos(14000);
  }
  
  xTaskCreatePinnedToCore(digitalInput::run, "digitalInputTask", 2000, (void*)master->buttons, 4, NULL, 1);
  vTaskDelay(pdMS_TO_TICKS(50));

  xTaskCreatePinnedToCore(sensor::run, "sensorTask", 2000, (void*)master->weather, 2, NULL, 1);  
  vTaskDelay(pdMS_TO_TICKS(50));

  xTaskCreatePinnedToCore(COMPort::run, "COMPortTask", 4000, (void*)master->ser, 6, NULL, 1);     
  vTaskDelay(pdMS_TO_TICKS(200));

  bool isMoving = false;

  for(;;){
    
    master->executeButtons();

    // TODO: block parsing new commands until last command processed (command buffer?)
    if(master->ser->commandAvailable()){
      master->executeCommand();
    }

    // if moving changes state, update and send message
    if(master->motor->isMoving() != isMoving){
      isMoving = master->motor->isMoving();
      master->ser->reportInfo('m', int(isMoving));

      if(!isMoving){ // if just finished a move
        if(master->nvs->nvs_OK()){
          master->nvs->updatePosSteps(master->motor->getPositionSteps());
        }
        
      }
    
    }
    
    

    vTaskDelay(pdMS_TO_TICKS(MAIN_LOOP_RATE));

  }
}




void setup(){

  Master* master = new Master();

  xTaskCreatePinnedToCore(Master::run,   // Task function
                          "MasterTask",  // Task name
                          16000,         // Stack size (bytes)
                          (void*)master, // Parameter to pass to task
                          10,            // Priority
                          NULL,          // Task handle
                          1 );           // Core

}



void Master::executeButtons(){

  b1 = buttons->isPressed(1);
  b2 = buttons->isPressed(2);

  if(b1 && !b2){
    motor->startJog(1);
  }

  if(!b1 && b2){
    motor->startJog(-1);
  }

  if(motor->isJogging()){
    if(!b1 && !b2){
      motor->stopJog();
      motor->reduceSpeed(1);  // TODO set jog speed and command move speed separately
    }
    if(b1 && b2){
      motor->reduceSpeed(1);
    }else{
      motor->reduceSpeed(10);
    }
  }

}



void Master::executeCommand(){

  this->cmd = ser->getCommand();

  if(this->cmd.prim == 'E'){
    if(this->cmd.sec == 's'){
      if(this->cmd.val == -1){
        this->motor->resetEmergencyStop();
      }
      else{
        this->motor->emergencyStop();
      }
    }
    else{

    }
  }

  else if(this->cmd.prim == 'S'){  // Set
    if(this->cmd.sec == 'h'){
      // check that motor is stopped before setting home, else return error
      this->motor->setHomeHere();
    }
    else if(this->cmd.sec == 'r'){
      // check against rate limits
      if(cmd.val <= 0){
        // Error
      }
      else{this->motor->reduceSpeed(cmd.val);}
    }
    else if(this->cmd.sec == 'p'){
      // check that motor is stopped before setting position, else return error
      // check bounds 
      this->motor->setCurrentPos((float)cmd.val);
      if(this->nvs->nvs_OK()){
        this->nvs->updatePosSteps(this->motor->getPositionSteps());
      }
    }
    else{

    }
  }

  else if(this->cmd.prim == 'R'){ // Read
    if(this->cmd.sec == 'p'){
      if(abs((float)this->targetPos - this->motor->getPosition()) <= 1){  // if error between requested position and true position <= 1
        snprintf(this->charBuff, OUT_BUFF_SIZE, "<Ip%d>", this->targetPos); // return target pos (to fixed off-by-1 error)
        this->targetPos = 0;
      }else{
        snprintf(this->charBuff, OUT_BUFF_SIZE, "<Ip%d>", (int32_t)this->motor->getPosition());
      }
      this->ser->send(this->charBuff);
    }
    else if(this->cmd.sec == 'm'){
      snprintf(this->charBuff, OUT_BUFF_SIZE, "<Im%d>", this->motor->isMoving());
      this->ser->send(this->charBuff);
    }
    else if(this->cmd.sec == 't'){
      snprintf(this->charBuff, OUT_BUFF_SIZE, "<It%.2f>", this->weather->getTemperature());
      this->ser->send(this->charBuff);
    }
    else if(this->cmd.sec == 'k'){
      snprintf(this->charBuff, OUT_BUFF_SIZE, "<It%.3f>", this->weather->getPressure());
      this->ser->send(this->charBuff);
    }
    else if(this->cmd.sec == 'h'){
      snprintf(this->charBuff, OUT_BUFF_SIZE, "<It%.2f>", this->weather->getHumidity());
      this->ser->send(this->charBuff);
    }
    else if(this->cmd.sec == 'd'){
      snprintf(this->charBuff, OUT_BUFF_SIZE, "<It%.2f>", this->weather->getDewPoint());
      this->ser->send(this->charBuff);
    }
    else if(this->cmd.sec == 'v'){
      snprintf(this->charBuff, OUT_BUFF_SIZE, "<Iv'%s'>", VERSION);
      this->ser->send(this->charBuff);
    }
    else{

    }
  }

  else if(this->cmd.prim == 'M'){ // Move
    if(this->cmd.sec == 'a'){     // Absolute
      if(!this->motor->moveToPosition(this->cmd.val)){ // returns error if out of bounds, return 0 if OK
        this->targetPos = this->cmd.val;
      }
      else{
        
      }
    }
    else if(this->cmd.sec == 'r'){// Relative
      if(!this->motor->moveRelative(this->cmd.val)){ // returns error if out of bounds, return 0 if OK
        
      }
      else{
        
      }
    }
    else if(this->cmd.sec == 's'){// Stop
      this->motor->softStop();
    }
    else{
      // invalid
    }
  }

  else{
    // invalid
  }

}



Master::Master(){
  ser = new COMPort(USB_SERIAL);
  buttons = new digitalInput();
  motor = new stepper();
  weather = new sensor();
  nvs = new NVMEM();
}


Master::~Master(){

}


void loop(){
}


