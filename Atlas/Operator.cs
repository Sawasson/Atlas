using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas
{
    public class Operator
    {
        public int id { get; set; }
        public int user_id { get; set; }

        public string created_date { get; set; }

        public string period_type { get; set; }

        public string country_code { get; set; }
        public string operator_name { get; set; }

        public string tpay_activated_date { get; set; }

        public string tpay_phone { get; set; }

        public string Parked_Days { get; set; }

        public bool Parked { get; set; }
    }
}
