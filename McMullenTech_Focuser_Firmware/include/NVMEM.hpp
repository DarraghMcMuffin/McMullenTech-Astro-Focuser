#pragma once

#include "config.hpp"
#include "nvs_flash.h"
#include "nvs.h"

// consider implementing key-value pairs for most config values

class NVMEM{

    public:
        NVMEM();
        ~NVMEM();
        int32_t getPosSteps();
        void updatePosSteps(int32_t posSteps);
        bool init();
        bool nvs_OK();

    private:
        static void timerCallback(TimerHandle_t xTimer);
        uint8_t startNVS();
        uint8_t endNVS();
        uint8_t writePosSteps(int32_t posSteps);
        int32_t readPosSteps();

        TimerHandle_t timer_handle;
        nvs_handle_t nvs_handle;
        int32_t storedPosSteps = 0;
        bool OK = false;
};