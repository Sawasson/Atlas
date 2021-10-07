using Atlas;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Jawabkom_Generator3
{
    class Program
    {
        static void Main(string[] args)
        {
            int dataRange = 1;
            int oldestRange = 50;
            int last30 = 30;
            string[] adnetworksUtm5 = { "Kimia", "Mobip", "IMOCa", "Mobid" };
            string[] googleUtm5 = { "GoogleAds-JAWAB", "GoogleDisplay", "GoogleAds-JAWABENGLISH" };
            string[] parseDates = { "created_date", "tpay_activated_date"};

            string filePath = "C:/Users/savas/Downloads/revenues_last.xls";
            var lastId = CsvReader(filePath).LastOrDefault().Result.Line;

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
    }
}
