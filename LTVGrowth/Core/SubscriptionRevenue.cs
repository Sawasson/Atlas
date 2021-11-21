using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTVGrowth.Core
{
    public class SubscriptionRevenue
    {


        public int next_subscription_id { get; set; }

        public int subscription_id { get; set; }

        public int is_last_sub { get; set; }

        public string utm_medium_at_subscription { get; set; }

        public int user_id { get; set; }

        public string period_type { get; set; }


        public string site_lang { get; set; }

        public string currency_code { get; set; }

        public int fully_paid { get; set; }

        public decimal currency_amount { get; set; }

        public string payment_gateway { get; set; }

        public string operator_name { get; set; }

        public int unique_id { get; set; }
        public object Id { get; set; }

        public string utm_source_at_activation { get; set; }

        public string utm_source_at_subscription { get; set; }
        public int is_demo { get; set; }

        public int is_first_sub { get; set; }


        public string first_subscription_subject_id { get; set; }

        public DateTime created_date { get; set; }

        public DateTime tpay_activated_date { get; set; }
        public string Language { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }

        public string Project { get; set; }

        public string Category { get; set; }
        //public string Country { get; set; }

        public bool IsMatched { get; set; }

        public string utm_medium_at_activation { get; set; }

        public double quotes { get; set; }




        public bool Parked { get; set; }

        public decimal usd_amount { get; set; }

        public decimal net_usd_amount { get; set; }

        public int index { get; set; }
        public string source { get; set; }
        public string UTM5 { get; set; }
        public string UTM7 { get; set; }
        public string Google { get; set; }
        public string Apps { get; set; }
        public string Apps2 { get; set; }
        public string country_code { get; set; }
        public string operator2 { get; set; }
        public string country2 { get; set; }
        public bool taboola { get; set; }
        public bool postquare { get; set; }
        public string model { get; set; }
        public DateTime first_sub_date { get; set; }
        public decimal Days7 { get; set; }
        public decimal Days14 { get; set; }
        public decimal Days30 { get; set; }
        public decimal Days60 { get; set; }
        public decimal Days90 { get; set; }
        public decimal Days120 { get; set; }
        public decimal Days150 { get; set; }
        public decimal Days180 { get; set; }
        public decimal Days210 { get; set; }
        public decimal Days240 { get; set; }
        public decimal Days270 { get; set; }
        public decimal Days300 { get; set; }
        public decimal Days330 { get; set; }
        public decimal Days360 { get; set; }
        public decimal total_spending { get; set; }
        public decimal spending_360 { get; set; }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string operator_country { get; set; }








    }

}
