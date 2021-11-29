﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JawabTawzeef.Core
{
    public class RawDailyCost
    {
        [Key]
        public ObjectId Id { get; set; }
        public string created_date { get; set; }
        public string country_code { get; set; }
        public decimal SearchCost { get; set; }
        public decimal GDNCost { get; set; }
        public decimal TotalCost { get; set; }


    }
}
