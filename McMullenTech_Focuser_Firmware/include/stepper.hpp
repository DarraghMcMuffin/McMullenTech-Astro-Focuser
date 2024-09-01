#pragma once

#include "config.hpp"
#include <TMCStepper.h>
#include <ESP_FlexyStepper.h>

class stepper {

    public:
        stepper();
        bool isMoving();
        float getPosition();
        uint8_t moveToPosition(float pos);
        uint8_t moveRelative(float dist);
        uint8_t enable();
        uint8_t disable();
        uint8_t softStop();
        uint8_t startJog(signed char direction);
        uint8_t stopJog();
        uint8_t emergencyStop();
        uint8_t resetEmergencyStop();
        uint8_t setHomeHere();
        uint8_t setCurrentPos(float pos);
        long stepsToTarget();

        static void run(void *pvParameter);

        void reduceSpeed(int factor);

    private:
        float position;
        HardwareSerial stepper_serial;
        DRIVER_MODEL driver;
        ESP_FlexyStepper motor;
        float mm2um(float mm);
        float um2mm(float um);
        float steps2mm(long steps);
        long mm2steps(float mm);

        int killFlag = 0;
};

