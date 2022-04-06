using MongoDB;
using System.Threading.Tasks;

namespace JawabTawzeef
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var list1 = await JawabTawzeef.RawRevenuesLastDaily();

            MongoHelper.DropTable("JawabTawzeef_RawRevenuesLastDaily");

            await MongoHelper.AddMany(list1.Item2, "JawabTawzeef_RawRevenuesLastDaily");


            var list2 = await JawabTawzeef.RawRevenuesLastMonthly(list1.Item1);

            MongoHelper.DropTable("JawabTawzeef_RawRevenuesLastMonthly");

            await MongoHelper.AddMany(list2, "JawabTawzeef_RawRevenuesLastMonthly");


            var tuple1 = await JawabTawzeef.New_LTV_SAMEMONTH();

            MongoHelper.DropTable("JawabTawzeef_NewLTVSamemonth");

            await MongoHelper.AddMany(tuple1.Item1, "JawabTawzeef_NewLTVSamemonth");



            var list3 = await JawabTawzeef.RawFinalReportMonthly(tuple1);

            MongoHelper.DropTable("JawabTawzeef_RawFinalReportMonthly");

            await MongoHelper.AddMany(list3, "JawabTawzeef_RawFinalReportMonthly");


            var tuple2 = await JawabTawzeef.RawDailyCost();

            MongoHelper.DropTable("JawabTawzeef_RawDailyCost");

            await MongoHelper.AddMany(tuple2.Item1, "JawabTawzeef_RawDailyCost");



            var list4 = await JawabTawzeef.RawMonthlyClicks(tuple2.Item2);

            MongoHelper.DropTable("JawabTawzeef_RawMonthlyClicks");

            await MongoHelper.AddMany(list4, "JawabTawzeef_RawMonthlyClicks");























            //var list = await JawabTawzeef.RawRevenuesLastDaily();
            //await JawabTawzeef.RawRevenuesLastMonthly(list);
            //var list2 = await JawabTawzeef.New_LTV_SAMEMONTH();
            //await JawabTawzeef.RawFinalReportMonthly(list2);
            //var costList = await JawabTawzeef.RawDailyCost();
            //await JawabTawzeef.RawMonthlyClicks(costList.Item2);

        }
    }
}
