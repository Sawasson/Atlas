using CsvFramework;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jawabsale_Generator.Core
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
