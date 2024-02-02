using System;
using System.Collections.Generic;
using System.Text;

namespace Plant_Monitor.Models
{
	public class Schedule
	{
		public int Schedule_ID { get; set; }
		public int Plant_ID { get; set; }
		public string PlantName { get; set; }
		public string LightCategory { get; set; }
		public string MoistureCategory { get; set; }
		public int NotificationInterval { get; set; }
	}
}
