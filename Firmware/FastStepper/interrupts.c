#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

#include "analog_input.h"
#include "stepper_motor.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
//
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
//
// ISR(TCD1_OVF_vect, ISR_NAKED)
//
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/
/* STOP                                                                 */
/************************************************************************/
extern bool motor_is_running;

ISR(PORTB_INT0_vect, ISR_NAKED)
{
	if (read_STOP_SWITCH)
	{
		/* Update register and send event */
		app_regs.REG_STOP_SWITCH = 0;
		core_func_send_event(ADD_REG_STOP_SWITCH, true);
	}
	else
	{		
		/* Stop motor */
		timer_type0_stop(&TCC0);
		motor_is_running = false;
		
		/* Disable motor */
		clr_MOTOR_ENABLE;
		
		/* Update register and send event */
		app_regs.REG_STOP_SWITCH = REG_STOP_SWITCH_B_STOP_SWITCH;
		core_func_send_event(ADD_REG_STOP_SWITCH, true);
	}
	
	reti();
}

/************************************************************************/
/* ENDSTOP                                                              */
/************************************************************************/
extern enum MovementStatus current_movement_status;
extern uint8_t endstop_counter;
extern uint8_t home_steps_events;
extern int32_t motor_current_position;
extern bool homing_enabled;
extern bool homing_performed;
extern uint8_t move_to_events;


ISR(PORTC_INT0_vect, ISR_NAKED)
{
	// If the counter is inactive and it's a falling transition, we execute the interrupt
	// The counter resets after 10ms on the core_callback_t_before_exec() function 
	if (homing_enabled && endstop_counter == 0 && read_HOME_SWITCH == false)
	{
		endstop_counter = 1;
		
		// Stop motor and reset the position
		stop_motor();
		motor_current_position = 0;
		homing_performed = true;
		
			
		// If the endstop switch was triggered while the motor was homing, that's perfect, it's what we want.
		// So in this case, we will send the success event
		if (current_movement_status==MOVEMENT_STATUS_HOMING)
		{
			home_steps_events = REG_HOME_STEPS_EVENTS_B_HOMING_SUCCESSFUL;
		}
		// However, if the switch was triggered while we were not homing, it's an unexpected event
		else
		{
			home_steps_events = REG_HOME_STEPS_EVENTS_B_UNEXPECTED_HOME;

			// If we are not homing but also not stopped, it means we were doing a normal movement that was just aborted		
			if (current_movement_status!=MOVEMENT_STATUS_STOPPED)
			{
				move_to_events = REG_MOVE_TO_EVENTS_B_MOVE_ABORTED;						
			}
		}
			
		current_movement_status = MOVEMENT_STATUS_STOPPED;		
	}
	
	reti();
}


/************************************************************************/
/* ADC                                                                  */
/************************************************************************/
ISR(ADCA_CH0_vect, ISR_NAKED)
{
	app_regs.REG_ANALOG_INPUT = get_analog_input();
	core_func_send_event(ADD_REG_ANALOG_INPUT, false);
	
	reti();
}

/************************************************************************/
/* EXTERNAL MOTOR CONTROL                                               */
/************************************************************************/

//bool external_control_first_byte = true;
//int16_t motor_pulse_interval;
//
//ISR(USARTD0_RXC_vect, ISR_NAKED)
//{
	//if (external_control_first_byte)
	//{
		//external_control_first_byte = false;
		//
		//motor_pulse_interval = USARTD0_DATA;
		//
		//timer_type0_enable(&TCD0, TIMER_PRESCALER_DIV64, 100, INT_LEVEL_LOW);	// 200 us
	//}
	//else
	//{
		//external_control_first_byte = true;
		//
		//int16_t temp = USARTD0_DATA;
		//
		//motor_pulse_interval |= (temp << 8) & 0xFF00;
		//
		//timer_type0_stop(&TCD0);
		//
		////app_regs.REG_ANALOG_INPUT = motor_pulse_interval;
		////core_func_send_event(ADD_REG_ANALOG_INPUT, true);
		//
		////app_write_REG_IMMEDIATE_PULSES(&motor_pulse_interval);
	//}
	//
	//reti();
//}
//
//
//ISR(TCD0_OVF_vect, ISR_NAKED)
//{
	//external_control_first_byte = true;
	//
	//timer_type0_stop(&TCD0);
	//
	//reti();
//}
 