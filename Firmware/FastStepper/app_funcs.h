#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/

/* Register read functions */

/* General control registers */
void app_read_REG_CONTROL(void);
/* Specific hardware registers */
void app_read_REG_ENCODER(void);
void app_read_REG_ANALOG_INPUT(void);
/* Motor specific registers */
void app_read_REG_STOP_SWITCH(void);
void app_read_REG_MOTOR_BRAKE(void);
void app_read_REG_MOVING(void);
/* Direct motor control */
void app_read_REG_STOP_MOVEMENT(void);
void app_read_REG_DIRECT_VELOCITY(void);
/* Accelerated motor control */
void app_read_REG_MOVE_TO(void);
void app_read_REG_MOVE_TO_PARAMETRIC(void);
void app_read_REG_MOVE_TO_EVENTS(void);
void app_read_REG_MIN_VELOCITY(void);
void app_read_REG_MAX_VELOCITY(void);
void app_read_REG_ACCELERATION(void);
void app_read_REG_DECELERATION(void);
void app_read_REG_ACCELERATION_JERK(void);
void app_read_REG_DECELERATION_JERK(void);
/* Homing control */
void app_read_REG_HOME_STEPS(void);
void app_read_REG_HOME_STEPS_EVENTS(void);
void app_read_REG_HOME_VELOCITY(void);
void app_read_REG_HOME_SWITCH(void);


/* Register write functions */

/* General control registers */
bool app_write_REG_CONTROL(void *a);
/* Specific hardware registers */
bool app_write_REG_ENCODER(void *a);
bool app_write_REG_ANALOG_INPUT(void *a);
/* Motor specific registers */
bool app_write_REG_STOP_SWITCH(void *a);
bool app_write_REG_MOTOR_BRAKE(void *a);
bool app_write_REG_MOVING(void *a);
/* Direct motor control */
bool app_write_REG_STOP_MOVEMENT(void *a);
bool app_write_REG_DIRECT_VELOCITY(void *a);
/* Accelerated motor control */
bool app_write_REG_MOVE_TO(void *a);
bool app_write_REG_MOVE_TO_PARAMETRIC(void *a);
bool app_write_REG_MOVE_TO_EVENTS(void *a);
bool app_write_REG_MIN_VELOCITY(void *a);
bool app_write_REG_MAX_VELOCITY(void *a);
bool app_write_REG_ACCELERATION(void *a);
bool app_write_REG_DECELERATION(void *a);
bool app_write_REG_ACCELERATION_JERK(void *a);
bool app_write_REG_DECELERATION_JERK(void *a);
/* Homing control */
bool app_write_REG_HOME_STEPS(void *a);
bool app_write_REG_HOME_STEPS_EVENTS(void *a);
bool app_write_REG_HOME_VELOCITY(void *a);
bool app_write_REG_HOME_SWITCH(void *a);

#endif /* _APP_FUNCTIONS_H_ */