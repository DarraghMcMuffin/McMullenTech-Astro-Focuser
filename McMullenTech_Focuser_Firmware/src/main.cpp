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

  master->ser->begin(USB_BAUD);

  xTaskCreatePinnedToCore(COMPort::run, "COMPortTask", 4000, (void*)master->ser, 4, NULL, 1);
  xTaskCreatePinnedToCore(digitalInput::run, "digitalInputTask", 2000, (void*)master->buttons, 6, NULL, 1);
  xTaskCreatePinnedToCore(stepper::run, "stepperTask", 8000, (void*)master->motor, 8, NULL, 1);                 
  xTaskCreatePinnedToCore(sensor::run, "sensorTask", 2000, (void*)master->weather, 2, NULL, 1);       
  //xTaskCreatePinnedToCore(NVMEM::run, "NVMEMTask", 2048, NULL, 1, NULL, 1);   

  vTaskDelay(pdMS_TO_TICKS(100));

  for(;;){
    
    master->executeButtons();

    // TODO: block parsing new commands until last command processed (command buffer?)
    if(master->ser->commandAvailable()){
      master->executeCommand();
    }

    // if moving changes state, ser->reportInfo()
    
    /*
    if(weather->available()){
      temp = weather->getTemperature();
      hum = weather->getHumidity();
      pres = weather->getPressure();
      dp = weather->getDewPoint();
      snprintf(charBuff, OUT_BUFF_SIZE, "%.2f %.2f %.3f %.2f", temp, hum, pres, dp);
      ser->send(charBuff);
    }
    */

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
      this->motor->emergencyStop();
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
      this->motor->setCurrentPos(cmd.val);
    }
    else{

    }
  }

  else if(this->cmd.prim == 'R'){ // Read
    if(this->cmd.sec == 'p'){
      //this->ser->reportPosition(this->motor->getPosition());
    }
    else if(this->cmd.sec == 'm'){
      //this->ser->reportMoving(this->motor->isMoving());
    }
    else if(this->cmd.sec == 'w'){
      //this->ser->reportPosition(this->weather->getData);
    }
    else if(this->cmd.sec == 'v'){
      //this->ser->reportVersion();
    }
    else{

    }
  }

  else if(this->cmd.prim == 'M'){ // Move
    if(this->cmd.sec == 'a'){     // Absolute
      if(!this->motor->moveToPosition(this->cmd.val)){ // returns error if out of bounds, return 0 if OK
        
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
}


Master::~Master(){

}


void loop(){
}


