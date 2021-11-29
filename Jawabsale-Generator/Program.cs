using MongoDB;
using System;
using System.Threading.Tasks;

namespace Jawabsale_Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var list1 = await Jawabsale.RevenuesLast();

            MongoHelper.DropTable("Jawabsale_RevenuesLast");

            await MongoHelper.AddMany(list1, "Jawabsale_RevenuesLast");


            var tuple1 = await Jawabsale.NewLTVSameMonth();

            MongoHelper.DropTable("Jawabsale_NewLTVSameMonth");

            await MongoHelper.AddMany(tuple1.Item1, "Jawabsale_NewLTVSameMonth");


            var lists2 = await Jawabsale.FirstSubReport(tuple1);

            MongoHelper.DropTable("Jawabsale_FirstSubReport");

            await MongoHelper.AddMany(lists2.Item2, "Jawabsale_FirstSubReport");


            var list3 = await Jawabsale.LTVModels(lists2);

            MongoHelper.DropTable("Jawabsale_LTVModels");

            await MongoHelper.AddMany(list3, "Jawabsale_LTVModels");


        }
    }
}
