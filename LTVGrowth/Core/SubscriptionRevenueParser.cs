using CsvFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTVGrowth.Core
{
    public class SubscriptionRevenueParser : IPaseringAsync<SubscriptionRevenue>
    {

        public SubscriptionRevenueParser()
        {

        }



        public async Task<ParserResult> ParsingAsync(SubscriptionRevenue item)
        {

            ParserResult result = new ParserResult();

            if (item.payment_gateway != "tpay" && item.is_demo != 0)
            //if (item.payment_gateway != "tpay" || item.is_demo != 0 || item.created_date >= "2017-01-01")
            {
                result.SkipThis = true;
                return result;
            }


            result.SkipThis = false;
            return result;


        }

    }

}
