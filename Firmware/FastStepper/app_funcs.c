#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"

#include "encoder.h"
#include "stepper_motor.h"

/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;


void (*app_func_rd_pointer[])(void) = {
	/* General control registers */
	&app_read_REG_CONTROL,
	/* Specific hardware registers */
	&app_read_REG_ENCODER,
	&app_read_REG_ANALOG_INPUT,
	/* Motor specific registers */
	&app_read_REG_STOP_SWITCH,
	&app_read_REG_MOTOR_BRAKE,
	&app_read_REG_MOVING,
	/* Direct motor control */
	&app_read_REG_STOP_MOVEMENT,
	&app_read_REG_DIRECT_VELOCITY,
	/* Accelerated motor control */
	&app_read_REG_MOVE_TO,
	&app_read_REG_MOVE_TO_PARAMETRIC,
	&app_read_REG_MOVE_TO_EVENTS,
	&app_read_REG_MIN_VELOCITY,
	&app_read_REG_MAX_VELOCITY,
	&app_read_REG_ACCELERATION,
	&app_read_REG_DECELERATION,
	&app_read_REG_ACCELERATION_JERK,
	&app_read_REG_DECELERATION_JERK,
	/* Homing control */
	&app_read_REG_HOME_STEPS,
	&app_read_REG_HOME_STEPS_EVENTS,
	&app_read_REG_HOME_VELOCITY,
	&app_read_REG_HOME_SWITCH
};

bool (*app_func_wr_pointer[])(void*) = {
	/* General control registers */
	&app_write_REG_CONTROL,
	/* Specific hardware registers */
	&app_write_REG_ENCODER,
	&app_write_REG_ANALOG_INPUT,
	/* Motor specific registers */
	&app_write_REG_STOP_SWITCH,
	&app_write_REG_MOTOR_BRAKE,
	&app_write_REG_MOVING,
	/* Direct motor control */
	&app_write_REG_STOP_MOVEMENT,
	&app_write_REG_DIRECT_VELOCITY,
	/* Accelerated motor control */
	&app_write_REG_MOVE_TO,
	&app_write_REG_MOVE_TO_PARAMETRIC,
	&app_write_REG_MOVE_TO_EVENTS,
	&app_write_REG_MIN_VELOCITY,
	&app_write_REG_MAX_VELOCITY,
	&app_write_REG_ACCELERATION,
	&app_write_REG_DECELERATION,
	&app_write_REG_ACCELERATION_JERK,
	&app_write_REG_DECELERATION_JERK,
	/* Homing control */
	&app_write_REG_HOME_STEPS,
	&app_write_REG_HOME_STEPS_EVENTS,
	&app_write_REG_HOME_VELOCITY,
	&app_write_REG_HOME_SWITCH
};


/************************************************************************/
/* REG_CONTROL                                                          */
/************************************************************************/
bool reg_control_was_updated = false;
uint16_t temporary_reg_control;


void app_read_REG_CONTROL(void)
{
	uint16_t temp = 0;
	
	if (app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_MOTOR)
	{
		temp |= REG_CONTROL_B_ENABLE_MOTOR;
	}
	else
	{
		temp |= REG_CONTROL_B_DISABLE_MOTOR;
	}
	
	if (app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_ANALOG_IN)
	{
		temp |= REG_CONTROL_B_ENABLE_ANALOG_IN;
	}
	else
	{
		temp |= REG_CONTROL_B_DISABLE_ANALOG_IN;
	}
	
	if (app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_QUAD_ENCODER)
	{
		temp |= REG_CONTROL_B_ENABLE_QUAD_ENCODER;
	}
	else
	{
		temp |= REG_CONTROL_B_DISABLE_QUAD_ENCODER;
	}

	if (app_regs.REG_CONTROL & REG_CONTROL_B_ENABLE_HOMING)
	{
		temp |= REG_CONTROL_B_ENABLE_HOMING;
	}
	else
	{
		temp |= REG_CONTROL_B_DISABLE_HOMING;
	}

	app_regs.REG_CONTROL = temp;
}

extern enum MovementStatus current_movement_status;
extern bool homing_enabled;

