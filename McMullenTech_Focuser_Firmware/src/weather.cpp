#include "weather.hpp"

Bme280TwoWire BME280;


sensor::sensor(){
    Wire.begin(BME280_SDA,BME280_SCL);
    BME280.begin(Bme280TwoWireAddress::BME280_I2C_ADDRESS);
    BME280.setSettings(Bme280Settings::defaults());
}


void sensor::run(void *pvParameter){
    sensor* weather = static_cast<sensor*> (pvParameter);

    for(;;){
        vTaskDelay(pdMS_TO_TICKS(BME280_RATE));
        weather->pollSensor();
    }
}


void sensor::pollSensor(){
    temperature = BME280.getTemperature();
    pressure = BME280.getPressure() / 1000.0;
    humidity = BME280.getHumidity();
    dewPoint = calcDewPoint(temperature,humidity);
    freshData = true;
}

float sensor::getTemperature(){
    freshData = false;
    return temperature;
}
float sensor::getHumidity(){    // good oportunity to use prototype function
    freshData = false;
    return humidity;
}
float sensor::getPressure(){
    freshData = false;
    return pressure;
}
float sensor::getDewPoint(){
    freshData = false;
    return dewPoint;
}
bool sensor::available(){
    return freshData;
}

float sensor::calcDewPoint(float temp, float hum){
    float a = 17.271;
    float b = 237.7;
    float var = (a * temp) / (b + temp) + log(hum * 0.01);
    float td = (b * var) / (a - var);
    return td;
}