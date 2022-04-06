using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HawiyyahGenerator.Core
{
    public class FirstSubReport
    {
        [Key]
        public ObjectId Id { get; set; }
        public int index { get; set; }
        public string created_date { get; set; }
        public string country_code { get; set; }
        public string utm_source_at_subscription { get; set; }
        public string operator_name { get; set; }
        public bool Parked { get; set; }
        public string period_type { get; set; }
        public string Category { get; set; }
        public int user_id { get; set; }
        public decimal usd_amount { get; set; }
        public decimal net_usd_amount { get; set; }

        public string domain { get; set; }


    }
}
