/***********
* Class: Database
*
* Purpose:
*	The purpose of this class is to manage the interactions between users and the attached database.
*
* Manager Functions:
*	Database()
*		Create a connection to the Amazon Web Services (AWS) Relational Database Service (RDS) database 
*		that has been created for this project
*		
*
* Methods:
*	void Insert(string query)
*		Performs an insert operation on the passed in query.
*	Task<NpgsqlDataReader> Select(string query)
*		Gets the results for retrieving data from the database and returns to be read by 
*		whatever function called the query
*
***********/
using Plant_Monitor.Models;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Plant_Monitor.Views;
using Xamarin.Forms;

namespace Plant_Monitor.Models
{
	public class Database
	{
		public Amazon.CognitoIdentity.CognitoAWSCredentials credentials { get; private set; }
		//public Amazon.RDS.AmazonRDSClient RDSCLient { get; private set; }

		private NpgsqlDataSource dataSource { get; set; }

		/* Purpose: Constructor Database, establish and create connection to AWS RDS server and database
		 * Input: None
		 * Output: None
		 */
		public Database()
		{
			credentials = new Amazon.CognitoIdentity.CognitoAWSCredentials(AWSEnvironment.IdentityPool, Amazon.RegionEndpoint.USWest2);

			//var RDSClient = new Amazon.RDS.AmazonRDSClient(credentials, Amazon.RegionEndpoint.USWest2);

			string connectionString = "Host=" + AWSEnvironment.Hostname + ";Username=" + AWSEnvironment.Username + ";Password=" + AWSEnvironment.Password + ";Database=" + AWSEnvironment.DBName + "";
			dataSource = NpgsqlDataSource.Create(connectionString);

		}

		/* Purpose: Perform Insert query with database
		 * Input: string
		 * Output: None, but query has been performed. 
		 */
		public async void Insert(string query)
		{
			var command = dataSource.CreateCommand(query);
			await command.ExecuteNonQueryAsync();
		}
		/* Purpose: Add data into the database
		 * Input: string, string, int, int
		 * Output: None
		 */
		public async void AddData(string commonname, string nickname)
		{
			int plantid = 0;
			string scientificname = string.Empty;
			string lightlevel = string.Empty;
			string moisturelevel = string.Empty;

			string connectionString = "Host=" + AWSEnvironment.Hostname + ";Username=" + AWSEnvironment.Username + ";Password=" + AWSEnvironment.Password + ";Database=" + AWSEnvironment.DBName + "";
			//create and open connection
			NpgsqlConnection profileConnection = new NpgsqlConnection(connectionString);
			profileConnection.Open();
			//Query string
			string query = "SELECT plant_id, plant_common_name, plant_scientific_name, optimal_light_level, optimal_moisture_level FROM public.\"PlantDatabase\" WHERE plant_common_name = \'" + commonname + "\';";
			//Reader that gets the data from the database after the written query has been run by the App's Database
			NpgsqlDataReader reader = await App.Database.Select(query);

			//While there are still results to read, read them. 
			while (await reader.ReadAsync())
			{
				if (commonname == reader.GetString(1))
				{
					plantid = reader.GetInt32(0);
					commonname = reader.GetString(1);
					scientificname = reader.GetString(2);
					lightlevel = reader.GetString(3);
					moisturelevel = reader.GetString(4);
				}
				
			}

			if (scientificname == string.Empty)
			{
				return;
			}
				//create query and command
				query = "INSERT into public.\"PlantList\" (\"plant_id\",\"user_id\",\"plant_common_name\", \"plant_scientific_name\", \"light_level\", \"moisture_level\", \"active\", \"plant_nick_name\", \"device_id\") values (:plant_id, :user_id, :commonName, :scientificName, :lightlevel, :moisturelevel, :active, :nickname, :device_id);";
				NpgsqlCommand insertCommand = new NpgsqlCommand(query, profileConnection);

				//Creating the paramets for values to be added too
				insertCommand.Parameters.Add(new NpgsqlParameter("plant_id", DbType.Int16));
				insertCommand.Parameters.Add(new NpgsqlParameter("user_id", DbType.Int16));
				insertCommand.Parameters.Add(new NpgsqlParameter("commonName", DbType.String));
				insertCommand.Parameters.Add(new NpgsqlParameter("scientificName", DbType.String));
				insertCommand.Parameters.Add(new NpgsqlParameter("lightlevel", DbType.String));
				insertCommand.Parameters.Add(new NpgsqlParameter("moisturelevel", DbType.String));
				insertCommand.Parameters.Add(new NpgsqlParameter("active", DbType.Boolean));
				insertCommand.Parameters.Add(new NpgsqlParameter("nickname", DbType.String));
				insertCommand.Parameters.Add(new NpgsqlParameter("device_id", DbType.Int16));

				//Inserting values into the specified parameters
				insertCommand.Parameters[0].Value = plantid;
				insertCommand.Parameters[1].Value = App.User.UserID;
				insertCommand.Parameters[2].Value = commonname;
				insertCommand.Parameters[3].Value = scientificname;
				insertCommand.Parameters[4].Value = lightlevel;
				insertCommand.Parameters[5].Value = moisturelevel;
				insertCommand.Parameters[6].Value = true;
				insertCommand.Parameters[7].Value = " ";
				insertCommand.Parameters[8].Value = 8;
				await insertCommand.ExecuteNonQueryAsync();

				insertCommand.Dispose();
				profileConnection.Close();
		}
		public async void UpdateActive(string name)
		{
			try
			{
				string connectionString = "Host=" + AWSEnvironment.Hostname + ";Username=" + AWSEnvironment.Username + ";Password=" + AWSEnvironment.Password + ";Database=" + AWSEnvironment.DBName + "";
				NpgsqlConnection conn = new NpgsqlConnection(connectionString);
				conn.Open();

				NpgsqlCommand cmd = new NpgsqlCommand("update public.\"PlantList\" set \"active\" = :active where \"plant_common_name\" = '" + name + "' ;", conn);

				cmd.Parameters.Add(new NpgsqlParameter("active", DbType.Boolean));
				cmd.Parameters[0].Value = false;
				await cmd.ExecuteNonQueryAsync();
				cmd.Dispose();
				conn.Close();
			}

			catch (Exception ex)
			{
				await Shell.Current.GoToAsync("..");
			}
		}
		/* Purpose: Return items from database, as queried
		 * Input: string
		 * Output: Task<NpgsqlDataReader> (way to return data from async method)
		 */
		public async Task<NpgsqlDataReader> Select(string query)
		{
			var cmd = dataSource.CreateCommand(query);
			var reader = await cmd.ExecuteReaderAsync();
			return reader;
		}
	}
}
