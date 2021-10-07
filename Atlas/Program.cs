using CsvHelper;
using CsvHelper.Configuration;
using NUnit.Framework;
using Parquet;
using Parquet.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Atlas
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:/Users/savas/Downloads/revenues_last.xls";
            var df_revenue = CsvReader(filePath);
            var list = df_revenue.AsQueryable();

            list= list.TakeLast(5);
            var faceReport = list.GroupBy(x => new
            {
                x.Result.Category,
            }).Select(x => new UsdAmountReport
            {
                Category = x.Key.Category,
                TotalUsdAmount = x.Sum(y => y.Result.UsdAmount),
                //TotalNetUsdAmount = x.Sum(y => y.Result.NetUsdAmount)
            }).ToList();
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item.Result.Category);
            //    Console.WriteLine(item.Result.UsdAmount);
                

            //}
            foreach (var item in faceReport)
            {
                Console.WriteLine(item.Category);
                Console.WriteLine(item.TotalUsdAmount);


            }
            Console.ReadLine();
            DateTime Date2017 = new DateTime(2017, 1, 1);

            var df_revenue_mtd_apps = df_revenue.Where(x => x.Result.CreatedDate >= Date2017).ToList();
            //df_revenue_mtd_apps = df_revenue.Where(x => x.Result.Category == "Apps").ToList();
            df_revenue_mtd_apps = df_revenue.Where(x => x.Result.OperatorName == "EG-Etisalat").ToList();


            var Report = df_revenue_mtd_apps.GroupBy(x => new
            {
                x.Result.CreatedDate,
                x.Result.CountryCode,
                x.Result.OperatorName,
                x.Result.Category
            }).Select(x => new UsdAmountReport
            {
                CreatedDate = x.Key.CreatedDate,
                CountryCode = x.Key.CountryCode,
                OperatorName = x.Key.OperatorName,
                Category = x.Key.Category,
                TotalUsdAmount = x.Sum(y => y.Result.UsdAmount),
                TotalNetUsdAmount = x.Sum(y => y.Result.NetUsdAmount)
            }).ToList();

            CsvWriter(Report);




        }


        public static List<CsvMappingResult<Data>> CsvReader(string filePath)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            CsvMapping csvMapper = new CsvMapping();
            CsvParser<Data> csvParser = new CsvParser<Data>(csvParserOptions, csvMapper);

            var data = csvParser.ReadFromFile(filePath, Encoding.UTF8).ToList();
            Assert.IsTrue(data.All(x => x.IsValid));
            return data;
        }

        //public static IEnumerable<Data> CsvReader(string filePath)
        //{
        //    IEnumerable<Data> data = null;

        //    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        //    {
        //        PrepareHeaderForMatch = args => args.Header.ToLower(),
        //    };
        //    using (var reader = new StreamReader(filePath))
        //    using (var csv = new CsvReader(reader, config))
        //    {
        //        csv.Context.RegisterClassMap<DataCsvMap>();
        //        var records = csv.GetRecords<Data>();
        //        data = records;
        //    }
        //    return data;

        //}

        public static void CsvWriter(List<UsdAmountReport> list)
        {
            string path = "C:/Users/savas/Desktop/Book.csv";

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }


        }


    }
}
