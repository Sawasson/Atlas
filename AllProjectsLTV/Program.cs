using AllProjectsLTV.Core;
using CsvHelper;
using HawiyyahGenerator;
using Jawabkom_Generator3;
using Jawabsale_Generator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AllProjectsLTV
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await LTVModels();
        }

        public static async Task<List<LTVModels>> LTVModels()
        {
            var listsJawabkom = await Jawabkom.NewLTVSameMonth();
            var jawabkom = await Jawabkom.LTVModels(listsJawabkom);

            var listsHawiyyah = await Hawiyyah.NewLTVSameMonth();
            var listsHawiyyah2 = await Hawiyyah.FirstSubReport(listsHawiyyah);
            var hawiyyah = await Hawiyyah.LTVModels(listsHawiyyah2);

            var listsJawabsale = await Jawabsale.NewLTVSameMonth();
            var listsJawabsale2 = await Jawabsale.FirstSubReport(listsJawabsale);
            var jawabsale = await Jawabsale.LTVModels(listsJawabsale2);


            List<LTVModels> ltvModels = new List<LTVModels>();

            int m = 0;


            foreach (var item in jawabsale)
            {
                LTVModels jwbsle = new LTVModels();
                jwbsle.main_index = m;
                m++;
                jwbsle.index = item.index;
                jwbsle.created_date = item.created_date;
                jwbsle.country_code = item.country_code;
                jwbsle.utm_source_at_subscription = item.utm_source_at_subscription;
                jwbsle.operator_name = item.operator_name;
                //jwbsle.Parked = item.Parked;
                jwbsle.period_type = item.period_type;
                jwbsle.user_id = item.user_id;
                jwbsle.usd_amount = item.usd_amount;
                jwbsle.net_usd_amount = item.net_usd_amount;
                jwbsle.Category = item.Category;
                jwbsle.model = item.model;
                jwbsle.project = "Jawabsale";
                ltvModels.Add(jwbsle);

            }


            foreach (var item in jawabkom)
            {
                LTVModels jwbkom = new LTVModels();
                jwbkom.main_index = m;
                m++;
                jwbkom.index = item.index;
                jwbkom.created_date = item.created_date;
                jwbkom.country_code = item.country_code;
                jwbkom.utm_source_at_subscription = item.utm_source_at_subscription;
                jwbkom.operator_name = item.operator_name;
                jwbkom.Parked = ParkedGenerator(item.Parked);
                jwbkom.period_type = item.period_type;
                jwbkom.user_id = item.user_id;
                jwbkom.usd_amount = item.usd_amount;
                jwbkom.net_usd_amount = item.net_usd_amount;
                jwbkom.Category = item.Category;
                jwbkom.model = item.model;
                jwbkom.project = "Jawabkom";
                ltvModels.Add(jwbkom);

            }

            foreach (var item in hawiyyah)
            {
                LTVModels hwyyah = new LTVModels();
                hwyyah.main_index = m;
                m++;
                hwyyah.index = item.index;
                hwyyah.created_date = item.created_date;
                hwyyah.country_code = item.country_code;
                hwyyah.utm_source_at_subscription = item.utm_source_at_subscription;
                hwyyah.operator_name = item.operator_name;
                hwyyah.Parked = ParkedGenerator(item.Parked);
                hwyyah.period_type = item.period_type;
                hwyyah.user_id = item.user_id;
                hwyyah.usd_amount = item.usd_amount;
                hwyyah.net_usd_amount = item.net_usd_amount;
                hwyyah.Category = item.Category;
                hwyyah.model = item.model;
                hwyyah.project = "Hawiyyah";
                ltvModels.Add(hwyyah);

            }



            return ltvModels;

            await CsvWriter(ltvModels, "AllLTVModels");

        }

        public static string ParkedGenerator(bool parked)
        {
            if (parked==true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public static async Task CsvWriter<T>(IEnumerable<T> list, string fileName)
        {
            //string path = "C:/Users/savas/Desktop/list.csv";

            //using (var writer = new StreamWriter(path))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(list);
            //}
            string filePath = fileName + ".csv";
            var exportPath = Path.Combine(@"C:\temp\AllLTVModels\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
            }
        }




    }
}
