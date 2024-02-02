/***********
* Class: User
*
* Purpose:
*	The purpose of this class is to manage the user's information across the app.
*
* Manager Functions:
*	User()
*		Basic constructor, sets public variable "IsLoggedIn", to false upon creation
*		
*
* Methods:
*	None
*
***********/

namespace Plant_Monitor.Models
{
	public class User
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public bool IsLoggedIn { get; set; }

		/* Purpose: Constructor User, sets IsLoggedIn to false upon creation of new User object
		 * Input: None
		 * Output: IsLoggedIn is false
		 */
		public User ()
		{
			//When new user is created, automatically set to false.
			UserID = -1;
			Username = string.Empty;
			IsLoggedIn = false;
		}
	}
}
