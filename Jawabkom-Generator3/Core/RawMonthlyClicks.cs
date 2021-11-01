using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public class RawMonthlyClicks
    {
        public DateTime created_date { get; set; }
        public string country_code { get; set; }
        public int Clicks { get; set; }
        public int GDNClicks { get; set; }
        public int Total_Clicks { get; set; }

    }
}
