using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWin.Core
{
    public class RawMonthlyClicks
    {
        [Key]
        public ObjectId Id { get; set; }
        public string created_date { get; set; }
        public string country_code { get; set; }
        public int Clicks { get; set; }
        public int GDNClicks { get; set; }
        public int Total_Clicks { get; set; }

    }
}
