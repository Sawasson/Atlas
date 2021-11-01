using CsvHelper;
using CsvHelper.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
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
        private readonly static IMongoCollection<Operator> Operators;

        static void Main(string[] args)
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
            var database = dbClient.GetDatabase("JawabDB");
            var collection = database.GetCollection<BsonDocument>("Operators");

            string filePath = "C:/temp/user_operator.csv";
            var operators = CsvReader(filePath);

            if (true)
            {

            }

            //string filePath = "C:/Users/savas/Downloads/revenues_last.xls";
            //var df_revenue = CsvReader(filePath);
            //var list = df_revenue.AsQueryable();

            //var faceReport = list.GroupBy(x => new
            //{
            //    x.Result.Category,
            //}).Select(x => new UsdAmountReport
            //{
            //    Category = x.Key.Category,
            //    TotalUsdAmount = x.Sum(y => y.Result.UsdAmount),
            //    //TotalNetUsdAmount = x.Sum(y => y.Result.NetUsdAmount)
            //}).ToList();
            ////foreach (var item in list)
            ////{
            ////    Console.WriteLine(item.Result.Category);
            ////    Console.WriteLine(item.Result.UsdAmount);


            ////}
            //foreach (var item in faceReport)
            //{
            //    Console.WriteLine(item.Category);
            //    Console.WriteLine(item.TotalUsdAmount);


            //}
            //Console.ReadLine();
            //DateTime Date2017 = new DateTime(2017, 1, 1);

            //var df_revenue_mtd_apps = df_revenue.Where(x => x.Result.CreatedDate >= Date2017).ToList();
            ////df_revenue_mtd_apps = df_revenue.Where(x => x.Result.Category == "Apps").ToList();
            //df_revenue_mtd_apps = df_revenue.Where(x => x.Result.OperatorName == "EG-Etisalat").ToList();


            //var Report = df_revenue_mtd_apps.GroupBy(x => new
            //{
            //    x.Result.CreatedDate,
            //    x.Result.CountryCode,
            //    x.Result.OperatorName,
            //    x.Result.Category
            //}).Select(x => new UsdAmountReport
            //{
            //    CreatedDate = x.Key.CreatedDate,
            //    CountryCode = x.Key.CountryCode,
            //    OperatorName = x.Key.OperatorName,
            //    Category = x.Key.Category,
            //    TotalUsdAmount = x.Sum(y => y.Result.UsdAmount),
            //    TotalNetUsdAmount = x.Sum(y => y.Result.NetUsdAmount)
            //}).ToList();

            //CsvWriter(Report);





        }


        public static List<Operator> CsvReader(string filePath)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
            CsvMappingOperators csvMapper = new CsvMappingOperators();
            CsvParser<Operator> csvParser = new CsvParser<Operator>(csvParserOptions, csvMapper);

            var data = csvParser.ReadFromFile(filePath, Encoding.UTF8).ToList();
            //try
            //{
            //    Assert.IsTrue(data.All(x => x.IsValid));
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            List<Operator> list = new List<Operator>();
            foreach (var item in data)
            {
                Operator op = new Operator();
                op.country_code = item.Result.country_code;
                op.created_date = item.Result.created_date;
                op.id = item.Result.id;
                op.operator_name = item.Result.operator_name;
                op.Parked = item.Result.Parked;
                op.Parked_Days = item.Result.Parked_Days;
                op.period_type = item.Result.period_type;
                op.tpay_activated_date = item.Result.tpay_activated_date;
                op.tpay_phone = item.Result.tpay_phone;
                op.user_id = item.Result.user_id;
                list.Add(op);
                Operators.InsertOne(op);


            }
            return list;
        }

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
