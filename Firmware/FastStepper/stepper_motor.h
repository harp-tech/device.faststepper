#ifndef _STEPPER_MOTOR_H_
#define _STEPPER_MOTOR_H_
#include <avr/io.h>

// Define if not defined
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
	#define false 0
#endif

// Enumeration to specify the status of the current movement
enum MovementStatus {MOVEMENT_STATUS_STOPPED, MOVEMENT_STATUS_ACCELERATING, MOVEMENT_STATUS_DECELERATING, MOVEMENT_STATUS_CONSTANT_VELOCITY, MOVEMENT_STATUS_HOMING};


// Move the motor with a specific fixed interval between each step
void set_motor_step_period(int32_t period);

// Move the motor to a specific position
void move_to_target_position(int32_t target_position);

// Move the motor to the home position (where the endstop switch activates)
void move_to_home(int32_t homing_distance);

// Calculate the braking distance necessary to go from the the current speed to the minimum speed
float calculate_braking_distance();

// Update the current velocity of the motor, based on the acceleration and jerk settings
void update_motor_velocity();

// Immediately stop the motor 
void stop_motor();


#endif /* _STEPPER_MOTOR_H_ */