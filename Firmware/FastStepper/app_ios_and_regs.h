#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// STOP_SWITCH            Description: Emergency stop indication

#define read_STOP_SWITCH read_io(PORTB, 0)      // STOP_SWITCH


// ENDSTOP_SWITCH        Description: Indicates the motor reached the initial/final position of the movement

#define read_HOME_SWITCH read_io(PORTC, 7)		// HOME_SWITCH



/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// MOTOR_ENABLE           Description: Enable the motor external driver
// MOTOR_PULSE            Description: Moves one step
// MOTOR_DIRECTION        Description: Sets the motor direction

/* MOTOR_ENABLE */
#define set_MOTOR_ENABLE clear_io(PORTC, 3)
#define clr_MOTOR_ENABLE set_io(PORTC, 3)
#define tgl_MOTOR_ENABLE toggle_io(PORTC, 3)
#define read_MOTOR_ENABLE read_io(PORTC, 3)

/* MOTOR_PULSE */
#define set_MOTOR_PULSE clear_io(PORTC, 0)
#define clr_MOTOR_PULSE set_io(PORTC, 0)
#define tgl_MOTOR_PULSE toggle_io(PORTC, 0)
#define read_MOTOR_PULSE read_io(PORTC, 0)

/* MOTOR_DIRECTION */
#define set_MOTOR_DIRECTION clear_io(PORTC, 6)
#define clr_MOTOR_DIRECTION set_io(PORTC, 6)
#define tgl_MOTOR_DIRECTION toggle_io(PORTC, 6)
#define read_MOTOR_DIRECTION read_io(PORTC, 6)

/* MOTOR_BRAKE */
#define set_MOTOR_BRAKE clear_io(PORTB, 3)
#define clr_MOTOR_BRAKE set_io(PORTB, 3)
#define tgl_MOTOR_BRAKE toggle_io(PORTB, 3)
#define read_MOTOR_BRAKE read_io(PORTB, 3)

/* OUTPUT_0 */
#define set_OUTPUT_0 set_io(PORTB, 1)
#define clr_OUTPUT_0 clear_io(PORTB, 1)
#define tgl_OUTPUT_0 toggle_io(PORTB, 1)
#define read_OUTPUT_0 read_io(PORTB, 1)

/* OUTPUT_1 */
#define set_OUTPUT_1 set_io(PORTA, 7)
#define clr_OUTPUT_1 clear_io(PORTA, 7)
#define tgl_OUTPUT_1 toggle_io(PORTA, 7)
#define read_OUTPUT_1 read_io(PORTA, 7)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	/* General control registers */
	uint16_t REG_CONTROL; 
	/* Specific hardware registers */
	int16_t REG_ENCODER;
	int16_t REG_ANALOG_INPUT;
	/* Motor specific registers */	
	uint8_t REG_STOP_SWITCH;
	uint8_t REG_MOTOR_BRAKE;
	uint8_t REG_MOVING;
	/* Direct motor control */	
	uint8_t REG_STOP_MOVEMENT;
	int32_t REG_DIRECT_VELOCITY;
	/* Accelerated motor control */
	int32_t REG_MOVE_TO;
	uint8_t REG_MOVE_TO_EVENTS;	
	uint16_t REG_MIN_VELOCITY;
	uint16_t REG_MAX_VELOCITY;
	int32_t REG_ACCELERATION;
	int32_t REG_DECELERATION;
	int32_t REG_ACCELERATION_JERK;
	int32_t REG_DECELERATION_JERK;
	/* Homing control */
	int32_t REG_HOME_STEPS;
	uint8_t REG_HOME_STEPS_EVENTS;
	uint32_t REG_HOME_VELOCITY;
	uint8_t REG_HOME_SWITCH;

} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* General control registers */
#define ADD_REG_CONTROL                     32 // U16    Controls the device's modules. (bitmask defined below)
/* Specific hardware registers */
#define ADD_REG_ENCODER                     33 // I16    Contains the reading of the quadrature encoder.
#define ADD_REG_ANALOG_INPUT                34 // I16    Contains the reading of the analog input.
/* Motor specific registers */
#define ADD_REG_STOP_SWITCH                 35 // U8     Contains the state of the stop switch.
#define ADD_REG_MOTOR_BRAKE                 36 // U8     Sets the state of the motor brake output.
#define ADD_REG_MOVING		                37 // U8     Contains the state of the motor movement.

