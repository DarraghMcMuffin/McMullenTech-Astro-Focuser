#pragma once

#include "config.hpp"
#include <TMCStepper.h>
#include <ESP_FlexyStepper.h>

class stepper {

    public:
        stepper();
        bool isMoving();
        float getPosition();
        int32_t getPositionSteps();
        uint8_t moveToPosition(float pos);
        uint8_t moveRelative(float dist);
        uint8_t enable();
        uint8_t disable();
        uint8_t softStop();
        uint8_t startJog(signed char direction);
        uint8_t stopJog();
        bool isJogging();
        uint8_t emergencyStop();
        uint8_t resetEmergencyStop();
        uint8_t setHomeHere();
        uint8_t setCurrentPos(float pos);
        uint8_t setCurrentPosSteps(int32_t steps);
        long stepsToTarget();

        static void run(void *pvParameter);

        void reduceSpeed(int factor);

    private:
        bool jogging = false;
        float position = 0;
        HardwareSerial stepper_serial;
        DRIVER_MODEL driver;
        ESP_FlexyStepper motor;
        float mm2um(float mm);
        float um2mm(float um);
        float steps2mm(long steps);
        long mm2steps(float mm);

        int killFlag = 0;
};

