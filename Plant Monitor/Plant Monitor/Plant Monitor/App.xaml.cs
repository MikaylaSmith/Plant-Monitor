/***********
* Class: App
*
* Purpose:
*	The purpose of this class is to manage the entire app and its functions and functionality.
*
* Manager Functions:
*	App()
*		Basic constructor, sets User and the main page of the app
*		
*
* Methods:
*	OnStart()
*		Defines actions for starting the app fresh.
*	OnSleep()
*		Defines actions for the app when it goes to sleep. 
*	OnResume()
*		Defines actions for the app when returning to it without starting new. 
*
***********/
using Plant_Monitor.Services;
using Plant_Monitor.Views;
using Plant_Monitor.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace Plant_Monitor
{
	public partial class App : Application
	{
		public static User User { get; set; }
		static Database database { get; set; }
		static Notifications notifications { get; set; }


		/* Purpose: Sets singleton instances of the database, so only one for app lifetime
		 * Input: None
		 * Output: Database instance returned
		 */
		public static Database Database
		{
			get
			{
				if (database == null)
				{
					database = new Database();
				}
				return database;
			}
		}

		/* Purpose: Sets singelton instance of notification "manager"
		 * Input: None
		 * Output: Notification "manager" returned
		 */
		public static Notifications Notification
		{
			get
			{
				if (notifications == null)
				{
					notifications = new Notifications();
				}
				return notifications;
			}
		}

		/* Purpose: Constructor App, creates new global user profile, sets Main Page property.
		 * Input: None
		 * Output: None
		 */
		public App()
		{
			InitializeComponent();

			User = new User();

			DependencyService.Register<MockDataStore>();
			
			MainPage = new AppShell();

			Notification.NotifyCheck();
		}

		/* Purpose: Determines path based on IsLoggedIn status. 
		 * Input: None
		 * Output: Either Home page, or LoginPage
		 */
		protected override async void OnStart()
		{
			if (User.IsLoggedIn == true)
			{
				//On starting the app, if is logged in is true, go to home
				//NotifyCheck();
				await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
			}
			else
			{
				//If not logged in, go to login page
				await Shell.Current.GoToAsync("//LoginPage");
			}
		}

		/* Purpose: Determines path based on IsLoggedIn status. 
		 * Input: None
		 * Output: Either Home page, or LoginPage
		 */
		protected override async void OnSleep()
		{
			if (User.IsLoggedIn == true)
			{
				//On starting the app, if is logged in is true, go to home
				//NotifyCheck();
				await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
			}
			else
			{
				//If not logged in, go to login page
				await Shell.Current.GoToAsync("//LoginPage");
			}
		}

		/* Purpose: Determines path based on IsLoggedIn status. 
		 * Input: None
		 * Output: Either Home page, or LoginPage
		 */
		protected override async void OnResume()
		{
			if (User.IsLoggedIn == true)
			{
				//On starting the app, if is logged in is true, go to home
				//NotifyCheck();
				await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
			}
			else
			{
				//If not logged in, go to login page
				await Shell.Current.GoToAsync("//LoginPage");
			}
		}
	}
}
