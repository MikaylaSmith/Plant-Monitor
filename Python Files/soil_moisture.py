#!/usr/bin/python3
##########################################
# soil_moisture.py
#
# Grabs info from light.txt for sensor values for the day
# Sets up the code for connecting to and pulling data from the soil sensor
# Runs query connection and uploads data
#
# Abbey DuBois
##########################################
import psycopg2
from config import config
from datetime import datetime
from grove.adc import ADC

class GroveSoilMoistureSensor:
	#Constructor of Python
	#This sets up the sensor with the ADC and the channel it's connected to
	def __init__(self, channel):
		self.channel = 0
		self.adc = ADC()

	#This returns what the value the adc is currently reading
	@property
	def value(self):
		return self.adc.read(self.channel)

#This is a test function as of now to see if the output is correct
def main():
	#Creates the sensor variable
	sensor = GroveSoilMoistureSensor(0)
	return sensor.value

#Connection function, runs the query
def connect(light, water, datetime, pi_id):
        sqlUpdate = "UPDATE public.\"PlantList\" SET light_level = %s, moisture_level = %s, date_and_time = %s WHERE device_id = %s; Commit;"
        sqlHistory = 'INSERT INTO public.\"PlantHistory\"(row_id, unique_plant_id, light_level, moisture_level, date) VALUES(%s, %s, %s, %s, %s); Commit;'
        sqlPlantID = 'SELECT unique_plant_id FROM public.\"PlantList\" WHERE device_id = 8'
        conn = None
        try:
                params = config()
                print('Connecting to database...')
                conn = psycopg2.connect(**params)
                cur = conn.cursor()

                print('Adding info to database...')
                cur.execute(sqlUpdate, (light, water, datetime, pi_id))

                #print('This is where the select comes in')
                cur.execute(sqlPlantID)
                unique_id = cur.fetchone()[0]
                cur.execute('SELECT COUNT(row_id) FROM public.\"PlantHistory\"')
                count = cur.fetchone()[0]
                #print('Unique plant ID: ', unique_id)
                #print('Row ID for insert: ', count + 1)
                cur.execute(sqlHistory, (count + 1, unique_id, light, water, datetime))
                cur.close()
        except(Exception, psycopg2.DatabaseError) as error:
                print(error)
        finally:
                if conn is not None:
                        conn.close()
                        print('Database connection closed')

#Main
if __name__ == '__main__':
	#Declaring an array, for grabbing values from light.txt's reading
	data = [-1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1,
		-1, -1, -1, -1, -1, -1]
	#Read in all values currently
	with open('light.txt', 'r') as nlr:
		data = [int(row) for row in nlr]
	light = 0
	count = 0
	loop = 0
	#print('array before read: ', data)
	for x in data:
		#If not empty value
		if data[loop] > -1:
			#If not dark (No "Sunlight")
			if data[loop] < 100000:
				light = light + data[loop]
				count = count + 1
		loop = loop + 1
	#print('total light: ', light)
	lightText = 'ERR: reading failed.'
	#Brightness string check
	if light < 50000:
		lightText = 'Bright Direct Light'
	elif light < 100000:
		lightText = 'Bright Indirect Light'
	elif light < 150000:
		lightText = 'Medium Light'
	else:
		lightText = 'Low Light'
	soil = GroveSoilMoistureSensor(0).value
	soilTxt = 'ERR: reading failed.'
	#Soil string check
	if soil < 400:
		soilTxt = 'Wet'
	elif soil < 500:
		soilTxt = 'Moist'
	else:
		soilTxt = 'Dry'

	#Run connection
	#print('soil: ', soil)
	connect(lightText, soilTxt, datetime.now(), 8)
	#print('All successful')
