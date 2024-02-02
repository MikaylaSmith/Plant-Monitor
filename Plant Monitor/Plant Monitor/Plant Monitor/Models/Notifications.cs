/***********
* Class: Notifications
*
* Purpose:
*	The purpose of this class is to manage sending out notifications for the 
*	health status of a particular plant.
*
* Manager Functions:
*	Notifications()
*		No arg basic constructor, only initializes a blank page
*		
* Methods:
*	SendNotification(string, string)
*		Sends information for notification formatting to the Dependensy Service, which 
*		in turn sends info to either Android or iOS notification system. 
*
***********/
using System.Collections.Generic;
using Xamarin.Forms;
using Plant_Monitor.Services;
using Npgsql;
using System;

namespace Plant_Monitor.Models
{
	public class Notifications
	{
		private NpgsqlDataReader reader { get; set; }
		private List<Plant> userPlantList { get; set; }

		const int DryInterval = 14;
		const int MoistInterval = 7;
		const int WetInterval = 1;

		//const int NotificationSoilTime = 18;
		//const int NotificationLightTime = 17;
		const int NotificationSoilTime = 18;
		const int NotificationLightTime = 17;
		const int Midnight = 23;

		bool NotifiedSoil = false;
		bool NotifiedLight = false;



		/* Purpose: Constructor Notifications
		 * Input: None
		 * Output: New blank instance of Notifications 
		 */
		public Notifications()
		{

		}

		/* Purpose: Check if notifications need to be sent
		 * Input: Plant object
		 * Output: Notification sent;
		 *			No notification sent
		 */
		public async void OldNotifyCheck()
		{
			//TODO: Pull from the database the info in the plant list
			//Then parse the recent information
			//Then send out notifications based on the most recent data. 

			//Check the current time for if a notification needs to be sent out. 
			int time = DateTime.Now.Hour;

			if (time > 0 && time < NotificationSoilTime)
			{
				//If not within the designated time zone, set has been notified to false
				NotifiedSoil = false;
				NotifiedLight = false;
			}
			else if (NotifiedSoil && NotifiedLight)
			{
				//If notification for soil has been sent out for the day, don't set to false
				//and if notification for soil has been sent out for the day, don't set to false
				//Keep as true
			}
			else if ((time >= NotificationSoilTime || time >= NotificationLightTime) && time <= Midnight)
			{
				//If within the time range to send out the notification
				//Greater than the hour to send the notification
				//Check to see if notification needs to be sent
				
				//SoilMoistureNotification();
				//LightLevelNotification();

				//Create query to select plant data from the list

				//TODO: Test query appears in the way that it needs to. 
				//string plantListQuery = "SELECT DISTINCT(personal_plant_id), plant_common_name, light_level, moisture_level, date_and_time FROM public.\"PlantList\" WHERE user_id = " + App.User.UserID + " " +
				//	"ORDER BY date_and_time ASC;";

				//try
				//{
				//	reader = await App.Database.Select(plantListQuery);
				//}
				//catch (Exception ex)
				//{
				//	Console.WriteLine("Error: Soil Moisture Value Select Statement - Notificiations");
				//}

				////Get plants from database
				//while (await reader.ReadAsync())
				//{
				//	//Create a new plant object with the information from the database
				//	Plant newPlant = new Plant
				//	{
				//		Plant_ID = reader.GetInt32(0),
				//		CommonName = reader.GetString(1),
				//		ScientificName = "",
				//		Light = reader.GetString(2),
				//		Moisture = reader.GetString(3),
				//		PlantInfo = "",
				//		DaysAtLowWater = 0
				//	};

				//	//Add to the a linked list of plants that can be parsed
				//	userPlantList.Add(newPlant);
				//}

				////Check if there is anything in the list.
				//if (userPlantList != null)
				//{
				//	//If the list isn't empty
				//	//Iterate through the list of plants from the database and check
				//	//If plants need to have notifications sent out. 
				//	foreach (Plant plant in userPlantList)
				//	{
				//		//Do the time check again for specifically soil
				//		if (time >= NotificationSoilTime && time <= Midnight && !NotifiedSoil)
				//		{
				//			NotifiedSoil = true;
				//SoilMoistureNotification(plant);
				//}

				//Do the time check again for specifically light 
				//if (time >= NotificationLightTime && time <= Midnight && !NotifiedLight)
				//{
				//	NotifiedLight = true;
				//LightLevelNotification(plant);
				//}
			}
				//}
			//}
		}

		/* Purpose: Check if notifications need to be sent
		 * Input: Plant object
		 * Output: Notification sent;
		 *			No notification sent
		 */
		public void NotifyCheck()
		{
			//TODO: Pull from the database the info in the plant list
			//Then parse the recent information
			//Then send out notifications based on the most recent data. 

			//Check the current time for if a notification needs to be sent out. 
			int time = DateTime.Now.Hour;

			if (time > 0 && time < NotificationSoilTime)
			{
				//If not within the designated time zone, set has been notified to false
				NotifiedSoil = false;
				NotifiedLight = false;
			}
			else if (NotifiedSoil && NotifiedLight)
			{
				//If notification for soil has been sent out for the day, don't set to false
				//and if notification for soil has been sent out for the day, don't set to false
				//Keep as true
			}
			else if ((time >= NotificationSoilTime || time >= NotificationLightTime) && time <= Midnight)
			{
				//If within the time range to send out the notification
				//Greater than the hour to send the notification
				//Check to see if notification needs to be sent

				SimpleLightLevelNotification();
				SimpleSoilMoistureNotification();

				NotifiedLight = true;
				NotifiedSoil = true;
			}
		}

