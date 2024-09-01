#pragma once

#include "config.hpp"
#include <Bme280.h>

class sensor {

    public:
        sensor();

        static void run(void *pvParameter);
        void pollSensor();
        float getTemperature(); // deg C
        float getPressure();    // kPa
        float getHumidity();    // %
        float getDewPoint();    // deg C
        bool available();       // true if new data is available, reading any value (temp,pres,hum) resets to false

    private:
        float calcDewPoint(float temp, float hum);
        float temperature = 0.0;
        float humidity = 0.0;
        float pressure = 0.0;
        float dewPoint = 0.0;
        bool freshData = 0;

};
