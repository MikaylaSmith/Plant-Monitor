/***********
* Class: AppShell
*
* Purpose:
*	The purpose of this class is to define properties that are persistent across the whole app.
*
* Manager Functions:
*	AppShell()
*		Basic constructor, sets up routes to pages.
*		
*
* Methods:
*	OnLogOutClicked(object sender, EventArgs e)
*		Logs out user by removing their information and sends back to LoginPage
*	
*
***********/
using Plant_Monitor.ViewModels;
using Plant_Monitor.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Plant_Monitor
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		/* Purpose: Constructor AppShell, makes routes to pages for navigation 
		 * Input: None
		 * Output: Routes set up
		 */
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(PlantDatabaseDetailPage), typeof(PlantDatabaseDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
			Routing.RegisterRoute(nameof(NewUserPage), typeof(NewUserPage));
			Routing.RegisterRoute(nameof(PlantDatabasePage), typeof(PlantDatabasePage));
			//Routing.RegisterRoute(nameof(NotificationSchedulePage), typeof(NotificationSchedulePage));
		}

		/* Purpose: Removes currently logged in information and goes to login page. 
		 * Input: Button Clicked parameters.
		 * Output: None
		 */
		private async void OnLogOutClicked(object sender, EventArgs e)
		{
			//Destroy all current logged in user data. 
			//Allow for changing users without possible overlapping. 
			App.User.IsLoggedIn = false;
			App.User.UserID = -1;
			App.User.Username = string.Empty;
			
			await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
		}
	}
}
