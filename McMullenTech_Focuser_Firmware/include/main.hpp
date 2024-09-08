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
        void executeButtons();
        void executeCommand();

    private:        
        COMPort* ser = NULL;
        digitalInput* buttons = NULL;
        stepper* motor = NULL;
        sensor* weather = NULL;
        int state = 0;  // -1 = error, 0 = not init, 1 = ready, 2 = moving
        bool b1 = 0;
        bool b2 = 0;
        float temp = 0.0;
        float hum = 0.0;
        float pres = 0.0;
        float dp = 0.0;
        char charBuff[OUT_BUFF_SIZE];
        struct cmdStruct cmd;
        
};
