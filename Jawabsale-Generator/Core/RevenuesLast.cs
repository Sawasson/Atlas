using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabsale_Generator.Core
{
    public class RevenuesLast
    {
        public int index { get; set; }
        public string created_date { get; set; }
        public string country_code { get; set; }
        public string operator_name { get; set; }
        public decimal usd_amount { get; set; }
        public decimal net_usd_amount { get; set; }
    }
}
