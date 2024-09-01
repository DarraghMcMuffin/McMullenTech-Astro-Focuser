#pragma once

#include "config.hpp"
#include "nvs_flash.h"
#include "nvs.h"

// consider implementing key-value pairs for most config values

class NVMEM{

    public:
        NVMEM();
        uint8_t writePosInSteps(int32_t posInSteps);
        int32_t readPosInSteps();

        static void run(void *pvParameter);

    private:
        uint8_t startNVS();
        uint8_t endNVS();
        nvs_handle_t nvs_handle;

};