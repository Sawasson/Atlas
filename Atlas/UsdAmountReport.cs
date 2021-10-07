using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas
{
    public class UsdAmountReport
    {
        public DateTime CreatedDate { get; set; }
        public string CountryCode { get; set; }
        public string OperatorName { get; set; }
        public string Category { get; set; }
        public decimal? TotalUsdAmount { get; set; }
        public decimal? TotalNetUsdAmount { get; set; }


    }
}
