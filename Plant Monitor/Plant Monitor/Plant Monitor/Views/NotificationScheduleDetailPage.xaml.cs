/***********
* Class: NotificationScheduleDetailPage
*
* Purpose:
*	The purpose of this class is to display the schedule data, specifically
*	schedule details of a schedule selected from the schedules database.
*
* Manager Functions:
*	NotificationScheduleDetailPage()
*		No arg basic constructor, only initializes a blank page
*		
*	NotificationScheduleDetailPage(Schedule schedule)
*		Basic constructor, passed the schedule that was selected to have
*		more details given. 
*		
*
* Methods:
*	None, all work is done solely in constructors
*
***********/
using Plant_Monitor.Models;
using Plant_Monitor.Services;
using Xamarin.Forms;
using System;

namespace Plant_Monitor.Views
{
	public partial class NotificationScheduleDetailPage : ContentPage
	{

		/* Purpose: Constructor NotificationScheduleDetailPage
		 * Input: None
		 * Output: New blank instance of the NotificationScheduleDetailPage
		 *		   with no information on it at all. 
		 */
		public NotificationScheduleDetailPage()
		{
			InitializeComponent();
		}


		/* Purpose: Constructor NotificationScheduleDetailPage
		 * Input: None
		 * Output: Values displayed on the page will be filled with the data from the selected 
		 *			schedule
		 */
		public NotificationScheduleDetailPage(Schedule schedule)
		{
			InitializeComponent();

			PlantName.Text = schedule.PlantName;
			LightLevel.Text = schedule.LightCategory;
			MoistureLevel.Text = schedule.MoistureCategory;
		}

		private void Schedule_Clicked(object sender, System.EventArgs e)
		{
			INotificationManager notification;
			notification = DependencyService.Get<INotificationManager>();

			string title = "Plant Monitor";
			string message = "Your plant is currently in an unhealthy state. \n Please add water or move to a different location in the sun. ";
			notification.SendNotification(title, message);
			//notification.SendNotification(title, message, DateTime.Now.AddSeconds(15));
		}
	}
}