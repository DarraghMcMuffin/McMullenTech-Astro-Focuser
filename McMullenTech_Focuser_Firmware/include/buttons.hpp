#pragma once

#include "config.hpp"


class digitalInput{

    public:
        digitalInput();

        static void run(void *pvParameter);

        bool isPressed(int button);

    private:

        void pollInputs();

        bool state_1 = 0;
        bool state_2 = 0;

        bool state_1_prev = 0;
        bool state_2_prev = 0;
        uint32_t state_1_prev_time = 0;
        uint32_t state_2_prev_time = 0;
        
};