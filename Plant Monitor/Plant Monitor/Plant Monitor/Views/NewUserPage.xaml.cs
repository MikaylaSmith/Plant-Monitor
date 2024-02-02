/***********
* Class: NewUserPage
*
* Purpose:
*	The purpose of this class is to manage the entering of new user data into the database
*
* Manager Functions:
*	NewUserPage()
*		Basic constructor
*		
*
* Methods:
*	CreateNewUser(object sender, EventArgs e)
*		Check for SQL injection attacks, then create a salt and hash the password entered, entering into the database.
*		When completed, set the newly created user to the logged in user.
*
***********/
using System;
using Plant_Monitor.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Npgsql;

namespace Plant_Monitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewUserPage : ContentPage
	{
		private NpgsqlDataReader UsersResults;

		/* Purpose: Constructor NewUserPage
		 * Input: None
		 * Output: None
		 */
		public NewUserPage()
		{
			InitializeComponent();
		}

		/* Purpose: Create new user, insert information into database, check for injection attacks
		 * Input: Button Clicked parameters
		 * Output: Navigation to Home page, or error message displayed.
		 */
		private async void CreateNewUser(object sender, EventArgs e)
		{
			string NewUsername = NewUserNameField.Text;
			string NewPassword = NewUserPasswordField.Text;
			string NewPasswordConfirm = NewUserPasswordConfirmField.Text;

			//Make new password object, making salt and hashing the confirm password
			Passwords testingNewPassword = new Passwords(NewPasswordConfirm);

			//Search for SQL injection on all fields
			bool testUsername = testingNewPassword.InjectionCheck(NewUsername);
			bool testPassword = testingNewPassword.InjectionCheck(NewPassword);
			bool testPasswordConfirm = testingNewPassword.InjectionCheck(NewPasswordConfirm);

			if (testUsername && testPassword && testPasswordConfirm)
			{
				//All fields pass the injection test
				//If passes SQL injection, test character by character the password to make sure they are the same.
				//Check that passwords match identically 
				if (NewPassword != NewPasswordConfirm)
				{
					//If not the same or there was injection on confriming, reject 
					await DisplayAlert("Passwords Do Not Match", "Please make sure they match", "Ok");
				}
				else
				{
					//If passes checks to each other and passwords match

					//Run query to see if username already exists in database.
					string GetTakenUsernamesQuery = "SELECT user_id FROM public.\"Users\" WHERE username = \'" + NewUsername + "\' LIMIT 1;";

					UsersResults = await App.Database.Select(GetTakenUsernamesQuery);

					//Check the one result. 
					int foundUserID = 0;
					while (await UsersResults.ReadAsync())
					{
						foundUserID = UsersResults.GetInt32(0);
					}


					if (foundUserID != 0)
					{
						//If there is an identical username, display message
						await DisplayAlert("Username Already Exists", "Please enter a different username", "Ok");
					}
					else
					{
						//Pull out the salt that was used, so as to be stored with user data in a table. 
						var salt = testingNewPassword.Salt;

						var hash = testingNewPassword.HashedPass;

						//Run query to insert new user into database (auto increment userid in db, save Username and hashed password)
						string InsertNewUserQuery = "INSERT INTO public.\"Users\" (\"username\", \"password\") VALUES (\'" + NewUsername + "\', \'" + hash + "\');";

						//Store the results 
						App.Database.Insert(InsertNewUserQuery);
						

						//Run query to insert salt with new user id number (in separate table, save username and/or ID with the salt used)
						string GetNewUserID = "SELECT user_id FROM public.\"Users\" WHERE username = \'" + NewUsername + "\' LIMIT 1;";
						var NewUserIDResult = await App.Database.Select(GetNewUserID);

						int UserID = 0;
						//Find the user ID that was just created for the new account
						while (await NewUserIDResult.ReadAsync())
						{
							UserID = NewUserIDResult.GetInt32(0);
						}

						//Using the newly created userID, insert the salt into the salt table
						string InsertNewSaltQuery = "INSERT INTO public.\"User_Salt\" (\"user_id\", \"salt\") VALUES (\'" + UserID + "\', \'" + salt + "\');";
						//Store the results 
						App.Database.Insert(InsertNewSaltQuery);

						//If all passes and inserted correctly, set to new user info and set to logged in
						App.User.Username = NewUsername;
						App.User.UserID = UserID;
						App.User.IsLoggedIn = true;

						//Go to home. 
						await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
						await Navigation.PopAsync();
					}
				}
			}
			else
			{
				//If any of the tests failed injection testing, go here and display message. 
				await DisplayAlert("Usernames and Passwords can only contain the following characters: ", "Capital and lowercase letters, underscores, ! and @", "Ok");
			}

			
		}
	}
}