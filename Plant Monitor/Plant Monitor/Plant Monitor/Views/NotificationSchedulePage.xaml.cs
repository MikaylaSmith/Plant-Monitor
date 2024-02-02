using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plant_Monitor.Models;
using System.Collections.ObjectModel;
using Npgsql;
using Plant_Monitor.Services;

namespace Plant_Monitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationSchedulePage : ContentPage
	{
		//ObservableCollection<Schedule> schedules;
		//private string searchFilterSelected { get; set; }

		/* Purpose: Constructor NotificationSchedulePage, initialize and display the list of schedueles for plants to display on page
		 * Input: None
		 * Output: New instance of the Schedules Page
		 */
		public NotificationSchedulePage()
		{
			InitializeComponent();

			//Initialize the observable collection
			//schedules = new ObservableCollection<Schedule>();

			//Schedule schedule = new Schedule()
			//{
			//	Schedule_ID = 0,
			//	Plant_ID = 1,
			//	PlantName = "Plant Name",
			//	LightCategory = "Light Category",
			//	MoistureCategory = "Moisture Category",
			//	NotificationInterval = 3
			//};

			//schedules.Add(schedule);

			//Go to async method to populate schedules using database
			//ListSchedules();

			//Make the Collection view set to plantscmd
			//SchedulesCollectionView.ItemsSource = schedules;

		}

		private void Schedule_Clicked(object sender, System.EventArgs e)
		{
			Plant plant = new Plant
			{
				Plant_ID = 1,
				CommonName = "Test Plant",
				Light = "Low Light",
				Moisture = "Moist", 
				DaysAtLowWater = 7
			};

			INotificationManager notification;
			notification = DependencyService.Get<INotificationManager>();

			App.Notification.NotifyCheck();
		}

		//NOT CURRENTLY IN USE BY THE PROGRAM. HERE FOR LATER ADDITIONS IF NECESSARY
		/* Purpose: Separate async method for accessing database and retrieving all of the 
		 *			data from the schedules database. 
		 *			Iterates through the results and makes schedule objects based on the 
		 *			data in the table. 
		 * Input: None
		 * Output: schedules is populated with all of the necessary schedule data
		 */
		//private async void ListSchedules()
		//{
		//	/*Order of Results:
		//	0 - Schedule_ID
		//	1 - plant_id
		//	2 - common name
		//	3 - light_category
		//	4 - moisture_category
		//	5 - interval for notifications
		//	 */

		//	//Query string
		//	string query = "SELECT * FROM public.\"NotificationSchedules\";";

		//	//Reader that gets the data from the database after the written query has been run by the App's Database
		//	NpgsqlDataReader reader = await App.Database.Select(query);

		//	//While there are still results to read, read them. 
		//	while (await reader.ReadAsync())
		//	{
		//		//Based on the data read from the database,
		//		//Make a new plant with the information 
		//		Schedule newSchedule = new Schedule
		//		{
		//			Schedule_ID = reader.GetInt32(0),
		//			Plant_ID = reader.GetInt32(1),
		//			PlantName = reader.GetString(2),
		//			LightCategory = reader.GetString(3),
		//			MoistureCategory = reader.GetString(4),
		//			NotificationInterval = reader.GetInt32(5)
		//		};

		//		//Add the new schedule that was made to the Observable Collection of schedules that
		//		//will be displayed
		//		schedules.Add(newSchedule);
		//	}
		//}

		//NOT CURRENTLY IN USE BY THE PROGRAM. HERE FOR LATER ADDITIONS IF NECESSARY
		/* Purpose: Navigate to the page to display more details about the selected plant
		 * Input: object, SelectionChangedEventArgs
		 * Output: Has been navigated to the Schedule Detail Page from the Database
		 */
		//public async void OnItemChanged(object sender, SelectionChangedEventArgs e)
		//{
		//	Schedule schedule = e.CurrentSelection.FirstOrDefault() as Schedule;
		//	if (schedule == null)
		//		return;

		//	await Navigation.PushAsync(new NotificationScheduleDetailPage(schedule));

		//}

		//NOT CURRENTLY IN USE BY THE PROGRAM. HERE FOR LATER ADDITIONS IF NECESSARY
		/* Purpose: Set the current filter for the search bar 
		 * Input: object, EventArgs
		 * Output: searchFilterSelected has the string value of whatever radio button has been selected
		 */
		//private void RadioButtonClicked(object sender, EventArgs e)
		//	{
		//		RadioButton searchFilter = (RadioButton)sender;
		//		searchFilterSelected = searchFilter.Content.ToString();
		//	}


		/* Purpose: Display filtered search results based on the entered characters and the radio button selected 
		 * Input: object, TextChangedEventArgs
		 * Output: Filtered list based on search results. 
		 */
		//public void ScheduleSearchChanged(object sender, TextChangedEventArgs e)
		//{
		//	string textValues = e.NewTextValue.ToLower();
		//	switch (searchFilterSelected)
		//	{
		//		case "Plant Name":
		//			var commonNameFilteredList = schedules.Where(p => p.PlantName.ToLower().StartsWith(textValues));

		//			SchedulesCollectionView.ItemsSource = commonNameFilteredList;
		//			break;


		//		default:
		//			var defaultFilteredList = schedules.Where(p => p.PlantName.ToLower().StartsWith(textValues));

		//			SchedulesCollectionView.ItemsSource = defaultFilteredList;
		//			break;
		//	}

		//}
	}
}