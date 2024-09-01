#pragma once

#include "config.hpp"
#include "COMPort.hpp"
#include "weather.hpp"
#include "stepper.hpp"
#include "buttons.hpp"
#include "NVMEM.hpp"

class Master {
    public:
        Master();
        ~Master();
        static void run(void *pvParameter);

    private:        
        COMPort ser;
        digitalInput buttons; 
};