#include "comport.hpp"


COMPort::COMPort(SoftwareSerial& device){
    swStream = &device;
}

COMPort::COMPort(HWCDC& device){
    hwcdcStream = &device;
}

COMPort::COMPort(HardwareSerial& device){
    hwStream = &device;
}

COMPort::~COMPort(){
}

void COMPort::begin(uint32_t baudRate){
  if (hwStream){
    hwStream->begin(baudRate);
    stream = (Stream*)hwStream;
  }
  if (swStream){
    swStream->begin(baudRate);
    stream = (Stream*)swStream;
  }
  if (hwcdcStream){
    hwcdcStream->begin(baudRate);
    stream = (Stream*)hwcdcStream;
  }
}


void COMPort::run(void *pvParameter){
    COMPort* ser = static_cast<COMPort*> (pvParameter);
    char c = '\0';

    for(;;){
        //ser->stream->println(ser->outBuff);
        vTaskDelay( pdMS_TO_TICKS(COM_LOOP_RATE) );

        if(ser->stream->available() > 0){
          c = ser->stream->read();
          // simplify into parseInput()
          if(c == '\r' && !ser->cmd.valid){
            ser->cmd = ser->parseCommand(ser->inBuff);
            /*
            if(command.valid){
              ser->send("VALID");
            }else{
              ser->send("INVAL");
            }
            */
            ser->resetInBuff();
          }else if(c != '\n'){    // ignore \n
            if(!ser->appendInBuff(c)){
              ser->resetInBuff();
            }
          }
        }

        /*
        if(command.valid){
          //ser->send("EXEC");
          ser->execCmd(ser->cmd);
          command.valid = false;
        }
        */

    }
}


int COMPort::appendInBuff(char c){
  this->inBuff[inBuffIdx] = c;
  this->inBuffIdx++;
  this->send(this->inBuff);
  if(this->inBuffIdx < IN_BUFF_SIZE){
    return 1;
  }
  return 0;
}


void COMPort::resetInBuff(){
  memset(this->inBuff, 0, sizeof(this->inBuff));
  this->inBuffIdx = 0;
}



int COMPort::setOutBuff(const char *buff){
  int ret = 0;
  if(strlen(buff) > OUT_BUFF_SIZE){
    ret = 0;
  }else{
  strcpy(outBuff, buff);
    ret = 1;
  }
  return ret;
}

int COMPort::send(const char *buff){
  this->stream->println(buff);
  return 1;
}


struct cmdStruct COMPort::parseCommand(const char *buff){   
  cmdStruct newCmd;
  char c = '\0';
  int i = 0;
  char valBuff[MAX_FLOAT_LENGTH] = {'0'};   // allow no value command (val buff default to 0 and pass)
  int valBuffIdx = 0;
  int state = 0;    // 0 = waiting for cmd start
                    // 1 = primary cmd
                    // 2 = secondary cmd
                    // 3 = value
                    // 4 = end of cmd (valid cmd parsed)
                    // -1 = exit
  while(i < IN_BUFF_SIZE && state != -1){
    c = buff[i];
    switch(state){
      case 0:{ if(c == '['){state++; i++;}else{state = -1;} break;}                           // if '[': start command parsing
      case 1:{ if(isupper(c)){newCmd.prim = c; state++; i++;}else{state = -1;} break;}      // if 'A': set primary command char
      case 2:{ if(islower(c)){ newCmd.sec = c; state++; i++;}else{state = -1;} break;}      // if 'a': set secondary command char
      case 3:{ if((isdigit(c) || (c == '-' && valBuffIdx == 0)) && valBuffIdx < MAX_FLOAT_LENGTH){ valBuff[valBuffIdx] = c;     // if '#': write to value buffer 
                                                                valBuffIdx++;                                //         increment val buff idx
                                                                i++;}                                         //         next char                   
                                                              else if(c == ']'){state++;}                    // if ']': end of command
                                                              else{state = -1;}                 
                                                              break;}
      case 4:{ newCmd.val = (int32_t)atof(valBuff); newCmd.valid = true; state = -1; break;}         // set value to int cast of valBuff and set cmd valid
      default:{state = -1; break;}
    }
  }
  return newCmd;
}


int COMPort::reportCmd(){   // add set:home:relative:0 to set home at current position
  if(!this->cmd.valid){
    return 0;
  }
  char tempBuff[OUT_BUFF_SIZE] = {'\0'};
  snprintf(tempBuff, OUT_BUFF_SIZE, "<%c%c%d>", (char)this->cmd.prim, (char)this->cmd.sec, (int32_t)this->cmd.val);
  this->send(tempBuff);

  return 1;
}



bool COMPort::commandAvailable(){
  return this->cmd.valid;
}

struct cmdStruct COMPort::getCommand(){
  this->reportCmd();
  this->cmd.valid = false;
  return this->cmd;
}