/* Direct motor control */
#define ADD_REG_STOP_MOVEMENT               38 // U8     Instantly stops the motor movement.
#define ADD_REG_DIRECT_VELOCITY             39 // I32    Instantly start moving at a specific speed and direction according to the register's value and signal.

/* Accelerated motor control */
#define ADD_REG_MOVE_TO                     40 // I32    Moves to a specific position, using the velocity, acceleration and jerk configurations.
#define ADD_REG_MOVE_TO_EVENTS              41 // U8     Reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
#define ADD_REG_MIN_VELOCITY                42 // U16    Sets the minimum velocity for the movement (steps/s)
#define ADD_REG_MAX_VELOCITY                43 // U16    Sets the maximum velocity for the movement (steps/s)
#define ADD_REG_ACCELERATION                44 // I32    Sets the acceleration for the movement (steps/s^2)
#define ADD_REG_DECELERATION                45 // I32    Sets the deceleration for the movement (steps/s^2)
#define ADD_REG_ACCELERATION_JERK           46 // I32    Sets the jerk for the acceleration part of the movement (steps/s^3)
#define ADD_REG_DECELERATION_JERK           47 // I32    Sets the jerk for the deceleration part of the movement (steps/s^3)

/* Homing control */
#define ADD_REG_HOME_STEPS                  48 // I32    Moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine.											   
											   // 	     Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
#define ADD_REG_HOME_STEPS_EVENTS           49 // U8     Reports possible events regarding the execution of the REG_HOME_STEPS register.
#define ADD_REG_HOME_VELOCITY               50 // U32    Sets the fixed velocity for the homing movement (steps/s)
#define ADD_REG_HOME_SWITCH                 51 // U8     Contains the state of the home switch.



/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x33
#define APP_NBYTES_OF_REG_BANK              49

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define REG_CONTROL_B_ENABLE_MOTOR                     (1<<0)       // 
#define REG_CONTROL_B_DISABLE_MOTOR                    (1<<1)       // 
#define REG_CONTROL_B_ENABLE_ANALOG_IN                 (1<<2)       // 
#define REG_CONTROL_B_DISABLE_ANALOG_IN                (1<<3)       // 
#define REG_CONTROL_B_ENABLE_QUAD_ENCODER              (1<<4)       // 
#define REG_CONTROL_B_DISABLE_QUAD_ENCODER             (1<<5)       // 
#define REG_CONTROL_B_RESET_QUAD_ENCODER               (1<<6)       // 
#define REG_CONTROL_B_ENABLE_HOMING                    (1<<7)       //
#define REG_CONTROL_B_DISABLE_HOMING                   (1<<8)       //

#define REG_MOVE_TO_EVENTS_B_MOVE_SUCCESSFUL           (1<<0)       // Movement terminated successfully
#define REG_MOVE_TO_EVENTS_B_MOVE_ABORTED              (1<<1)       // Movement was aborted before terminating
#define REG_MOVE_TO_EVENTS_B_INVALID_POSITION          (1<<2)       // Movement can't start because position is invalid
#define REG_MOVE_TO_EVENTS_B_HOMING_MISSING            (1<<3)       // Homing is enabled and the homing routine has not happened yet
#define REG_MOVE_TO_EVENTS_B_CURRENTLY_HOMING          (1<<4)       // Movement can't start because motor is currently homing
#define REG_MOVE_TO_EVENTS_B_MOTOR_DISABLED            (1<<5)       // Movement can't start because motor is disabled


#define REG_HOME_STEPS_EVENTS_B_HOMING_SUCCESSFUL      (1<<0)       // Homing terminated successfully
#define REG_HOME_STEPS_EVENTS_B_HOMING_FAILED          (1<<1)       // Homing failed, motor moved but home position was not reached
#define REG_HOME_STEPS_EVENTS_B_ALREADY_HOME           (1<<2)       // Tried homing while already at home position
#define REG_HOME_STEPS_EVENTS_B_UNEXPECTED_HOME        (1<<3)       // Home sensor triggered unexpectedly
#define REG_HOME_STEPS_EVENTS_B_HOMING_DISABLED        (1<<4)       // Home command received but homing is disabled
#define REG_HOME_STEPS_EVENTS_B_MOTOR_DISABLED         (1<<5)       // Homing can't start because motor is disabled


#define REG_STOP_SWITCH_B_STOP_SWITCH                 (1<<0)		// 
#define B_IS_MOVING                        (1<<0)					// 
#define REG_HOME_SWITCH_B_HOME_SWITCH                  (1<<0)       //
#define B_MOTOR_BRAKE                      (1<<0)					//

#endif /* _APP_REGS_H_ */