using Atlas;
using CsvFramework;
using GoogleSheetsHelper;
using Jawabkom_Generator3.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Jawabkom_Generator3
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var currencyList = await Jawabkom.RawCurrency();

            MongoHelper.DropTable("RawCurrency");

            await MongoHelper.AddMany(currencyList, "RawCurrency");



            var RawRevenuesLastMonthlyList = await Jawabkom.RawRevenuesLastMonthly();

            MongoHelper.DropTable("Jawabkom_RawRevenuesLastMonthly");

            await MongoHelper.AddMany(RawRevenuesLastMonthlyList, "Jawabkom_RawRevenuesLastMonthly");



            var lists = await Jawabkom.NewLTVSameMonth();

            var RawFinalReportMonthlyList = await Jawabkom.RawFinalReportMonthly(lists);

            MongoHelper.DropTable("Jawabkom_RawFinalReportMonthly");

            await MongoHelper.AddMany(RawFinalReportMonthlyList, "Jawabkom_RawFinalReportMonthly");



            await Jawabkom.LTVModels(lists);




            var DailyCostList = await Jawabkom.DailyCost();

            MongoHelper.DropTable("Jawabkom_RawDailyCost");

            await MongoHelper.AddMany(DailyCostList, "Jawabkom_RawDailyCost");




            var RawMonthlyClicksList = await Jawabkom.RawMonthlyClicks();

            MongoHelper.DropTable("Jawabkom_RawMonthlyClicks");

            await MongoHelper.AddMany(RawMonthlyClicksList, "Jawabkom_RawMonthlyClicks");






            //////GET GOOGLE SHEETS DATA
            //var sheet = GoogleSheetsKeys.Hawiyyah_NewLTVSAMEMONTH();
            //var gsh = new GoogleSheetsHelper.GoogleSheetsHelper(sheet.FilePath, sheet.Key);
            //var gsp = new GoogleSheetParameters() { RangeColumnStart = 1, RangeRowStart = 1, RangeColumnEnd = 20, RangeRowEnd = 30000, FirstRowIsHeaders = true, SheetName = sheet.SheetTitle };
            //var rowValues = gsh.GetDataFromSheet(gsp);
            //ExcelFactory.ExportExcelTable(rowValues, sheet.SheetTitle);



            //var currencyList = await Jawabkom.RawCurrency();
            //await Jawabkom.RawRevenuesLastMonthly();
            //var lists = await Jawabkom.NewLTVSameMonth();
            //await Jawabkom.RawFinalReportMonthly(lists);
            //await Jawabkom.LTVModels(lists);

            //await Jawabkom.DailyCost();

            //await Jawabkom.RawMonthlyClicks();


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
