#include "buttons.hpp"


digitalInput::digitalInput(){

    // set weak pullup on both pins
    // set debounce
    // enable jogging on falling edge
    // disable jogging on rising edge
    // both buttons for 500ms = change jog speed??    

    pinMode(BUTTON_1, INPUT_PULLUP);
    pinMode(BUTTON_2, INPUT_PULLUP);

}


bool digitalInput::isPressed(int button){
    switch(button){
        case 1:
            return(!state_1);
        case 2:
            return(!state_2);
        default:
            return 0;
    }
}


void digitalInput::run(void *pvParameter){
    digitalInput* buttons = static_cast<digitalInput*> (pvParameter);
    for(;;){
        buttons->pollInputs();
        vTaskDelay(pdMS_TO_TICKS(5));
    }
}


void digitalInput::pollInputs(){
    bool state_new = 0;
    uint32_t time = 0;

    state_new = digitalRead(BUTTON_1);
    time = pdTICKS_TO_MS(xTaskGetTickCount());
    if(state_new != state_1_prev){  // if button changed state
        state_1_prev = state_new;   // save new state
        state_1_prev_time = time;   // start timer 
    }
    else{   // if button did not change state 
        if(state_1 != state_1_prev){    // AND not equal to debounced state
            if(time - state_1_prev_time >= DEBOUNCE_MILLIS){   // AND timer elapsed
                state_1 = state_1_prev; // update debounced state
            }
        }
    }

    state_new = digitalRead(BUTTON_2);
    time = pdTICKS_TO_MS(xTaskGetTickCount());
    if(state_new != state_2_prev){  // if button changed state
        state_2_prev = state_new;   // save new state
        state_2_prev_time = time;   // start timer 
    }
    else{   // if button did not change state 
        if(state_2 != state_2_prev){    // AND not equal to debounced state
            if(time - state_2_prev_time >= DEBOUNCE_MILLIS){   // AND timer elapsed
                state_2 = state_2_prev; // update debounced state
            }
        }
    }

}

// this method: (slower to react to button press, fast computation)
// poll buttons always, if state change: start timer (restarts on bounces)
// if not change state: check if timer elapsed: if true: update debounced state
// functionality: timer restarts on every bounce, must remain in new state for Xms before set

// another method: (faster to react to button press, slower computation)
// add bool button_lockout_timer and update_timers()
// when not locked out, check for change state, if change state: start lockout timer (don't poll during lockout)
// once timer elapsed: check state again, if not changed, update debonced state
// functionality: timer starts on initial state change, if same at end of timer, set state