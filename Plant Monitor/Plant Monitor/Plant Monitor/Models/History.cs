using System;
using System.Collections.Generic;
using System.Text;

namespace Plant_Monitor.Models
{
    class History
    {
        public int Row_id { get; set; }
        public int Unique_id { get; set; }
        public string Light { get; set; }
        public string Moisture { get; set; }
    }
}
