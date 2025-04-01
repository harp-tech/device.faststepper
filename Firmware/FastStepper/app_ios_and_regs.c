#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTB, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // STOP_SWITCH
	io_pin2in(&PORTD, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // RX
	io_pin2in(&PORTC, 7, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // ENDSTOP_SWITCH

	/* Configure input interrupts */
	// io_set_int(PORT_t* port, uint8_t int_level, uint8_t int_n, uint8_t mask, bool reset_mask);
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // STOP_SWITCH
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<7), false);                 // ENDSTOP_SWITCH

	/* Configure output pins */
	io_pin2out(&PORTC, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_ENABLE
	io_pin2out(&PORTC, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_PULSE
	io_pin2out(&PORTC, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_DIRECTION

	io_pin2out(&PORTB, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // MOTOR_BRAKE


	io_pin2out(&PORTB, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // GENERIC OUTPUT 0
	io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // GENERIC OUTPUT 1


	/* Initialize output pins */
	clr_MOTOR_ENABLE;
	clr_MOTOR_PULSE;
	clr_MOTOR_DIRECTION;
	
	/* @TODO: Confirm if this one should start high or low */	
	clr_MOTOR_BRAKE;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	/* General control registers */
	TYPE_U16,
	/* Specific hardware registers */
	TYPE_I16,
	TYPE_I16,
	/* Motor specific registers */
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	/* Direct motor control */
	TYPE_U8,
	TYPE_I32,
	/* Accelerated motor control */	
	TYPE_I32,
	TYPE_I32,
	TYPE_U8,
	TYPE_I32,
	TYPE_I32,
	TYPE_I32,
	TYPE_I32,
	TYPE_I32,
	TYPE_I32,
	/* Homing control */	
	TYPE_I32,
	TYPE_U8,
	TYPE_U32,
	TYPE_U8
};

uint16_t app_regs_n_elements[] = {
	/* General control registers */
	1,
	/* Specific hardware registers */
	1,
	1,
	/* Motor specific registers */
	1,
	1,
	1,
	/* Direct motor control */
	1,
	1,
	/* Accelerated motor control */	
	1,
	7,
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	/* Homing control */	
	1,
	1,
	1,
	1
};


uint8_t *app_regs_pointer[] = {
	/* General control registers */
	(uint8_t*)(&app_regs.REG_CONTROL),
	/* Specific hardware registers */
	(uint8_t*)(&app_regs.REG_ENCODER),
	(uint8_t*)(&app_regs.REG_ANALOG_INPUT),
	/* Motor specific registers */
	(uint8_t*)(&app_regs.REG_STOP_SWITCH),
	(uint8_t*)(&app_regs.REG_MOTOR_BRAKE),
	(uint8_t*)(&app_regs.REG_MOVING),
	/* Direct motor control */
	(uint8_t*)(&app_regs.REG_STOP_MOVEMENT),
	(uint8_t*)(&app_regs.REG_DIRECT_VELOCITY),
	/* Accelerated motor control */
	(uint8_t*)(&app_regs.REG_MOVE_TO),
	(uint8_t*)(app_regs.REG_MOVE_TO_PARAMETRIC),
	(uint8_t*)(&app_regs.REG_MOVE_TO_EVENTS),
	(uint8_t*)(&app_regs.REG_MIN_VELOCITY),
	(uint8_t*)(&app_regs.REG_MAX_VELOCITY),
	(uint8_t*)(&app_regs.REG_ACCELERATION),
	(uint8_t*)(&app_regs.REG_DECELERATION),
	(uint8_t*)(&app_regs.REG_ACCELERATION_JERK),
	(uint8_t*)(&app_regs.REG_DECELERATION_JERK),
	/* Homing control */
	(uint8_t*)(&app_regs.REG_HOME_STEPS),
	(uint8_t*)(&app_regs.REG_HOME_STEPS_EVENTS),
	(uint8_t*)(&app_regs.REG_HOME_VELOCITY),
	(uint8_t*)(&app_regs.REG_HOME_SWITCH)
};