using HawiyyahGenerator.Core;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HawiyyahGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tuple1 = await Hawiyyah.RevenuesLast();

            MongoHelper.DropTable("Hawiyyah_RevenuesLastALL");

            await MongoHelper.AddMany(tuple1.Item1, "Hawiyyah_RevenuesLastALL");

            MongoHelper.DropTable("Hawiyyah_RevenuesLastHawiyyah");

            await MongoHelper.AddMany(tuple1.Item2, "Hawiyyah_RevenuesLastHawiyyah");

            MongoHelper.DropTable("Hawiyyah_RevenuesLastPeopleReveal");

            await MongoHelper.AddMany(tuple1.Item3, "Hawiyyah_RevenuesLastPeopleReveal");


            var tuple2 = await Hawiyyah.NewLTVSameMonth();

            MongoHelper.DropTable("Hawiyyah_NewLTVSamemonth");

            await MongoHelper.AddMany(tuple2.Item1, "Hawiyyah_NewLTVSamemonth");


            var tuple3 = await Hawiyyah.FirstSubReport(tuple2);

            MongoHelper.DropTable("Hawiyyah_FirstSubReportALL");

            await MongoHelper.AddMany(tuple3.Item1, "Hawiyyah_FirstSubReportALL");

            MongoHelper.DropTable("Hawiyyah_FirstSubReportHawiyyah");

            await MongoHelper.AddMany(tuple3.Item2, "Hawiyyah_FirstSubReportHawiyyah");

            MongoHelper.DropTable("Hawiyyah_FirstSubReportPeopleReveal");

            await MongoHelper.AddMany(tuple3.Item3, "Hawiyyah_FirstSubReportPeopleReveal");



            //await Hawiyyah.LTVModels(lists2);

















            await Hawiyyah.RevenuesLast();
            var lists = await Hawiyyah.NewLTVSameMonth();
            var lists2 = await Hawiyyah.FirstSubReport(lists);
            //await Hawiyyah.LTVModels(lists2);
        }
    }
}
