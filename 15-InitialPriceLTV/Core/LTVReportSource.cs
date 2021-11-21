using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV.Core
{
    public class LTVReportSource
    {
        public string created_date { get; set; }
        public string Project { get; set; }
        public string source { get; set; }
        public string country_code { get; set; }
        public string site_lang { get; set; }
        public int source_count { get; set; }
        public decimal usd_amount_source { get; set; }
        public decimal source_LTV { get; set; }
    }
}
