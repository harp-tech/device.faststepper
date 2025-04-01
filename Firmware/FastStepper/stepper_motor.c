#include "stepper_motor.h"
#include "app_ios_and_regs.h"

#include "math.h"

/************************************************************************/
/* Global Parameters                                                    */
/************************************************************************/

// Current status regarding the acceleration of the motor
enum MovementStatus current_movement_status = MOVEMENT_STATUS_STOPPED;

// Flag indicating if homing is enabled on the board
bool homing_enabled = false;

// Flag indicating if homing is currently active on the board
// This is used to make sure homing is not active when leaving the home position, 
// which could cause problems because of the electrical noise on the switch
bool homing_active = false;

// Flag indicating if the homing routine has been performed since the board is active
bool homing_performed = false;

// Flag indicating if the current movement is a homing movement
bool homing_movement = false;


// Flag used by the interrupts to indicate the motor has stopped moving
bool send_motor_stopped_notification = false;

// Current velocity of the motor (updated dynamically on every step during the movement)
float motor_current_velocity = 0;
// Current acceleration of the motor (updated dynamically on every step during the movement)
float motor_current_acceleration = 0;
// Current jerk of the motor (updated dynamically on every step during the movement)
float motor_current_jerk = 0;

// Estimated braking distance for the current motor velocity
uint32_t motor_current_braking_distance = 0;

// Distance from the current position to the movement final position
uint32_t motor_distance_to_target;


// Homing velocity of the motor set by the user
uint16_t motor_homing_velocity = DEFAULT_HOMING_VELOCITY;

// Minimum velocity of the motor set by the user
uint16_t motor_minimum_velocity = DEFAULT_MINIMUM_VELOCITY;

// Maximum velocity of the motor set by the user
uint16_t motor_maximum_velocity = DEFAULT_MAXIMUM_VELOCITY;

// Acceleration of the motor set by the user
float motor_acceleration = DEFAULT_ACCELERATION;

// Deceleration of the motor set by the user
float motor_deceleration = DEFAULT_DECELERATION;

// Acceleration jerk of the motor set by the user
float motor_acceleration_jerk = DEFAULT_ACCELERATION_JERK;

// Deceleration jerk of the motor set by the user
float motor_deceleration_jerk = DEFAULT_DECELERATION_JERK;


// Period value for the stepper motor pulses (in us)
uint16_t motor_current_step_period;


extern uint8_t home_steps_events;
extern uint8_t move_to_events;
 
/************************************************************************/
/* Globals                                                              */
/************************************************************************/

// Current position of the motor (in steps)
int32_t motor_current_position = 0;
// Position the motor is trying to reach
int32_t motor_target_position = 0;

// Flag indicating if the motor is currently moving
bool motor_is_running = false;

/************************************************************************/
/* Functions                                                            */
/************************************************************************/


float calculate_braking_distance()
{
	// First we calculate the time it will take to brake, based on the current parameters
	float distance;
	
	// Since we are not braking to 0 but to the minimum velocity, we can simplify the calculations slightly
	float velocity = motor_current_velocity-motor_minimum_velocity;
		
	// If the deceleration jerk is 0, the calculations are much simpler, it's a direct formula
	if (motor_deceleration_jerk == 0)
	{
		//return (uint32_t)(current_speed*current_speed) / (2 * (int32_t)_deceleration);
		distance = pow(velocity, 2) / (2 * (-motor_deceleration));
	}
	// If the jerk is not 0, we need to do more calculations
	else
	{
		// This solution assumes the value for the velocity is positive, the acceleration is negative,
		// and the jerk can be either positive or negative
		// The time to stop can be calculating by solving the following equation
		// v0-vmin + a0*t + 1/2*j^2 = 0
		// which gives us the time it will take for the velocity to get to 0
		// So the solutions for the quadratic equation are: (-a0 +- sqrt(pow(a0, 2)-4*j*(v0-vmin)))/(2*j)
	
		// Calculation benchmarks using different variable types:
		// 105 us using floats
		// 102 us using int32_t
		// Performance is quite similar, staying with floats for better precision
		//set_OUTPUT_1;
		// First we calculate the root portion: sqrt(pow(a0, 2)-4*j*v0))	
		float root = sqrt(pow(motor_deceleration, 2)-(4*motor_deceleration_jerk*velocity));

		// If root is NAN, then the equation has no solution, the velocity can never reach zero
		if (isnan(root)) return NAN;

		// Now that we know the equation has a solution, the value we want is given by calculation using the negative root
		float time = (-motor_deceleration-root)/(2*motor_deceleration_jerk);

		// Then we calculate how many steps we take during that time, using the same exact parameters
		distance = time*((velocity) + (motor_deceleration*time/2) + (motor_deceleration_jerk*pow(time, 2)/6));		
	}	

	motor_current_braking_distance = (uint32_t)distance;
	
	//clr_OUTPUT_1;
	return distance;
}



