using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JawabMehan.Core
{
    public class SubscriptionRevenue
    {
        public object Id { get; set; }
        public int user_id { get; set; }
        //public int subscription_id { get; set; }
        //public int next_subscription_id { get; set; }
        public string created_date { get; set; }
        public DateTime created_datex { get; set; }

        //public string created_time { get; set; }
        public int is_first_sub { get; set; }
        //public int is_last_sub { get; set; }
        //public string utm_source_at_activation { get; set; }
        //public string utm_medium_at_activation { get; set; }
        public string utm_source_at_subscription { get; set; }
        //public string utm_medium_at_subscription { get; set; }
        //public string expired_time { get; set; }
        //public string expired_date { get; set; }
        public string period_type { get; set; }
        public string site_lang { get; set; }
        public int is_demo { get; set; }
        public string country_code { get; set; }
        public string currency_code { get; set; }
        public int fully_paid { get; set; }
        public decimal currency_amount { get; set; }
        //public string auto_renewal { get; set; }
        //public string subscription_log_type { get; set; }
        //public string status { get; set; }
        //public string charging_status { get; set; }
        public string payment_gateway { get; set; }
        //public string unsubscribed_by { get; set; }
        public int first_subscription_subject_id { get; set; }
        //public string utm_source_first { get; set; }
        //public string utm_source_last { get; set; }
        //public string utm_medium_first { get; set; }
        //public string utm_medium_last { get; set; }
        //public string question_utm_source { get; set; }
        //public string question_utm_medium { get; set; }
        //public int ei_question_id { get; set; }
        //public string ei_question_source { get; set; }
        //public int ei_step3_view_id { get; set; }
        //public string ei_language_code { get; set; }
        //public string flow_name { get; set; }
        //public string ei_google_keyword { get; set; }
        //public string ei_google_placement { get; set; }
        //public string gclid { get; set; }
        //public string funnel_lp_slug { get; set; }
        //public int phone_confirmed_by_tpay_enreachment { get; set; }
        public string tpay_activated_date { get; set; }
        //public string tpay_activated_time { get; set; }
        //public string approval_method { get; set; }
        public string operator_name { get; set; }
        //public int tpay_operator_detail_id { get; set; }
        //public string ei_search_term_source { get; set; }
        //public string tpay_contract_id { get; set; }
        //public string tpay_phone { get; set; }
        //public string email { get; set; }



        public bool Parked { get; set; }
        public decimal usd_amount { get; set; }
        public decimal net_usd_amount { get; set; }
        public string UTM5 { get; set; }
        public string UTM7 { get; set; }
        public string Category { get; set; }
        public string Google { get; set; }
        public string Apps { get; set; }
        public string Apps2 { get; set; }

        //public string lang { get; set; }
        public string source { get; set; }

        public string operator2 { get; set; }
        public string country2 { get; set; }
        public bool taboola { get; set; }
        public bool postquare { get; set; }
        public string model { get; set; }
        //public string domain { get; set; }
        public int index { get; set; }
        //public double quotes { get; set; }
        //public string payout_percentage { get; set; }

    }
}
