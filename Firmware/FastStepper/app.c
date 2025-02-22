#include "hwbp_core.h"
#include "hwbp_core_regs.h"
#include "hwbp_core_types.h"

#include "app.h"
#include "app_funcs.h"
#include "app_ios_and_regs.h"

#include "analog_input.h"
#include "encoder.h"
#include "stepper_motor.h"

#define F_CPU 32000000
#include <util/delay.h>

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;
extern uint8_t app_regs_type[];
extern uint16_t app_regs_n_elements[];
extern uint8_t *app_regs_pointer[];
extern void (*app_func_rd_pointer[])(void);
extern bool (*app_func_wr_pointer[])(void*);

/************************************************************************/
/* Initialize app                                                       */
/************************************************************************/
static const uint8_t default_device_name[] = "FastStepper";

void hwbp_app_initialize(void)
{
    /* Define versions */
    uint8_t hwH = 1;
    uint8_t hwL = 3;
    uint8_t fwH = 0;
    uint8_t fwL = 6;
    uint8_t ass = 0;
    
   	/* Start core */
    core_func_start_core(
        1225,
        hwH, hwL,
        fwH, fwL,
        ass,
        (uint8_t*)(&app_regs),
        APP_NBYTES_OF_REG_BANK,
        APP_REGS_ADD_MAX - APP_REGS_ADD_MIN + 1,
        default_device_name,
        false,	// The device is not able to repeat the harp timestamp clock
        false,	// The device is not able to generate the harp timestamp clock
        3		// Default timestamp offset
    );
}

/************************************************************************/
/* Handle if a catastrophic error occur                                 */
/************************************************************************/
void core_callback_catastrophic_error_detected(void)
{
	/* Stop motor */
	timer_type0_stop(&TCC0);
	
	/* Disable motor */
	set_MOTOR_ENABLE;
}

/************************************************************************/
/* User functions                                                       */
/************************************************************************/
/* Add your functions here or load external functions if needed */

/************************************************************************/
/* Initialization Callbacks                                             */
/************************************************************************/
void core_callback_define_clock_default(void)
{
	/* Device don't have clock input or output */
}

void core_callback_initialize_hardware(void)
{
	
	/* Initialize IOs */
	/* Don't delete this function!!! */
	init_ios();
	
	/* Initialize ADC */
	init_analog_input();
	
	/* Initialize encoder */
	init_quadrature_encoder();
	
	/* Initialize serial with 100 KHz */
	uint16_t BSEL = 19;
	int8_t BSCALE = 0;
		
	USARTD0_CTRLC = USART_CMODE_ASYNCHRONOUS_gc | USART_PMODE_DISABLED_gc | USART_CHSIZE_8BIT_gc;
	USARTD0_BAUDCTRLA = *((uint8_t*)&BSEL);
	USARTD0_BAUDCTRLB = (*(1+(uint8_t*)&BSEL) & 0x0F) | ((BSCALE<<4) & 0xF0);
	USARTD0_CTRLB = USART_RXEN_bm;
	USARTD0_CTRLA |= (INT_LEVEL_LOW << 4);
}

// Motor configuration default variaables
extern uint16_t motor_minimum_velocity;
extern uint16_t motor_maximum_velocity;
extern float motor_acceleration;
extern float motor_deceleration;
extern float motor_acceleration_jerk;
extern float motor_deceleration_jerk;


void core_callback_reset_registers(void)
{
	/* Initialize registers */
	/* General control registers */
	app_regs.REG_CONTROL = 0;
	/* Specific hardware registers */
	app_regs.REG_ENCODER = 0;
	app_regs.REG_ANALOG_INPUT = 0;
	/* Motor specific registers */
	app_regs.REG_STOP_SWITCH = 0;
	app_regs.REG_MOTOR_BRAKE = 0;
	app_regs.REG_MOVING = 0;
	/* Direct motor control */
	app_regs.REG_STOP_MOVEMENT = 0;
	app_regs.REG_DIRECT_VELOCITY = 0;
	/* Accelerated motor control */
	app_regs.REG_MOVE_TO = 0;
	app_regs.REG_MOVE_TO_EVENTS = 0;
	app_regs.REG_MIN_VELOCITY = motor_minimum_velocity;
	app_regs.REG_MAX_VELOCITY = motor_maximum_velocity;
	app_regs.REG_ACCELERATION = (int32_t)motor_acceleration;
	app_regs.REG_DECELERATION = (int32_t)motor_deceleration;
	app_regs.REG_ACCELERATION_JERK = (int32_t)motor_acceleration_jerk;
	app_regs.REG_DECELERATION_JERK = (int32_t)motor_deceleration_jerk;
	/* Homing control */
	app_regs.REG_HOME_STEPS = 0;
	app_regs.REG_HOME_STEPS_EVENTS = 0;
	app_regs.REG_HOME_VELOCITY = 0;
	app_regs.REG_HOME_SWITCH = 0;
}