bool app_write_REG_CONTROL(void *a)
{
	uint16_t reg = *((uint16_t*)a);	
	
	if (reg & REG_CONTROL_B_ENABLE_MOTOR)  { temporary_reg_control |=  REG_CONTROL_B_ENABLE_MOTOR; temporary_reg_control &=  ~REG_CONTROL_B_DISABLE_MOTOR; }
	if (reg & REG_CONTROL_B_DISABLE_MOTOR) { temporary_reg_control &= ~REG_CONTROL_B_ENABLE_MOTOR; temporary_reg_control |=   REG_CONTROL_B_DISABLE_MOTOR; }
	
	if (reg & REG_CONTROL_B_ENABLE_ANALOG_IN)  { temporary_reg_control |=  REG_CONTROL_B_ENABLE_ANALOG_IN; temporary_reg_control &=  ~REG_CONTROL_B_DISABLE_ANALOG_IN; }
	if (reg & REG_CONTROL_B_DISABLE_ANALOG_IN) { temporary_reg_control &= ~REG_CONTROL_B_ENABLE_ANALOG_IN; temporary_reg_control |=   REG_CONTROL_B_DISABLE_ANALOG_IN; }
	
	if (reg & REG_CONTROL_B_ENABLE_QUAD_ENCODER)  { temporary_reg_control |=  REG_CONTROL_B_ENABLE_QUAD_ENCODER; temporary_reg_control &=  ~REG_CONTROL_B_DISABLE_QUAD_ENCODER; }
	if (reg & REG_CONTROL_B_DISABLE_QUAD_ENCODER) { temporary_reg_control &= ~REG_CONTROL_B_ENABLE_QUAD_ENCODER; temporary_reg_control |=   REG_CONTROL_B_DISABLE_QUAD_ENCODER; }


	if (reg & REG_CONTROL_B_ENABLE_HOMING)  { temporary_reg_control |=  REG_CONTROL_B_ENABLE_HOMING; temporary_reg_control &=  ~REG_CONTROL_B_DISABLE_HOMING; }
	if (reg & REG_CONTROL_B_DISABLE_HOMING) { temporary_reg_control &= ~REG_CONTROL_B_ENABLE_HOMING; temporary_reg_control |=   REG_CONTROL_B_DISABLE_HOMING; }
	

	if (temporary_reg_control & REG_CONTROL_B_ENABLE_HOMING)
	{
		homing_enabled = true;
	}
	else
	{
		homing_enabled = false;
	}
	
	
	if (reg & REG_CONTROL_B_RESET_QUAD_ENCODER)
	{
		reset_quadrature_encoder();
	}
	
	if (temporary_reg_control & REG_CONTROL_B_ENABLE_MOTOR)
	{
		set_MOTOR_ENABLE;
	}
	else
	{
		if (current_movement_status != MOVEMENT_STATUS_STOPPED) stop_motor();
		clr_MOTOR_ENABLE;
	}
	
	app_regs.REG_CONTROL = temporary_reg_control;
	//reg_control_was_updated = true;
	
	return true;
}

/************************************************************************/
/* _REG_ENCODER                                                         */
/************************************************************************/
void app_read_REG_ENCODER(void)
{
	//app_regs.REG_ENCODER = 0;
}

bool app_write_REG_ENCODER(void *a)
{
	int16_t reg = *((int16_t*)a);
	
	TCD1_CNT = 0x8000 + reg;

	app_regs.REG_ENCODER = reg;
	return true;
}

/************************************************************************/
/* REG_ANALOG_INPUT                                                     */
/************************************************************************/
void app_read_REG_ANALOG_INPUT(void)
{
	//app_regs.REG_ANALOG_INPUT = 0;
}

bool app_write_REG_ANALOG_INPUT(void *a)
{
	return false;
}

/************************************************************************/
/* REG_STOP_SWITCH                                                      */
/************************************************************************/
void app_read_REG_STOP_SWITCH(void)
{
	app_regs.REG_STOP_SWITCH = (read_STOP_SWITCH) ? 0 : REG_STOP_SWITCH_B_STOP_SWITCH;
}

bool app_write_REG_STOP_SWITCH(void *a)
{
	return false;
}

/************************************************************************/
/* REG_MOTOR_BRAKE                                                      */
/************************************************************************/
void app_read_REG_MOTOR_BRAKE(void)
{
}

bool app_write_REG_MOTOR_BRAKE(void *a)
{
	uint8_t reg = *((uint8_t*)a);
	
	if (reg)
	{
		set_MOTOR_BRAKE;
	}
	else
	{
		clr_MOTOR_BRAKE;
	}
	
	return true;
}

