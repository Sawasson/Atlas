using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JawabTawzeef
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var list = await JawabTawzeef.RawRevenuesLastDaily();
            //await JawabTawzeef.RawRevenuesLastMonthly(list);
            var list2 = await JawabTawzeef.New_LTV_SAMEMONTH();
            await JawabTawzeef.RawFinalReportMonthly(list2);
            await JawabTawzeef.RawDailyCost();

            await JawabTawzeef.RawMonthlyClicks();

        }
    }
}
