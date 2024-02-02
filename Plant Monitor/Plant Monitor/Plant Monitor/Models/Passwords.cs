/***********
* Class: Passwords
*
* Purpose:
*	The purpose of this class is to manage the creation of salts and hashed passwords for users.
*
* Manager Functions:
*	Passwords(string PassToHash)
*		Passes in the password with which the salting and hashing happens.
*		
*
* Methods:
*	bool InjectionCheck(Char [] stringToCheck)
*		Checks character by character through the string for any "bad" characters
*	string SaltPass()
*		Makes a salt for the password
*	string Hash(string PassToHash, string salt, int iterations, int hashSize)
*		Creates hash with the password string, salt string, the number of iterations, and the hash size. 
*
***********/
using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Plant_Monitor.Models
{
	class Passwords
	{
		//Define constants
		private const int SALT_SIZE = 64;
		private const int HASH_SIZE = 64;
		private const int ITERATION_SIZE = 10000;

		public string HashedPass { get; private set; }
		public string Salt { get; private set; }

		/* Purpose: Constructor Passwords, Only creates new Passwords object, doesn't make new hashed password
		 * Input: none
		 * Output: New Passwords object with no members set
		 */
		public Passwords()
		{

		}


		/* Purpose: Constructor Passwords, sets up salt and hash for the passed in password
		 * Input: string
		 * Output: None, but publicly accessible variables are set.
		 */
		public Passwords(string PassToHash)
		{
			//Make a Salt
			Salt = SaltPass();

			//Hash Password and Salt
			HashedPass = Hash(PassToHash, Salt, ITERATION_SIZE);
		}

		/* Purpose: Checks character by character through the string for any "bad" characters
		 * Input: string needing to be checked
		 * Output: boolean value for if the test passed at all
		 */
		public bool InjectionCheck(string stringToCheck)
		{
			bool InjectionPass = false;

			//Check for if is only capital and lowercase letters, numbers, and !, @ and _
			if (Regex.IsMatch(stringToCheck, @"^[a-zA-Z!@_0-9]+$"))
			{
				//If contains only these, set to true
				InjectionPass = true;
			}

			//Return the status of the test
			return InjectionPass;
		}

		/* Purpose: Creates salt for password
		 * Input: None
		 * Output: string for salt
		 */
		private string SaltPass()
		{
			var BytesVar = new byte[SALT_SIZE];
			var RandomNumbers = new RNGCryptoServiceProvider();
			RandomNumbers.GetBytes(BytesVar);
			return Convert.ToBase64String(BytesVar);
		}

		/* Purpose: Take salt and passed in password and hash into secure password string
		 * Input: string, string, int
		 * Output: string for hashed password
		 */
		private static string Hash(string PassToHash, string salt, int iterations)
		{
			var passwordString = Encoding.UTF8.GetBytes(PassToHash);
			var saltString = Encoding.UTF8.GetBytes(salt);

			var hasher = new Rfc2898DeriveBytes(passwordString, saltString, iterations);

			return Convert.ToBase64String(hasher.GetBytes(HASH_SIZE));

		}

		public string hashedPass(string PassToHash, string salt)
		{
			return Hash(PassToHash, salt, ITERATION_SIZE);
		}


		//Code from Nick Proud
		//https://www.automationmission.com/2020/09/17/hashing-and-salting-passwords-in-c/
	}
}
