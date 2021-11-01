using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public class RawDailyCost
    {
        public DateTime created_date { get; set; }
        public string country_code { get; set; }
        public decimal SearchCost { get; set; }
        public decimal GDNCost { get; set; }
        public decimal TotalCost { get; set; }


    }
}
