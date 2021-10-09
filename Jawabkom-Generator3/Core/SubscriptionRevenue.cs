﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
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

        public string created_date { get; set; }

        public string tpay_activated_date { get; set; }
        public string Language { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }

        public string Project { get; set; }

        public string Category { get; set; }
        public string Country { get; set; }

        public bool IsMatched { get; set; }

        public string utm_medium_at_activation { get; set; }
    }
}