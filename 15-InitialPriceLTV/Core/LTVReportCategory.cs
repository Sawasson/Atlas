using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV.Core
{
    public class LTVReportCategory
    {
        public string Project { get; set; }
        public string source { get; set; }
        public string country_code { get; set; }
        public string CategoryName { get; set; }
        public string site_lang { get; set; }
        public int category_count { get; set; }
        public decimal usd_amount_category { get; set; }
        public decimal category_LTV { get; set; }
        public int country_count { get; set; }
        public decimal usd_amount_country { get; set; }
        public decimal country_LTV { get; set; }


    }
}
