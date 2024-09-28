#include <Arduino.h>
#include <stdint.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "freertos/timers.h"
#include "freertos/event_groups.h"
#include "esp_system.h"

#pragma once

#define VERSION "MTAF_1.0"

// USB config
#define USB_SERIAL  Serial
#define USB_BAUD    115200
#define STREAM_TIMEOUT 500
#define IN_BUFF_SIZE   32
#define OUT_BUFF_SIZE  32
#define MAX_FLOAT_LENGTH 6

// BME280 config
#define BME280_SDA  D4
#define BME280_SCL  D5
#define BME280_I2C_ADDRESS  Primary

// Stepper config
//#define TMC_RX    44 //D7
//#define TMC_TX    43 //D6
#define TMC_SERIAL  0
#define TMC_BAUD    115200
#define DRIVER_MODEL     TMC2209Stepper
#define DRIVER_ADDRESS   0b00
#define R_SENSE   0.11f
#define DIR_PIN   8 //D9
#define STP_PIN   7 //D8
#define EN_PIN    9 //D10
#define I_HOLD    0.1   // *I_RUN
#define I_RUN     1000
#define STEPS_PER_REV 2038
#define MICROSTEPPING 1

// Mechanical config
#define STEPS_PER_UM  1.6   // step/um
#define MAX_RATE      800   // step/s
#define MAX_ACCEL     10000 // step/s^2
#define MAX_RANGE     37000 // um

// Button config
#define BUTTON_1    3   // GPIO3 = D2   TRAVEL OUT
#define BUTTON_2    4   // GPIO4 = D3   TRAVEL IN
#define DEBOUNCE_MILLIS     20

// Rates
#define BME280_RATE 2000    // loop time (ms)
#define MAIN_LOOP_RATE 10
#define COM_LOOP_RATE 25

// NVS
#define NVS_POS_TIMEOUT 20000   // save current position if haven't moved for (ms)