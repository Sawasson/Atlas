using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public class Currency
    {
        [Key]
        public ObjectId Id { get; set; }
        public string index { get; set; }
        public double quotes { get; set; }
        public string currency_code { get; set; }

    }
}
