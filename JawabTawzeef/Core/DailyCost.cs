using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JawabTawzeef.Core
{
    public class DailyCost
    {
        public DateTime Date { get; set; }
        public string Dom { get; set; }
        public string Cat { get; set; }
        public string Cou { get; set; }
        public decimal SearchCost { get; set; }
        public decimal GDNCost { get; set; }
        public int SearchSubs { get; set; }
        public int GDNSubs { get; set; }
        public int Clicks { get; set; }
        public int Posted { get; set; }
        public decimal PaidCost { get; set; }
        public decimal UnPaidCost { get; set; }
        public int GDNClicks { get; set; }
        public decimal TotalCost { get; set; }

    }
}
