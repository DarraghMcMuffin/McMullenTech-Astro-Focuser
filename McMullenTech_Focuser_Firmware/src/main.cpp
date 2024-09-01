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

static void masterTask(void *pvParameter){
  COMPort* ser = new COMPort(USB_SERIAL);
  digitalInput* buttons = new digitalInput();
  stepper* motor = new stepper();
  sensor* weather = new sensor();

  ser->begin(USB_BAUD);

  xTaskCreatePinnedToCore(COMPort::run, "COMPortTask", 4000, (void*)ser, 4, NULL, 1);
  xTaskCreatePinnedToCore(digitalInput::run, "digitalInputTask", 2000, (void*)buttons, 6, NULL, 1);
  xTaskCreatePinnedToCore(stepper::run, "stepperTask", 8000, (void*)motor, 8, NULL, 1);                 
  xTaskCreatePinnedToCore(sensor::run, "sensorTask", 2000, (void*)weather, 2, NULL, 1);       
  //xTaskCreatePinnedToCore(NVMEM::run, "NVMEMTask", 2048, NULL, 1, NULL, 1);   
  
  bool b1 = 0;
  bool b2 = 0;
  float temp = 0.0;
  float hum = 0.0;
  float pres = 0.0;
  float dp = 0.0;
  char charBuff[OUT_BUFF_SIZE];

  vTaskDelay(pdMS_TO_TICKS(100));

  for(;;){
    
    b1 = buttons->isPressed(1);
    b2 = buttons->isPressed(2);

    if(!b1 && !b2){
      motor->stopJog();
    }

    if(b1 && b2){
      motor->reduceSpeed(1);
    }else{
      motor->reduceSpeed(10);
    }

    if(b1 && !b2){
      motor->startJog(1);
    }

    if(!b1 && b2){
      motor->startJog(-1);
    }
    
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

  xTaskCreatePinnedToCore(masterTask,   // Task function
                          "MasterTask",  // Task name
                          16000,           // Stack size (bytes)
                          NULL,           // Parameter to pass to task
                          10,              // Priority
                          NULL,           // Task handle
                          1 );            // Core

}


void loop(){
}


