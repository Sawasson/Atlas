using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace Atlas
{
    public class CsvMapping : CsvMapping<Data>
    {
        public CsvMapping()
            : base()
        {
            MapProperty(0, x => x.Line);
            MapProperty(1, x => x.CreatedDate);
            MapProperty(2, x => x.CountryCode);
            MapProperty(3, x => x.UtmSourceAtSubscription);
            MapProperty(4, x => x.CurrencyCode);
            MapProperty(5, x => x.OperatorName);
            MapProperty(6, x => x.Parked);
            MapProperty(7, x => x.PeriodType);
            MapProperty(8, x => x.UserID);
            MapProperty(9, x => x.CurrencyAmount);
            MapProperty(10, x => x.Index);
            MapProperty(11, x => x.Quotes);
            MapProperty(12, x => x.PayoutPercentage);
            MapProperty(13, x => x.UsdAmount);
            MapProperty(14, x => x.NetUsdAmount);
            MapProperty(15, x => x.UTM5);
            MapProperty(16, x => x.Category);
            MapProperty(17, x => x.UTM7);
            MapProperty(18, x => x.Google);
            MapProperty(19, x => x.Apps);
            MapProperty(20, x => x.Apps2);




        }
    }
}
