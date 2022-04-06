using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawiyyahGenerator.Core
{
    public class RevenuesLast
    {
        [Key]
        public ObjectId Id { get; set; }
        public int index_0 { get; set; }
        public string created_date { get; set; }
        public string payment_gateway { get; set; }
        public string country_code { get; set; }
        public string utm_source_at_subscription { get; set; }
        public string currency_code { get; set; }
        public string operator_name { get; set; }
        public bool Parked { get; set; }
        public int user_id { get; set; }
        public decimal currency_amount { get; set; }
        public int index { get; set; }
        public double quotes { get; set; }
        public string payout_percentage { get; set; }
        public decimal usd_amount { get; set; }
        public decimal net_usd_amount { get; set; }
        public string UTM5 { get; set; }
        public string Index { get; set; }


    }
}
