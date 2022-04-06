using System.Threading.Tasks;
using MongoDB;

namespace JawabMehan
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var list1 = await JawabMehan.RawRevenuesLastDaily();

            MongoHelper.DropTable("JawabMehan_RawRevenuesLastDaily");

            await MongoHelper.AddMany(list1.Item2, "JawabMehan_RawRevenuesLastDaily");


            var list2 = await JawabMehan.RawRevenuesLastMonthly(list1.Item1);

            MongoHelper.DropTable("JawabMehan_RawRevenuesLastMonthly");

            await MongoHelper.AddMany(list2, "JawabMehan_RawRevenuesLastMonthly");


            var tuple1 = await JawabMehan.New_LTV_SAMEMONTH();

            MongoHelper.DropTable("JawabMehan_NewLTVSamemonth");

            await MongoHelper.AddMany(tuple1.Item1, "JawabMehan_NewLTVSamemonth");



            var list3 = await JawabMehan.RawFinalReportMonthly(tuple1);

            MongoHelper.DropTable("JawabMehan_RawFinalReportMonthly");

            await MongoHelper.AddMany(list3, "JawabMehan_RawFinalReportMonthly");


            var tuple2 = await JawabMehan.RawDailyCost();

            MongoHelper.DropTable("JawabMehan_RawDailyCost");

            await MongoHelper.AddMany(tuple2.Item1, "JawabMehan_RawDailyCost");



            var list4 = await JawabMehan.RawMonthlyClicks(tuple2.Item2);

            MongoHelper.DropTable("JawabMehan_RawMonthlyClicks");

            await MongoHelper.AddMany(list4, "JawabMehan_RawMonthlyClicks");






            //var list = await JawabMehan.RawRevenuesLastDaily();
            ////await JawabMehan.RawRevenuesLastMonthly(list);
            //var list2 = await JawabMehan.New_LTV_SAMEMONTH();
            //await JawabMehan.RawFinalReportMonthly(list2);
            //await JawabMehan.RawDailyCost();
            ////await JawabMehan.RawMonthlyClicks();

        }
    }
}