void set_motor_step_period(int32_t period)
{
	// Make sure we don't try moving faster the maximum speed supported by the hardware
	if ((period < MOTOR_MIN_STEP_PERIOD && period > -MOTOR_MIN_STEP_PERIOD) && (period != 0))
	{
		return;
	}
	// Setting the velocity to 0 stops the timer, stopping the motor pulses
	if (period == 0)
	{
		timer_type0_stop(&TCC0);
		return;
	}
	// If all is good, we need to start/update the movement

	// Make sure the motor spins in the right direction and the velocity (period) value is positive
	(period > 0) ? (set_MOTOR_DIRECTION) : (clr_MOTOR_DIRECTION);					
	if (period < 0) period = -period;
		
	// If the timer if off, we start it running at the desired period
	if (TCC0_CTRLA == 0 || TCC0_INTCTRLB != 0)
	{			
		//timer_type0_pwm(TC0_t* timer, uint8_t prescaler, uint16_t target_count, uint16_t duty_cycle_count, uint8_t int_level_ovf, uint8_t int_level_cca);
		timer_type0_pwm(&TCC0, TIMER_PRESCALER_DIV64, (period >> 1) - 1, period >> 2, INT_LEVEL_MED, INT_LEVEL_OFF);
	}

	// Now we update the motor_current_step_period variable which is used by the interrupt to update the timers
	/* Disable medium and high level interrupts */
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
	motor_current_step_period = period;
	/* Re-enable all interrupt levels */
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
}


void move_to_target_position(int32_t target_position)
{	
	// Need to get the current motor position and set the new target position safely, 
	// since this can be called while the motor is moving
	// Disable medium and high level interrupts
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
	int32_t current_position = motor_current_position;
	motor_target_position = target_position;	
	// Re-enable all interrupt levels
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
		
	// If we are already at the target position, no need to do anything
	if (motor_target_position == current_position) return;		

	// If we do need to move, first we need to set which direction to go
	// @TODO: In the future this could contemplate a change of direction mid-movement (with deceleration)
	// The current code instantly inverts the movement using its current velocity
	(motor_target_position > current_position) ? (set_MOTOR_DIRECTION) : (clr_MOTOR_DIRECTION);

	// If the motor is currently not running, we need to reset the movement variables to the default and start the timer
	if (motor_is_running == false)	
	{
		// Initialize all the relevant variables with the initial movement settings
		motor_current_velocity = motor_minimum_velocity;
		motor_current_acceleration = motor_acceleration;
		motor_current_jerk = motor_acceleration_jerk;		
		current_movement_status = MOVEMENT_STATUS_ACCELERATING;
		
		// Set the period for the initial step, which should correspond to the minimum velocity
		motor_current_step_period = (uint16_t)(1000000/motor_current_velocity);
	
		// Start the timer with the current step period
		timer_type0_pwm(&TCC0, TIMER_PRESCALER_DIV64, (motor_current_step_period >> 1)-1, motor_current_step_period >> 2, INT_LEVEL_MED, INT_LEVEL_MED);
		motor_is_running = true;
	}
	// No matter if the motor is already running or not, we need to update the motor_target_position variable
	// that is used in the interrupts to check if the motor arrived to the destination 
	/* Disable medium and high level interrupts */
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
	motor_target_position = target_position;
	/* Re-enable all interrupt levels */
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;	
}

