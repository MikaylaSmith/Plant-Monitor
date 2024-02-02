/***********
* Class: Plant
*
* Purpose:
*	The purpose of this class is to hold the information about a plant stored in the database.
*
* Manager Functions:
*		
* Methods:
*	None
*
***********/

namespace Plant_Monitor.Models
{
	public class Plant
	{
		public int Plant_ID { get; set; }
		public string Light { get; set; }
		public string Moisture { get; set; }
		public string UserDefinedName { get; set; }
		public string CommonName { get; set; }
		public string ScientificName { get; set; }
		public string PlantInfo { get; set; }
		public int DaysAtLowWater { get; set; } = 0;
		public bool Active { get; set; }
	}
}
