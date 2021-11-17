using CsvFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialPriceLTV.Core
{
    public class SubscriptionRevenueParser : IPaseringAsync<SubscriptionRevenue>
    {

        public SubscriptionRevenueParser()
        {

        }



        public async Task<ParserResult> ParsingAsync(SubscriptionRevenue item)
        {

            ParserResult result = new ParserResult();

            if (item.is_demo != 0)
            {
                result.SkipThis = true;
                return result;
            }


            result.SkipThis = false;
            return result;


        }

    }

}