void move_to_home(int32_t homing_distance)
{
	// Let's set the current position to 0 and the target position as the homing distance so we can start the movement
	// Once the homing is finished, the position will reset to 0 again when the endstop is triggered
	motor_current_position = 0;	
	motor_target_position = homing_distance;
	
	// Now let's set which direction to go
	(motor_target_position > motor_current_position) ? (set_MOTOR_DIRECTION) : (clr_MOTOR_DIRECTION);

	// Initialize all the relevant variables with the initial movement settings
	// The homing movement is typically at a slow constant velocity, since we need to be able to stop instantly
	motor_current_velocity = motor_homing_velocity;
	motor_current_acceleration = 0;
	motor_current_jerk = 0;
	current_movement_status = MOVEMENT_STATUS_HOMING;
		
	// Set the period for the initial step, which should correspond to the minimum velocity
	motor_current_step_period = (uint16_t)(1000000/motor_current_velocity);
	motor_is_running = true;
		
	// Start the timer with the current step period
	timer_type0_pwm(&TCC0, TIMER_PRESCALER_DIV64, (motor_current_step_period >> 1)-1, motor_current_step_period >> 2, INT_LEVEL_MED, INT_LEVEL_MED);
}


void stop_motor(void)
{
	timer_type0_stop(&TCC0);
	motor_is_running = false;
	
	motor_current_velocity = 0;
	motor_current_acceleration = 0;
	motor_current_jerk = 0;	
	
	set_MOTOR_PULSE;
	// Send the stop notification event from the main loop, since this code runs on an interrupt
	send_motor_stopped_notification = true;
}


