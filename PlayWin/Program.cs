using MongoDB;
using System;
using System.Threading.Tasks;

namespace PlayWin
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var list1 = await PlayWin.RawRevenuesLastMonthly();

            MongoHelper.DropTable("PlayWin_RawRevenuesLastMonthly");

            await MongoHelper.AddMany(list1, "PlayWin_RawRevenuesLastMonthly");


            var tuple1 = await PlayWin.New_LTV_SAMEMONTH();

            MongoHelper.DropTable("PlayWin_NewLTVSamemonth");

            await MongoHelper.AddMany(tuple1.Item1, "PlayWin_NewLTVSamemonth");



            var list2 = await PlayWin.RawFinalReportMonthly(tuple1);

            MongoHelper.DropTable("PlayWin_RawFinalReportMonthly");

            await MongoHelper.AddMany(list2, "PlayWin_RawFinalReportMonthly");


            var tuple2 = await PlayWin.RawDailyCost();

            MongoHelper.DropTable("PlayWin_RawDailyCost");

            await MongoHelper.AddMany(tuple2.Item1, "PlayWin_RawDailyCost");



            var list3 = await PlayWin.RawMonthlyClicks(tuple2.Item2);

            MongoHelper.DropTable("PlayWin_RawMonthlyClicks");

            await MongoHelper.AddMany(list3, "PlayWin_RawMonthlyClicks");
        }
    }
}