extern int32_t motor_current_position;
extern int32_t motor_target_position;

void core_callback_registers_were_reinitialized(void)
{
	/* Write register that have effect on other zones of the code */
	app_write_REG_CONTROL(&app_regs.REG_CONTROL);

	// @TODO: Make sure all necessary variables are initialized here	
	//app_write_REG_NOMINAL_PULSE_INTERVAL(&app_regs.REG_NOMINAL_PULSE_INTERVAL);
	//app_write_REG_INITIAL_PULSE_INTERVAL(&app_regs.REG_INITIAL_PULSE_INTERVAL);
	//app_write_REG_PULSE_STEP_INTERVAL(&app_regs.REG_PULSE_STEP_INTERVAL);
	//app_write_REG_PULSE_PERIOD(&app_regs.REG_PULSE_PERIOD);
	
		
	/* Read external states */
	app_read_REG_STOP_SWITCH();
	app_read_REG_HOME_SWITCH();
}

/************************************************************************/
/* Callbacks: Visualization                                             */
/************************************************************************/
void core_callback_visualen_to_on(void)
{
	/* Update visual indicators */
	
}

void core_callback_visualen_to_off(void)
{
	/* Clear all the enabled indicators */
	
}

/************************************************************************/
/* Callbacks: Change on the operation mode                              */
/************************************************************************/
void core_callback_device_to_standby(void)
{
	/* Disables motor when goes to Standby Mode */
	uint8_t reg = REG_CONTROL_B_DISABLE_MOTOR;
	app_write_REG_CONTROL(&reg);
}
void core_callback_device_to_active(void) {}
void core_callback_device_to_enchanced_active(void) {}
void core_callback_device_to_speed(void) {}

/************************************************************************/
/* Callbacks: 1 ms timer                                                */
/************************************************************************/
int16_t quadrature_previous_value = 0;
int8_t endstop_previous_value = -1;

extern bool send_motor_stopped_notification;

extern bool motor_is_running;

extern float calculate_braking_distance();

extern void update_motor_velocity();

extern enum MovementStatus current_movement_status;

//uint32_t counter = 0; 

void core_callback_t_before_exec(void)
{
	/* Read ADC */
	if (app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_ANALOG_IN)
	{
		core_func_mark_user_timestamp();
		start_analog_conversion();
	}
	
	/* Read quadrature encoder */
	app_regs.REG_ENCODER = get_quadrature_encoder();
		
	if (app_regs.REG_ENCODER != quadrature_previous_value)
	{
		if (app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_QUAD_ENCODER)
		{
			core_func_send_event(ADD_REG_ENCODER, true);
		}
	}		
	quadrature_previous_value = app_regs.REG_ENCODER;
	
	/* Notify that motor is stopped */
	if (send_motor_stopped_notification)
	{		
		send_motor_stopped_notification = false;
		
		app_regs.REG_MOVING = 0;
		core_func_send_event(ADD_REG_MOVING, true);
	}
	
	set_OUTPUT_0;	
	// Check if the motor is moving and changing velocity
	// If it is, we need to keep calculating the new velocity and breaking distance
	if (motor_is_running && (current_movement_status==MOVEMENT_STATUS_ACCELERATING || current_movement_status==MOVEMENT_STATUS_DECELERATING))
	{
		float braking_distance = calculate_braking_distance();
		clr_OUTPUT_0;
		set_OUTPUT_0;
		// @TODO: This is an error situation, should never happen
		// Think on how to prevent this to begin with
		if (isnan(braking_distance))
		{
			clr_OUTPUT_0;
			set_OUTPUT_0;
		}
		clr_OUTPUT_0;
		set_OUTPUT_0;
		// Update the velocity, based on the acceleration and jerk parameters
		update_motor_velocity();
		clr_OUTPUT_0;
		set_OUTPUT_0;
		
		// Sending this two events just for debugging purposes, remove it before release
		//counter++;
		//if (counter%20==0 && braking_distance > 0)
		{
			app_regs.REG_ACCELERATION = (int32_t)braking_distance;
			core_func_send_event(ADD_REG_ACCELERATION, true);
			int32_t remaining_distance = motor_target_position - motor_current_position;
			app_regs.REG_DECELERATION = remaining_distance;
			core_func_send_event(ADD_REG_DECELERATION, true);
		}
		
	}
	clr_OUTPUT_0;
	
	/* Check if the motor endstop state changed */
	int8_t endstop_value = read_HOME_SWITCH;
	if (endstop_value != endstop_previous_value)
	{
		endstop_previous_value = endstop_value;
		if (endstop_value)
		{
			// @TODO: Is it worth to send the event on this case? Hummmm....
			/* Update register and send event */
			app_regs.REG_HOME_SWITCH = 0;
			core_func_send_event(ADD_REG_HOME_SWITCH, true);
		}
		else
		{
			/* Stop motor */
			timer_type0_stop(&TCC0);
			motor_is_running = false;
			motor_current_position = 0;
			
			// If the endstop switch was triggered while the motor was homing, that's perfect, it's what we want.
			// So in this case, we will send the success event
			if (current_movement_status==MOVEMENT_STATUS_HOMING)
			{
				app_regs.REG_HOME_STEPS_EVENTS = REG_HOME_STEPS_EVENTS_B_HOMING_SUCCESSFUL;								
			}
			// However, if the switch was triggered while we were not homing, it's an unexpected event
			else
			{
				app_regs.REG_HOME_STEPS_EVENTS = REG_HOME_STEPS_EVENTS_B_UNEXPECTED_HOME;				
			}
			// Either way, we need to send an event
			core_func_send_event(ADD_REG_HOME_STEPS_EVENTS, true);
			
			current_movement_status=MOVEMENT_STATUS_STOPPED;
				
			// @TODO: Is it worth to send the event on this case? Hummmm....
			/* Update register and send event */
			app_regs.REG_HOME_SWITCH = REG_HOME_SWITCH_B_HOME_SWITCH;
			core_func_send_event(ADD_REG_HOME_SWITCH, true);
		}		
	}
	

	
}