/************************************************************************/
/* REG_MOVING                                                           */
/************************************************************************/
void app_read_REG_MOVING(void)
{
	app_regs.REG_MOVING = (TCC0_CTRLA) ? B_IS_MOVING : 0;
}

bool app_write_REG_MOVING(void *a)
{
	return false;
}

/************************************************************************/
/* REG_STOP_MOVEMENT                                                    */
/************************************************************************/
void app_read_REG_STOP_MOVEMENT(void)
{
}

bool app_write_REG_STOP_MOVEMENT(void *a)
{
	return true;
}

/************************************************************************/
/* REG_DIRECT_VELOCITY                                                  */
/************************************************************************/
void app_read_REG_DIRECT_VELOCITY(void)
{
}

extern void set_motor_step_period(int32_t period);

bool app_write_REG_DIRECT_VELOCITY(void *a)
{
	int32_t reg = *((int32_t*)a);

	set_motor_step_period(reg);
	
	return true;
}

/************************************************************************/
/* REG_MOVE_TO                                                          */
/************************************************************************/
void app_read_REG_MOVE_TO(void)
{
}

extern bool updated_target_position;
extern int32_t requested_target_position;
bool app_write_REG_MOVE_TO(void *a)
{
	// Save the requested target position update so it's processed on the main loop
	requested_target_position = *((int32_t*)a);
	updated_target_position = true;
	return true;
}

/************************************************************************/
/* REG_MOVE_TO_PARAMETRIC                                               */
/************************************************************************/
void app_read_REG_MOVE_TO_PARAMETRIC(void) {}
bool app_write_REG_MOVE_TO_PARAMETRIC(void *a)
{
	int32_t *reg = ((int32_t*)a);
	
	// Save the received parameters in the register
	app_regs.REG_MOVE_TO_PARAMETRIC[0] = reg[0];
	app_regs.REG_MOVE_TO_PARAMETRIC[1] = reg[1];
	app_regs.REG_MOVE_TO_PARAMETRIC[2] = reg[2];
	app_regs.REG_MOVE_TO_PARAMETRIC[3] = reg[3];
	app_regs.REG_MOVE_TO_PARAMETRIC[4] = reg[4];
	app_regs.REG_MOVE_TO_PARAMETRIC[5] = reg[5];
	app_regs.REG_MOVE_TO_PARAMETRIC[6] = reg[6];
	
	// Check if all the parameters are acceptable. Only start the movement if all the parameters check out
	bool result = true;
	result &= app_write_REG_MIN_VELOCITY(&reg[1]);
	result &= app_write_REG_MAX_VELOCITY(&reg[2]);
	result &= app_write_REG_ACCELERATION(&reg[3]);
	result &= app_write_REG_DECELERATION(&reg[4]);
	result &= app_write_REG_ACCELERATION_JERK(&reg[5]);
	result &= app_write_REG_DECELERATION_JERK(&reg[6]);
	
	// None of the parameters failed, let's start the movement	
	if (result)
	{
		requested_target_position = reg[0];
		updated_target_position = true;	
	}	
	return result;
}



/************************************************************************/
/* REG_MOVE_TO_EVENTS                                                   */
/************************************************************************/
void app_read_REG_MOVE_TO_EVENTS(void)
{
}

bool app_write_REG_MOVE_TO_EVENTS(void *a)
{
	return false;
}

/************************************************************************/
/* REG_MIN_VELOCITY                                                     */
/************************************************************************/
extern uint16_t motor_minimum_velocity;

void app_read_REG_MIN_VELOCITY(void)
{
}

bool app_write_REG_MIN_VELOCITY(void *a)
{
	motor_minimum_velocity = (uint16_t)*((int32_t*)a);

	if (motor_minimum_velocity < MOTOR_MINIMUM_VELOCITY)
	{
		app_regs.REG_MIN_VELOCITY = MOTOR_MINIMUM_VELOCITY;
		return false;		
	}
	if (motor_minimum_velocity > MOTOR_MAXIMUM_VELOCITY)
	{
		app_regs.REG_MIN_VELOCITY = MOTOR_MAXIMUM_VELOCITY;
		return false;
	}	
	app_regs.REG_MIN_VELOCITY = motor_minimum_velocity;	
	return true;
}

/************************************************************************/
/* REG_MAX_VELOCITY                                                     */
/************************************************************************/
extern uint16_t motor_maximum_velocity;

void app_read_REG_MAX_VELOCITY(void)
{
}

