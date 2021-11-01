using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public class Currency
    {
        public string index { get; set; }
        public double quotes { get; set; }
        public string currency_code { get; set; }

    }
}
