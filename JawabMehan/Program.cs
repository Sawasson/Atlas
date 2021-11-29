using System;
using System.Threading.Tasks; 

namespace JawabMehan
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var list = await JawabMehan.RawRevenuesLastDaily();
            //await JawabMehan.RawRevenuesLastMonthly(list);
            var list2 = await JawabMehan.New_LTV_SAMEMONTH();
            await JawabMehan.RawFinalReportMonthly(list2);
            await JawabMehan.RawDailyCost();
            //await JawabMehan.RawMonthlyClicks();

        }
    }
}
