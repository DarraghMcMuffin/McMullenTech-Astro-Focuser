#pragma once

#include "config.hpp"
#include <SoftwareSerial.h>


struct cmdStruct{
    bool valid;
    char prim;
    char sec;
    int32_t val;
    cmdStruct():valid(false),
                prim('\0'),
                sec('\0'),
                val(0){}
};

// Allowed commands:
// [Aaf]
// S:<h,r>:int      Set:home,rate
// R:<p,m,w,v>      Read:position,moving,weather,version
// M:<a,r,s>:int    Move:absolute,relative,stop
//
// Allowed respose:
// I:
// ""


class COMPort {

    public:
        COMPort(HWCDC& device);
        COMPort(HardwareSerial& device);
        COMPort(SoftwareSerial& device);
        ~COMPort();

        void begin(uint32_t baudRate);
        static void run(void *pvParameter);

        int appendInBuff(char c);
        void resetInBuff();
        int setOutBuff(const char *buff);
        int send(const char *buff);
        struct cmdStruct parseCommand(const char *buff);
        int reportCmd();
        bool commandAvailable();
        struct cmdStruct getCommand();

    private:        
        // check for stream available
        // add to buffer
        // parse command as cmdStruct

        void initBuff();

        HardwareSerial* hwStream = NULL;
        SoftwareSerial* swStream = NULL;
        HWCDC* hwcdcStream = NULL;
        Stream* stream = NULL;

        char inBuff[IN_BUFF_SIZE] = {'\0'};
        int inBuffIdx = 0;
        char outBuff[OUT_BUFF_SIZE] = {'\0'};
        struct cmdStruct cmd;
};

