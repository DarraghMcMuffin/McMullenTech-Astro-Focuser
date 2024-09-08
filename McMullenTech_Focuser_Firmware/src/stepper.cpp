#include "stepper.hpp"


stepper::stepper(): stepper_serial(HardwareSerial(TMC_SERIAL)),
                    driver(DRIVER_MODEL(&stepper_serial, R_SENSE, DRIVER_ADDRESS))
    {
    pinMode(DIR_PIN, OUTPUT);
    pinMode(STP_PIN, OUTPUT);
    pinMode(EN_PIN, OUTPUT);

    stepper_serial.begin(TMC_BAUD, SERIAL_8N1, -1, -1);
    driver.begin();
    driver.toff(3);
    driver.rms_current(I_RUN, I_HOLD);
    driver.seimin(1);
    //driver.semin(5);
    //driver.semax(4);
    driver.pwm_autoscale(true);
    driver.en_spreadCycle(false);//(false);
    driver.microsteps((MICROSTEPPING == 1) ? 0 : MICROSTEPPING);
    driver.intpol(true);
    driver.TCOOLTHRS(0xFFFFF);
    driver.iholddelay(15);
    driver.TPOWERDOWN(255);
    driver.blank_time(24);

    motor.connectToPins(STP_PIN, DIR_PIN);
    motor.setEnablePin(EN_PIN, ESP_FlexyStepper::ACTIVE_LOW);
    motor.setStepsPerMillimeter(STEPS_PER_UM * 1000);
    motor.setSpeedInStepsPerSecond(MAX_RATE * MICROSTEPPING);
    motor.setAccelerationInStepsPerSecondPerSecond(MAX_ACCEL * MICROSTEPPING);
    motor.setDecelerationInStepsPerSecondPerSecond(MAX_ACCEL * MICROSTEPPING);
    
}


void stepper::run(void *pvParameter){
    stepper* motor = static_cast<stepper*> (pvParameter);
    motor->enable();
    
    for(;;){

        // dynamic spreadcycle/stealthchop mode
        if(motor->isMoving()){
            motor->driver.en_spreadCycle(true);
            digitalWrite(LED_BUILTIN, LOW);
        }else{
            motor->driver.en_spreadCycle(false);
            digitalWrite(LED_BUILTIN, HIGH);
        }

        vTaskDelay(pdMS_TO_TICKS(20));
    }

    motor->disable();
}

uint8_t stepper::enable(){
    motor.startAsService(0);    // run on core 0
    motor.enableDriver();
    return 1;
}

uint8_t stepper::disable(){
    motor.disableDriver();
    motor.stopService();
    return 1;
}

void stepper::reduceSpeed(int factor){
    motor.setSpeedInStepsPerSecond((MAX_RATE * MICROSTEPPING)/factor);
}

uint8_t stepper::moveToPosition(float pos){
    float mm = 0;
    mm = um2mm(pos);
    // check against limits
    motor.setTargetPositionInMillimeters(mm);
    return 1;
}

uint8_t stepper::moveRelative(float dist){
    float mm = 0;
    mm = um2mm(dist);
    // check against limits
    motor.setTargetPositionRelativeInMillimeters(mm);
    return 1;
}

bool stepper::isMoving(){
    bool ret = false;
    if(!motor.motionComplete() || isJogging()) {ret = true;}
    return ret;
}

float stepper::getPosition(){
    float pos = 0;
    pos = motor.getCurrentPositionInMillimeters();
    pos = mm2um(pos);
    return pos;
}

uint8_t stepper::softStop(){
    motor.setTargetPositionToStop();
    return 1;
}

uint8_t stepper::startJog(signed char direction){
    motor.startJogging(direction);
    jogging = true;
    return 1;
}

uint8_t stepper::stopJog(){
    motor.stopJogging();
    jogging = false;
    return 1;
}

bool stepper::isJogging(){
    return jogging;
}

uint8_t stepper::emergencyStop(){
    motor.emergencyStop(true); // hold until released
    // if motor keeps moving after 250ms, disable driver, if still moving, return failed to stop as return value, handle elsewhere as error
    return 1;
}

uint8_t stepper::resetEmergencyStop(){
    motor.releaseEmergencyStop();
    return 1;
}

uint8_t stepper::setHomeHere(){
    motor.setCurrentPositionAsHomeAndStop();
    return 1;
}

uint8_t stepper::setCurrentPos(float pos){
    float mm = 0;
    mm = um2mm(pos);
    motor.setCurrentPositionInMillimeters(mm);
    return 1;
}

long stepper::stepsToTarget(){
    long steps = 0;
    steps = motor.getDistanceToTargetSigned();
    return steps;
}

float stepper::mm2um(float mm){
    float um = 0;
    um = 1000 * mm;
    return um;
}

float stepper::um2mm(float um){
    float mm = 0;
    mm = 0.001 * um;
    return mm;
}

float stepper::steps2mm(long steps){
    float mm = 0;
    mm = (0.001/STEPS_PER_UM) * steps;
    return mm;
}

long stepper::mm2steps(float mm){
    long steps = 0;
    steps = long((1000*STEPS_PER_UM) * mm);
    return steps;
}


// remember to program limit checks (override all movement commands), allowed to move oposite limits if overshot