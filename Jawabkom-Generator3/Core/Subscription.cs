using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public class Subscription
    {

        public int unique_id { get; set; }
        public object Id { get; set; }

        public string utm_source_at_activation { get; set; }

        public string utm_source_at_subscription { get; set; }
        public int is_demo { get; set; }

        public int is_first_sub { get; set; }


        public string first_subscription_subject_id { get; set; }

        public string created_date { get; set; }

        public string tpay_activated_date { get; set; }
        public string Language { get; set; }

        public string domain_pattern { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }

        public string Project { get; set; }

        public string Category { get; set; }
        public string Country { get; set; }

        public bool IsMatched { get; set; }

        public string utm_medium_at_activation { get; set; }

        //public bool IsPaid { get; set; }

        public bool IsTrial { get; set; }
    }
}
