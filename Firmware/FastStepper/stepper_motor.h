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


// Minimum velocity accepted for the motor, in steps/s (this a limitation imposed by the way the timers are being used)
#define MOTOR_MINIMUM_VELOCITY 16

// Maximum velocity accepted for the motor, in steps/s (this can probably be increased a bit more, would need to check that if necessary)
#define MOTOR_MAXIMUM_VELOCITY 20000

// Maximum step period allowed (this corresponds to the minimum allowed velocity, a little over 15 steps/s)
#define MOTOR_MAX_STEP_PERIOD 65535

// Minimum step period allowed (this corresponds to the maximum allowed velocity, 20k steps/s)
#define MOTOR_MIN_STEP_PERIOD 50


// Default homing velocity of the motor
#define DEFAULT_HOMING_VELOCITY 400

// Default minimum velocity of the motor
#define DEFAULT_MINIMUM_VELOCITY 400

// Default maximum velocity of the motor
#define DEFAULT_MAXIMUM_VELOCITY 2000

// Default acceleration of the motor (must be a positive value)
#define DEFAULT_ACCELERATION 1000

// Default deceleration of the motor (must be a negative value)
#define DEFAULT_DECELERATION -1000

// Default acceleration jerk of the motor
#define DEFAULT_ACCELERATION_JERK 0

// Default deceleration jerk of the motor
#define DEFAULT_DECELERATION_JERK 0


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