void core_callback_t_after_exec(void) {}
void core_callback_t_new_second(void) {}

extern bool reg_control_was_updated;
extern uint8_t temporary_reg_control;

void core_callback_t_500us(void)
{
	/* Update REG_CONTROL with the temporary register */
	/* Writing to a register happens before this function (core_callback_t_500us) */
	if (reg_control_was_updated)
	{
		reg_control_was_updated = false;
		
		app_regs.REG_CONTROL = temporary_reg_control;
	}
}

// Flag indicating that we received a new target position
bool updated_target_position = false;
// Last target position requested by the user
int32_t requested_target_position = 0;

// Flag indicating that we received a new homing request
bool requested_homing = false;
// Maximum homing distance requested by the user
int32_t requested_homing_distance = 0;


extern void move_to_target_position(int32_t target_position);
extern void move_to_home(int32_t homing_distance);

extern uint8_t home_steps_events;


void core_callback_t_1ms(void)
{
	//if ((app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_MOTOR) == false)
	//{
		///* Disable medium and high level interrupts */
		///* Medium are enough but we can win some precious cpu time here */
		//PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
		//
		///* Stop motor */
		//stop_motor();
		//
		///* Re-enable all interrupt levels */
		//PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
	//}

	// Process new requests to update the target position	
	if (updated_target_position)
	{
		updated_target_position = false;
		move_to_target_position(requested_target_position);
	}

	// Process new requests to home the motor
	if (requested_homing)
	{
		requested_homing = false;
		move_to_home(requested_homing_distance);
	}

	// If there was any home steps event to report, let's send it	
	if (home_steps_events)
	{
		app_regs.REG_HOME_STEPS_EVENTS = home_steps_events;
		core_func_send_event(ADD_REG_HOME_STEPS_EVENTS, true);
		home_steps_events = 0;
	}
	
}

/************************************************************************/
/* Callbacks: clock control                                             */
/************************************************************************/
void core_callback_clock_to_repeater(void) {}
void core_callback_clock_to_generator(void) {}
void core_callback_clock_to_unlock(void) {}
void core_callback_clock_to_lock(void) {}

/************************************************************************/
/* Callbacks: uart control                                              */
/************************************************************************/
void core_callback_uart_rx_before_exec(void) {}
void core_callback_uart_rx_after_exec(void) {}
void core_callback_uart_tx_before_exec(void) {}
void core_callback_uart_tx_after_exec(void) {}
void core_callback_uart_cts_before_exec(void) {}
void core_callback_uart_cts_after_exec(void) {}

/************************************************************************/
/* Callbacks: Read app register                                         */
/************************************************************************/
bool core_read_app_register(uint8_t add, uint8_t type)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;
	
	/* Receive data */
	(*app_func_rd_pointer[add-APP_REGS_ADD_MIN])();	

	/* Return success */
	return true;
}

/************************************************************************/
/* Callbacks: Write app register                                        */
/************************************************************************/
bool core_write_app_register(uint8_t add, uint8_t type, uint8_t * content, uint16_t n_elements)
{
	/* Check if it will not access forbidden memory */
	if (add < APP_REGS_ADD_MIN || add > APP_REGS_ADD_MAX)
		return false;
	
	/* Check if type matches */
	if (app_regs_type[add-APP_REGS_ADD_MIN] != type)
		return false;

	/* Check if the number of elements matches */
	if (app_regs_n_elements[add-APP_REGS_ADD_MIN] != n_elements)
		return false;

	/* Process data and return false if write is not allowed or contains errors */
	return (*app_func_wr_pointer[add-APP_REGS_ADD_MIN])(content);
}