bool app_write_REG_MAX_VELOCITY(void *a)
{
	motor_maximum_velocity = (uint16_t)*((int32_t*)a);
	
	if (motor_maximum_velocity < MOTOR_MINIMUM_VELOCITY)
	{
		app_regs.REG_MAX_VELOCITY = MOTOR_MINIMUM_VELOCITY;
		return false;
	}
	if (motor_maximum_velocity > MOTOR_MAXIMUM_VELOCITY)
	{
		app_regs.REG_MAX_VELOCITY = MOTOR_MAXIMUM_VELOCITY;
		return false;
	}
	
	app_regs.REG_MAX_VELOCITY = motor_maximum_velocity;
	return true;
}

/************************************************************************/
/* REG_ACCELERATION                                                     */
/************************************************************************/
extern float motor_acceleration;

void app_read_REG_ACCELERATION(void)
{
}

bool app_write_REG_ACCELERATION(void *a)
{
	int32_t reg = *((int32_t*)a);
	app_regs.REG_ACCELERATION = reg;
	motor_acceleration = (float)reg;
	return true;	
}


/************************************************************************/
/* REG_DECELERATION                                                     */
/************************************************************************/
extern float motor_deceleration;

void app_read_REG_DECELERATION(void)
{
}

bool app_write_REG_DECELERATION(void *a)
{
	int32_t reg = *((int32_t*)a);
	app_regs.REG_DECELERATION = reg;
	motor_deceleration = (float)reg;
	return true;
}

/************************************************************************/
/* REG_ACCELERATION_JERK                                                */
/************************************************************************/
extern float motor_acceleration_jerk;

void app_read_REG_ACCELERATION_JERK(void)
{
}

bool app_write_REG_ACCELERATION_JERK(void *a)
{
	int32_t reg = *((int32_t*)a);
	app_regs.REG_ACCELERATION_JERK = reg;
	motor_acceleration_jerk = (float)reg;
	return true;
}

/************************************************************************/
/* REG_DECELERATION_JERK                                                */
/************************************************************************/
extern float motor_deceleration_jerk;

void app_read_REG_DECELERATION_JERK(void)
{
}

bool app_write_REG_DECELERATION_JERK(void *a)
{
	int32_t reg = *((int32_t*)a);
	app_regs.REG_DECELERATION_JERK = reg;
	motor_deceleration_jerk = (float)reg;
	return true;
}

/************************************************************************/
/* REG_HOME_STEPS                                                       */
/************************************************************************/
void app_read_REG_HOME_STEPS(void)
{
}

extern bool motor_is_running;
extern bool requested_homing;
extern int32_t requested_homing_distance;
bool app_write_REG_HOME_STEPS(void *a)
{
	// Will not allow to start a homing procedure if the motor is currently moving
	if (motor_is_running) return true;
	// Save the requested homing max distance so it's processed on the main loop
	requested_homing_distance = *((int32_t*)a);
	requested_homing = true;
	return true;
}

/************************************************************************/
/* REG_HOME_STEPS_EVENTS                                                */
/************************************************************************/
void app_read_REG_HOME_STEPS_EVENTS(void)
{
}

bool app_write_REG_HOME_STEPS_EVENTS(void *a)
{
	return false;
}

/************************************************************************/
/* REG_HOME_VELOCITY                                                    */
/************************************************************************/
extern uint16_t motor_homing_velocity;

void app_read_REG_HOME_VELOCITY(void)
{
}

bool app_write_REG_HOME_VELOCITY(void *a)
{
	motor_homing_velocity = (uint16_t)*((int32_t*)a);

	if (motor_homing_velocity < MOTOR_MINIMUM_VELOCITY)
	{
		app_regs.REG_HOME_VELOCITY = MOTOR_MINIMUM_VELOCITY;
		return false;
	}
	if (motor_homing_velocity > MOTOR_MAXIMUM_VELOCITY)
	{
		app_regs.REG_HOME_VELOCITY = MOTOR_MAXIMUM_VELOCITY;
		return false;
	}
	app_regs.REG_HOME_VELOCITY = motor_homing_velocity;
	return true;
}

/************************************************************************/
/* REG_HOME_SWITCH                                                      */
/************************************************************************/
void app_read_REG_HOME_SWITCH(void)
{
	app_regs.REG_HOME_SWITCH = (read_HOME_SWITCH) ? 0 : REG_HOME_SWITCH_B_HOME_SWITCH;
}

bool app_write_REG_HOME_SWITCH(void *a)
{
	return false;
}