void update_motor_velocity()
{	
	//set_OUTPUT_0;

	// Check how many steps we still need to take until we reach the target position
	motor_distance_to_target = (motor_target_position > motor_current_position) ? (motor_target_position - motor_current_position) : (motor_current_position - motor_target_position);

	// If we are currently accelerating or moving at constant velocity, let's see if it's time to start decelerating
	// We start decelerating when the remaining distance we have to travel matches the estimated braking distance
	if (current_movement_status == MOVEMENT_STATUS_ACCELERATING || current_movement_status == MOVEMENT_STATUS_CONSTANT_VELOCITY)
	{
		// Are we at the point where we need to start decelerating because we're getting close to the target?
		if (motor_current_braking_distance >= motor_distance_to_target)
		{
			// Just change the acceleration and jerk variables into the deceleration values
			motor_current_acceleration = motor_deceleration;
			motor_current_jerk = motor_deceleration_jerk;
			current_movement_status = MOVEMENT_STATUS_DECELERATING;
		}
	}
	// If we are already decelerating, let's try to tweak the values to make the real curve matches the estimated curve
	else if (current_movement_status == MOVEMENT_STATUS_DECELERATING)
	{
		// If the estimated breaking distance is bigger than the remaining distance, we need to slow down a little harder to compensate
		if (motor_current_braking_distance > motor_distance_to_target)
		{			
			set_OUTPUT_1;	
			// Calculate a tweaking factor to be applied to the velocity. 
			// This factor needs to have a stronger effect the slower velocity in order to work properly
			float tweak = 1.0 - pow((motor_minimum_velocity/motor_current_velocity),2)/8;
			motor_current_velocity *= tweak;
			clr_OUTPUT_1;	
		}
	}
	
	// The following blocks are just some different version of the same calculations, just for benchmark comparison

	// 16 us
	//uint16_t period = (uint16_t)(1000000/motor_current_velocity);

	// 51 us
	// 48 us with jerk = 0
	// 42 us with acceleration = 0 & jerk = 0
	//motor_current_acceleration += motor_current_jerk*((float)motor_current_step_period/1000000);
	//motor_current_velocity += motor_current_acceleration*((float)motor_current_step_period/1000000);
	//motor_current_step_period = (uint16_t)(1000000/motor_current_velocity);
	
	//// 36 us
	//set_OUTPUT_1;
	//float delta = (float)motor_current_step_period/1000000;
	//motor_current_acceleration += motor_current_jerk*delta;
	//motor_current_velocity += motor_current_acceleration*delta;
	//uint32_t new_step_period = (uint32_t)(1000000/motor_current_velocity);	
	//clr_OUTPUT_1;

	//// 17 us
	//set_OUTPUT_1;
	//motor_current_acceleration += motor_current_jerk*0.0005;
	//motor_current_velocity += motor_current_acceleration*0.0005;
	//uint32_t new_step_period = (uint32_t)(1000000/motor_current_velocity);
	//clr_OUTPUT_1;

	//// 18 us
	//set_OUTPUT_1;
	//motor_current_acceleration += motor_current_jerk*0.0005;
	//motor_current_velocity += motor_current_acceleration*0.0005;
	//uint32_t new_step_period = (uint32_t)(1000000/motor_current_velocity);
	//clr_OUTPUT_1;

	// 17 us
	//set_OUTPUT_1;
	//// We assume the function is called every 500us, so the time delta is 0.0005s
	//float delta = 0.0005;
	//// Calculate the new acceleration and velocity based on the time elapsed
	//motor_current_acceleration += motor_current_jerk*delta;
	//motor_current_velocity += motor_current_acceleration*delta;
	//uint32_t new_step_period = (uint32_t)(1000000/motor_current_velocity);
	//clr_OUTPUT_1;


	// 20 us (added the maximum velocity constrain)
	//set_OUTPUT_1;
	// We assume the function is called every 500us, so the time delta is 0.0005s
	float delta = 0.0005;
	// Calculate the new acceleration and velocity based on the time elapsed
	motor_current_acceleration += motor_current_jerk*delta;
	motor_current_velocity += motor_current_acceleration*delta;
	
	// If we just exceeded maximum velocity, it means we were accelerating just now, 
	// so we need to stop the acceleration and set the velocity to the limit
	if (motor_current_velocity > motor_maximum_velocity)
	{
		motor_current_velocity = motor_maximum_velocity;
		current_movement_status = MOVEMENT_STATUS_CONSTANT_VELOCITY;
	}
	// Likewise, if we went below minimum velocity, we want to make sure we stay at
	// exactly minimum velocity. This should only happen for a maximum of a few steps
	// very close to the end of the movement
	else if (motor_current_velocity < motor_minimum_velocity)
	{
		motor_current_velocity = motor_minimum_velocity;
		current_movement_status = MOVEMENT_STATUS_CONSTANT_VELOCITY;
	}
	
	// Update the motor step period to match the final velocity
	uint32_t new_step_period = (uint32_t)(1000000/motor_current_velocity);
	//clr_OUTPUT_1;


	// Now we update the motor_current_step_period variable which is used by the interrupt to update the timers
	/* Disable medium and high level interrupts */
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm;
	motor_current_step_period = new_step_period;	
	/* Re-enable all interrupt levels */
	PMIC_CTRL = PMIC_RREN_bm | PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
}


ISR(TCC0_OVF_vect/*, ISR_NAKED*/)
{
	// Update the timer variables with the new step period that is calculates outside these interrupts
	TCC0_PER = (motor_current_step_period >> 1) - 1;
	TCC0_CCA = motor_current_step_period >> 2;			
}


ISR(TCC0_CCA_vect/*, ISR_NAKED*/)
{
	// Update the motor position depending on the direction the motor is spinning
	(motor_current_position < motor_target_position) ? motor_current_position++ : motor_current_position--;

	// The target position was reached, we can stop the motor now
	if (motor_current_position == motor_target_position)
	{
		// Stop motor
		stop_motor();
		// If we were performing a homing movement and reached the end, we got an error situation
		if (current_movement_status == MOVEMENT_STATUS_HOMING)
		{
			home_steps_events = REG_HOME_STEPS_EVENTS_B_HOMING_FAILED;
			homing_performed = false;			
		}
		// If we were not performing a homing movement, then we terminated a normal movement successfully
		else
		{
			move_to_events = REG_MOVE_TO_EVENTS_B_MOVE_SUCCESSFUL;		
		}		
		current_movement_status = MOVEMENT_STATUS_STOPPED;	
	}
}