		/* Purpose: Send Notification
		 * Input: string, string
		 * Output: New notification is output according to operating system 
		 *			and it goes to format and display the proper notification 
		 *			to the user. Function itself returns void.
		 */
		private void SendNotification(string title, string message)
		{
			INotificationManager notification;
			notification = DependencyService.Get<INotificationManager>();

			notification.SendNotification(title, message);
		}

		/* Purpose: Send a notification for soil moisture level
		 * Input: 
		 * Output: A notification 
		 */
		private void SimpleSoilMoistureNotification()
		{
			string title = "Plant Monitor - Soil Moisture";
			string message = "Check the moisture of your plants";
			SendNotification(title, message);
		}

		/* Purpose: Send a notification for light level
		 * Input: None
		 * Output: A notification 
		 */
		private void SimpleLightLevelNotification()
		{
			string title = "Plant Monitor - Light Level";
			string message = "Might be getting too much or too little light.";
			SendNotification(title, message);
		}

		/* Purpose: Test if it is time to send out a soil moisture notification based on the current status of a plant
		 * Input: Plant object
		 * Output: A notification 
		 */
		private async void SoilMoistureNotification(Plant plant)
		{
			//Basic set up for commonly resued variable
			string plantName = plant.CommonName;
			int plantDays = plant.DaysAtLowWater;
			string title = "Plant Monitor - Your " + plantName + " needs water";
			string message;
			string typicalMoistureLevel = "";

			//Go to the plant database and get the 
			string soilMoistureQuery = "SELECT optimal_moisture_level FROM public.\"PlantDatabase\" WHERE plant_id = " + plant.Plant_ID + " LIMIT 1;";

			try
			{
				reader = await App.Database.Select(soilMoistureQuery);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: Soil Moisture Value Select Statement - Notificiations");
			}

			//While there are still results to read, read them. 
			while (await reader.ReadAsync())
			{
				typicalMoistureLevel = reader.GetString(0);
			}

			//This function will contain the necessary prodecure to determine if a plant needs to have its health status changed at all. 
			switch (typicalMoistureLevel)
			{
				//For the Soil Mosture Level Dry that only needs a notification once the water level has reached low for about 2 weeks 
				case "Dry":
					if (plantDays == DryInterval)
					{
						message = "" + plantName + " hasn't been watered in " + DryInterval + " days.";
						SendNotification(title, message);
					}
					break;

				//For the Soil Mosture Level Moist that needs water after being at a low level [time frame] 
				case "Moist":
					if (plantDays == MoistInterval)
					{
						message = "" + plantName + " hasn't been watered in " + MoistInterval + " days.\n";
						SendNotification(title, message);
					}
					break;

				//For the Soil Mosture Level Moist that needs water after being at a low level [time frame] 
				case "Wet":
					if (plantDays == WetInterval)
					{
						message = "" + plantName + " is in need of water today.";
						SendNotification(title, message);
					}
					break;

				default:
					//Default Notification Message
					title = "Plant Monitor - Your " + plantName +" needs water";
					message = "" + plantName + " needs water. Please add some water soon.";
					SendNotification(title, message);
					break; 
			}
		}

		/* Purpose: Test if it is time to send out a light level notification based on the current status of a plant
		 * Input: Plant object
		 * Output: A notification 
		 */
		private async void LightLevelNotification(Plant plant)
		{
			//Set up for basic and reused variables
			string plantName = plant.CommonName;
			string currentLightAmount = plant.Light;
			string title = "Plant Monitor - Your " + plantName + " has had a change in light levels.";
			string message = "";
			string typicalLightLevel = "";

			string lightLevelQuery = "SELECT optimal_light_level FROM public.\"PlantDatabase\" WHERE plant_id = " + plant.Plant_ID + " LIMIT 1;";

			try
			{
				reader = await App.Database.Select(lightLevelQuery);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: Soil Moisture Value Select Statement - Notifications");
			}

			//While there are still results to read, read them. 
			while (await reader.ReadAsync())
			{
				typicalLightLevel = reader.GetString(0);
			}

			//Check the category of light that the plant is registered to. 
			switch(typicalLightLevel)
			{
				//If registered to low light, if not the same, it will be brighter
				case "Low Light":
					if (currentLightAmount != typicalLightLevel)
					{
						message = "" + plantName + " is currently getting too much light. Please move it to a darker area if possible.";
						SendNotification(title, message);
					}
					break;

				//Medium exists in the middle so it needs to have different messages depending if in too low of light or too bright of a light.
				case "Medium Light":
					if (currentLightAmount != typicalLightLevel)
					{
						//If not the correct category, check for if in low light
						if (currentLightAmount == "Low Light")
						{
							message = "" + plantName + " is currently getting too little light. Please move it to a brighter area if possible.";
						}
						//Check for if in bright light
						else if (currentLightAmount == "Bright Indirect Light")
						{
							message = "" + plantName + " is currently getting too much light. Please move it to a darker area if possible.";
						}
						SendNotification(title, message);
					}
					break;

				//Bright only has it is darker than needed
				case "Bright Indirect Light":
					if (currentLightAmount != typicalLightLevel)
					{
						message = "" + plantName + " is currently getting too little light. Please move it to a brighter area if possible.";
						SendNotification(title, message);
					}
					break;

				//Send a default message when there is no condition met.
				default:
					title = "Plant Monitor - Your " + plantName + "'s light level has changed from a healthy state. ";
					message = "" + plantName + " is either getting too much or too little light. Please alter the amount it is getting soon.";
					SendNotification(title, message);
					break;
			}

		}
	}
}
