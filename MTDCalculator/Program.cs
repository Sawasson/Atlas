using CsvFramework;
using CsvHelper;
using MTDCalculator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MTDCalculator
{
    class Program
    {
        static async Task Main(string[] args)
        {

            await RawMTD();
            await RawRevenuesMTD();




        }

        public static async Task RawMTD()
        {

            var AllrevLastCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Revenues_Last.xls");
            var allRevLast = await ParseRevenuesLast(AllrevLastCSV);

            var revenue = allRevLast;

            DateTime Date2017 = new DateTime(2017, 1, 1);

            var revenueMtdApps = revenue.Where(x => x.created_date >= Date2017).ToList();
            revenueMtdApps = revenueMtdApps.Where(x => x.Category == "Apps").ToList();
            revenueMtdApps = revenueMtdApps.Where(x => x.operator_name != "EG-Etisalat").ToList();

            var revenueMtdApps_ = revenueMtdApps.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.operator_name,
                x.Category,
            }).Select(x => new MTDAppsCalculation
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                Category = x.Key.Category,
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();

            await CsvWriter(revenueMtdApps_, "RawMTD");


        }

        public static async Task RawRevenuesMTD()
        {

            var AllrevLastCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Revenues_Last.xls");
            var allRevLast = await ParseRevenuesLast(AllrevLastCSV);

            var revenue = allRevLast;

            DateTime Date2017 = new DateTime(2017, 1, 1);

            var revenueMtd = revenue.Where(x => x.created_date >= Date2017).ToList();
            revenueMtd = revenueMtd.Where(x => x.Category != "Apps").ToList();

            var revenueMtd_ = revenueMtd.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.operator_name,
                x.Category,
            }).Select(x => new MTDAppsCalculation
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                Category = x.Key.Category,
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();

            await CsvWriter(revenueMtd_, "Raw_Revenues_MTD");


        }



        public static async Task CsvWriter<T>(List<T> list, string fileName)
        {
            //string path = "C:/Users/savas/Desktop/list.csv";

            //using (var writer = new StreamWriter(path))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(list);
            //}
            string filePath = fileName + ".csv";
            var exportPath = Path.Combine(@"C:\temp\MTDCalculator\OutData\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
            }
        }


        public static async Task<List<RevenuesLast>> ParseRevenuesLast(string[] revenuesLast)
        {

            CsvFactory.Register<RevenuesLast>(builder =>
            {
                //builder.Add(a => a.Id).Type(typeof(object)).ColumnName("Id");
                builder.Add(a => a.user_id).Type(typeof(int)).ColumnName("user_id");
                //builder.Add(a => a.subscription_id).Type(typeof(int)).ColumnName("subscription_id");
                //builder.Add(a => a.next_subscription_id).Type(typeof(int)).ColumnName("next_subscription_id");
                builder.Add(a => a.created_date).Type(typeof(DateTime)).ColumnName("created_date");
                //builder.Add(a => a.created_time).Type(typeof(string)).ColumnName("created_time");
                //builder.Add(a => a.is_first_sub).Type(typeof(int)).ColumnName("is_first_sub");
                //builder.Add(a => a.is_last_sub).Type(typeof(int)).ColumnName("is_last_sub");
                builder.Add(a => a.utm_source_at_subscription).Type(typeof(string)).ColumnName("utm_source_at_subscription");
                //builder.Add(a => a.utm_medium_at_subscription).Type(typeof(string)).ColumnName("utm_medium_at_subscription");
                //builder.Add(a => a.expired_time).Type(typeof(string)).ColumnName("expired_time");
                //builder.Add(a => a.period_type).Type(typeof(string)).ColumnName("period_type");
                //builder.Add(a => a.site_lang).Type(typeof(string)).ColumnName("site_lang");
                //builder.Add(a => a.is_demo).Type(typeof(int)).ColumnName("is_demo");
                builder.Add(a => a.country_code).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.currency_code).Type(typeof(string)).ColumnName("currency_code");
                //builder.Add(a => a.ei_language_code).Type(typeof(string)).ColumnName("ei_language_code");
                //builder.Add(a => a.ei_question_id).Type(typeof(int)).ColumnName("ei_question_id");
                //builder.Add(a => a.ei_step3_view_id).Type(typeof(int)).ColumnName("ei_step3_view_id");
                //builder.Add(a => a.fully_paid).Type(typeof(int)).ColumnName("fully_paid");
                builder.Add(a => a.currency_amount).Type(typeof(decimal)).ColumnName("currency_amount");
                //builder.Add(a => a.status).Type(typeof(string)).ColumnName("status");
                //builder.Add(a => a.charging_status).Type(typeof(string)).ColumnName("charging_status");
                builder.Add(a => a.payment_gateway).Type(typeof(string)).ColumnName("payment_gateway");
                //builder.Add(a => a.unsubscribed_by).Type(typeof(string)).ColumnName("unsubscribed_by");
                //builder.Add(a => a.first_subscription_subject_id).Type(typeof(int)).ColumnName("first_subscription_subject_id");
                //builder.Add(a => a.utm_source_first).Type(typeof(string)).ColumnName("utm_source_first");
                //builder.Add(a => a.domain_pattern).Type(typeof(string)).ColumnName("domain_pattern");
                //builder.Add(a => a.utm_source_last).Type(typeof(string)).ColumnName("utm_source_last");
                //builder.Add(a => a.utm_medium_first).Type(typeof(string)).ColumnName("utm_medium_first");
                //builder.Add(a => a.utm_medium_last).Type(typeof(string)).ColumnName("utm_medium_last");
                //builder.Add(a => a.phone_confirmed_by_tpay_enreachment).Type(typeof(int)).ColumnName("phone_confirmed_by_tpay_enreachment");
                //builder.Add(a => a.tpay_activated_date).Type(typeof(string)).ColumnName("tpay_activated_date");
                //builder.Add(a => a.tpay_activated_time).Type(typeof(string)).ColumnName("tpay_activated_time");
                builder.Add(a => a.operator_name).Type(typeof(string)).ColumnName("operator_name");
                //builder.Add(a => a.subscription_log_type).Type(typeof(string)).ColumnName("subscription_log_type");
                //builder.Add(a => a.tpay_operator_detail_id).Type(typeof(int)).ColumnName("tpay_operator_detail_id");
                builder.Add(a => a.Parked).Type(typeof(bool)).ColumnName("Parked");
                builder.Add(a => a.index).Type(typeof(string)).ColumnName("index");
                builder.Add(a => a.Category).Type(typeof(string)).ColumnName("Category");
                builder.Add(a => a.quotes).Type(typeof(double)).ColumnName("quotes");
                builder.Add(a => a.payout_percentage).Type(typeof(double)).ColumnName("payout_percentage");
                builder.Add(a => a.usd_amount).Type(typeof(decimal)).ColumnName("usd_amount");
                builder.Add(a => a.net_usd_amount).Type(typeof(decimal)).ColumnName("net_usd_amount");



            }, true, ',', revenuesLast);

            List<RevenuesLast> revLast = await CsvFactory.Parse<RevenuesLast>();

            return revLast;

        }

    }
}
