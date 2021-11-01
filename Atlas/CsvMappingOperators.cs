using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;

namespace Atlas
{
    public class CsvMappingOperators : CsvMapping<Operator>
    {
        public CsvMappingOperators()
    : base()
        {
            MapProperty(0, x => x.id);
            MapProperty(1, x => x.user_id);
            MapProperty(2, x => x.created_date);
            MapProperty(3, x => x.period_type);
            MapProperty(4, x => x.country_code);
            MapProperty(5, x => x.operator_name);
            MapProperty(6, x => x.tpay_activated_date);
            MapProperty(7, x => x.tpay_phone);
            MapProperty(8, x => x.Parked_Days);
            MapProperty(9, x => x.Parked);


        }
    }
}
