﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTVGrowth.Core
{
    public class UserSpending
    {
        public int user_id { get; set; }
        public decimal currency_amount { get; set; }
        public decimal total_spending { get; set; }

    }
}
