#!/usr/local/bin/python

import RPi.GPIO as GPIO #, time, os </code>
import time
import os
import datetime

#Defines the GPIO numbering scheme for Rasp4B
GPIO.setmode(GPIO.BOARD)

#The pin that is connected to the circuit on the board
pin_to_circuit = 7

#Function to get the value of the photoresistor
#Will increment the number of seconds it takes for the capacitor to charge up

def rc_time (pin_to_circuit):
	count = 0

	#This says that pin 7 can be used as an output
	GPIO.setup(pin_to_circuit, GPIO.OUT)

	#This sets the level of the pin to off
	GPIO.output(pin_to_circuit, GPIO.LOW)
	time.sleep(0.1)

	#Resets the pin 7 back to an input
	GPIO.setup(pin_to_circuit, GPIO.IN)

	#While the pin is low/off count
	#Will stop once the capacitor starts to discharge
	while(GPIO.input(pin_to_circuit) == GPIO.LOW):
		count += 1

	return count

#This is the main code for the program
#Will run the above function once then print the value
#The value is in seconds, basically printing out the seconds it takes for the capacitor to go high
#This relationship between the seconds is the less light the longer the charge time, the more light the less time

try:
	#This is where you'll take this value and use it in the connection program, I just have it to print right now
	light = [-1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1]
	hour = (datetime.datetime.now().hour)
	#second = datetime.datetime.now().second
	print(hour)
	with open('light.txt', 'r') as nlr: 
		light = [int(row) for row in nlr]
		avg = [-1, -1, -1]
		counter = 0
		for x in avg:
			avg[counter] = rc_time(pin_to_circuit)
			counter = counter + 1
			time.sleep(0.1)
		light[hour] = int((avg[0] + avg[1] + avg[2]) / 3)
	with open('light.txt', 'w') as nlw:
		for line in light:
			print("{}".format(line), file=nlw)
	print("Array: ", light)
finally:
	GPIO.cleanup()
#Basic clean up of pins when done (setting them back to original values)
