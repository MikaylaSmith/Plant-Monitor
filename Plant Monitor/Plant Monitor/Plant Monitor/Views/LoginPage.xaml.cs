/***********
* Class: LoginPage
*
* Purpose:
*	The purpose of this class is to manage the login page for the app.
*
* Manager Functions:
*	LoginPage()
*		Basic constructor
*		
*
* Methods:
*	OnLogIn(object sender, EventArgs e)
*		Performs actions to prevent SQL injection attacks, then queries database for user information, checking if the right user.
*		Sets the user for the app if all tests pass
*
***********/
using Plant_Monitor.ViewModels;
using System;
using Plant_Monitor.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Npgsql;

namespace Plant_Monitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		private bool UserFound = false;
		private NpgsqlDataReader UsersResults;
		private NpgsqlDataReader SaltResults;

		/* Purpose: Constructor LoginPage
		 * Input: None
		 * Output: New instance of the Login Page
		 */
		public LoginPage()
		{
			InitializeComponent();
			this.BindingContext = new LoginViewModel();
			
			App.Notification.NotifyCheck();
		}

		/* Purpose: Query Users data table and get information on the users stored in the table
		 * Input: Username string, Password string 
		 * Output: If true, go to Home. If false, return
		 */
		private async void OnLogIn(object sender, EventArgs e)
		{
			//Check username and password for SQL injections
			string Username = UserNameField.Text;
			string Password = PasswordField.Text;

			//Check if what was submitted is blank
			if (Username == null || Password == null) 
			{
				//If blank, display alert and go back
				await DisplayAlert("Field is Blank", "There is a field left blank.\nPlease Enter the Login Information", "Ok");
			}
			else
			{
				//If not blank

				//Make New Passwords object to test and use the functions of
				Passwords testingPassword = new Passwords();

				//Set Injection Status of both username and password
				bool InjectionTestUsername = testingPassword.InjectionCheck(Username);
				bool InjectionTestPassword = testingPassword.InjectionCheck(Password);

				//Test if there are "bad" characters in the strings that were submitted
				if (!InjectionTestUsername || !InjectionTestPassword)
				{
					//Display a different message based on where the invalid character is
					if (!InjectionTestUsername)
					{
						await DisplayAlert("Username Contains Invalid Characters", "Only capital and lowercase letters, underscores, ! and @ are allowed.\nPlease Enter Valid Information", "Ok");
					}
					else if (!InjectionTestPassword)
					{
						await DisplayAlert("Password Contains Invalid Characters", "Only capital and lowercase letters, underscores, ! and @ are allowed.\nPlease Enter Valid Information", "Ok");
					}
				}
				else
				{
					//If passes the injection tests

					//Run Query to get user data from database
					string GetUserInfoQuery = string.Empty;
					GetUserInfoQuery = "SELECT user_id, username, password FROM public.\"Users\" WHERE username = \'" + Username + "\' LIMIT 1;";

					try
					{
						//If able to access, store results
						UsersResults = await App.Database.Select(GetUserInfoQuery);
					}
					catch
					{
						//If there is an error, display message and return from function
						Console.WriteLine("Error Reading from database - LoginPage");
						await DisplayAlert("Database Error", "Error Getting Information From Database. Please try again later. ", "Ok");
						PasswordField.Text = string.Empty;
						return;
					}

					int queryUser_ID = -1;
					string queryUsername = string.Empty;
					string queryPassword = string.Empty;

					//Read the result and set to variables
					while (await UsersResults.ReadAsync())
					{
						queryUser_ID = UsersResults.GetInt32(0);
						queryUsername = UsersResults.GetString(1);
						queryPassword = UsersResults.GetString(2);
					}

					UsersResults = null;

					//Run Query to get Salt info 
					string GetUserSaltQuery = string.Empty;
					GetUserSaltQuery = "SELECT salt FROM public.\"User_Salt\" WHERE user_id = \'" + queryUser_ID + "\' LIMIT 1";

					try
					{
						//Store the results 
						SaltResults = await App.Database.Select(GetUserSaltQuery);
					}
					catch
					{
						//If there is an error, display message and return from function
						Console.WriteLine("Error Reading from database - LoginPage");
						await DisplayAlert("Database Error", "Error Getting Information From Database. Please try again later. ", "Ok");
						PasswordField.Text = string.Empty;
						return;
					}

					string salt = string.Empty;

					//If the salt does exist
					//Read the results
					while (await SaltResults.ReadAsync())
					{
						salt = SaltResults.GetString(0);
					}

					SaltResults = null;

					//Make an identical hashed password to match the one in the database
					string testHashedPassword = testingPassword.hashedPass(Password, salt);

					//Test if the queried password matches and double check that the username entered still matches
					if (queryPassword == testHashedPassword && queryUsername == Username)
					{
						//If matches, set user to "Found" and proceed to finish logging in
						UserFound = true;
					}

					if (UserFound)
					{
						//Set user information here
						App.User.UserID = queryUser_ID;
						App.User.Username = queryUsername;
						App.User.IsLoggedIn = true;

						PasswordField.Text = string.Empty;
						UserFound = false;

						//Navigation to home page
						await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
					}
					else
					{
						await DisplayAlert("Username or Password Does Not Match", "Please Enter the Correct Information", "Ok");
						PasswordField.Text = string.Empty;
						return;
					}
				}
			}
		}

		/* Purpose: Pass to NewUserPage to create a new user
		 * Input: Button Press Input
		 * Output: On NewUserPage
		 */
		private async void OnNewUserClicked(object sender, EventArgs e)
		{
			//Redirect to new user creation page
			var CreateNewUser = new NewUserPage();
			await Navigation.PushAsync(CreateNewUser);
		}
	}
}