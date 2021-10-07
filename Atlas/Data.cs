using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas
{
    public class Data
    {
        public int Line { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CountryCode { get; set; }
        public string UtmSourceAtSubscription { get; set; }
        public string CurrencyCode { get; set; }
        public string OperatorName { get; set; }
        public bool Parked { get; set; }
        public string PeriodType { get; set; }
        public int UserID { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public string Index { get; set; }
        public decimal? Quotes { get; set; }
        public decimal? PayoutPercentage { get; set; }
        public decimal? UsdAmount { get; set; }
        public decimal? NetUsdAmount { get; set; }
        public string UTM5 { get; set; }
        public string Category { get; set; }
        public string UTM7 { get; set; }
        public string Google { get; set; }
        public string Apps { get; set; }
        public string Apps2 { get; set; }






    }
}
