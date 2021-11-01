using CsvFramework;
using CsvHelper;
using Jawabsale_Generator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabsale_Generator
{
    public class Jawabsale
    {
        public static List<SubscriptionRevenue> allSubs { get; set; }

        public static List<OperatorPayout> allPayouts { get; set; }

        public static List<Currency> currencyList { get; set; }

        public static ILookup<string, Currency> lookupCurrency { get; set; }

        public static ILookup<string, OperatorPayout> lookupPayouts { get; set; }



        public static async Task RevenuesLast()
        {
            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Jawabsale\Base\SALE-subs-55419400.csv");

            allSubs = await ParseSubscriptions(AllSubsCSV);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");

            allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");

            currencyList = await ParseCurrency(AllCurrencyCSV);

            var revenue = allSubs.Distinct();

            //Unıque List
            var uniques = UniqueItems(revenue);

            foreach (var item in revenue)
            {
                if (item.created_date == item.tpay_activated_date)
                {
                    item.Parked = false;
                }
                else
                {
                    item.Parked = true;
                }
            }

            //Unıque List
            uniques = UniqueItems(revenue);

            foreach (var item in revenue)
            {
                if (item.operator_name==null)
                {
                    item.operator_name = "NoOperator";
                }
            }

            revenue = revenue.GroupBy(x => new
            {
                x.created_date,
                x.payment_gateway,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                payment_gateway = x.Key.payment_gateway,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                //user_id = x.GroupBy(c => c.user_id).Where(grp => grp.Count() > 1).Select(grp => grp.Key).Sum(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            //Unıque List
            uniques = UniqueItems(revenue);

            lookupCurrency = currencyList.ToLookup(p => p.currency_code);
            lookupPayouts = allPayouts.ToLookup(p => p.operator_name);

            int e = 0;
            int p = 0;
            foreach (var sub in revenue)
            {

                var currency = lookupCurrency[sub.currency_code].FirstOrDefault();

                var payout = lookupPayouts[sub.operator_name].FirstOrDefault();

                if (currency != null)
                {
                    try
                    {
                        sub.usd_amount = sub.currency_amount / (decimal)currency.quotes;
                        p++;
                    }
                    catch (Exception)
                    {
                        e++;
                        if (e >= allSubs.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }



                if (payout != null)
                {
                    sub.net_usd_amount = sub.usd_amount * decimal.Parse(payout.payout_percentage.Replace("%", "")) / 100;
                }

                uniques = UniqueItems(revenue);


                sub.UTM5 = TruncateLongString(sub.utm_source_at_subscription, 0, 5);

            }

            await CsvWriter(revenue, "revenues_last");

            revenue = revenue.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.operator_name,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();

            List<RevenuesLast> revenuesLast = new List<RevenuesLast>();

            int k = 0;
            foreach (var item in revenue)
            {
                RevenuesLast rev = new RevenuesLast();
                rev.index = k;
                k++;
                rev.created_date = item.created_date;
                rev.country_code = item.country_code;
                rev.operator_name = item.operator_name;
                rev.usd_amount = item.usd_amount;
                rev.net_usd_amount = item.net_usd_amount;
                revenuesLast.Add(rev);
            }

            //Send Google Sheets to revenuesLast

            await CsvWriter(revenuesLast, "RevenuesLast");


        }

        public static List<string> UniqueItems(IEnumerable<SubscriptionRevenue> list)
        {
            List<string> uniques = new List<string>();

            foreach (var item in list)
            {
                if (!uniques.Contains(item.payment_gateway)) uniques.Add(item.payment_gateway);
            }
            return uniques;
        }




        public static async Task<List<SubscriptionRevenue>> ParseSubscriptions(string[] subscriptions)
        {

            CsvFactory.Register<SubscriptionRevenue>(builder =>
            {
                builder.Add(a => a.Id).Type(typeof(object)).ColumnName("Id");
                builder.Add(a => a.user_id).Type(typeof(int)).ColumnName("user_id");
                builder.Add(a => a.subscription_id).Type(typeof(int)).ColumnName("subscription_id");
                builder.Add(a => a.next_subscription_id).Type(typeof(int)).ColumnName("next_subscription_id");
                builder.Add(a => a.created_date).Type(typeof(string)).ColumnName("created_date");
                builder.Add(a => a.created_time).Type(typeof(string)).ColumnName("created_time");
                builder.Add(a => a.is_first_sub).Type(typeof(int)).ColumnName("is_first_sub");
                builder.Add(a => a.is_last_sub).Type(typeof(int)).ColumnName("is_last_sub");
                builder.Add(a => a.utm_source_at_subscription).Type(typeof(string)).ColumnName("utm_source_at_subscription");
                builder.Add(a => a.utm_medium_at_subscription).Type(typeof(string)).ColumnName("utm_medium_at_subscription");
                builder.Add(a => a.expired_time).Type(typeof(string)).ColumnName("expired_time");
                builder.Add(a => a.period_type).Type(typeof(string)).ColumnName("period_type");
                builder.Add(a => a.site_lang).Type(typeof(string)).ColumnName("site_lang");
                builder.Add(a => a.is_demo).Type(typeof(int)).ColumnName("is_demo");
                builder.Add(a => a.country_code).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.currency_code).Type(typeof(string)).ColumnName("currency_code");
                builder.Add(a => a.ei_language_code).Type(typeof(string)).ColumnName("ei_language_code");
                builder.Add(a => a.ei_question_id).Type(typeof(int)).ColumnName("ei_question_id");
                builder.Add(a => a.ei_step3_view_id).Type(typeof(int)).ColumnName("ei_step3_view_id");
                builder.Add(a => a.fully_paid).Type(typeof(int)).ColumnName("fully_paid");
                builder.Add(a => a.currency_amount).Type(typeof(decimal)).ColumnName("currency_amount");
                builder.Add(a => a.status).Type(typeof(string)).ColumnName("status");
                builder.Add(a => a.charging_status).Type(typeof(string)).ColumnName("charging_status");
                builder.Add(a => a.payment_gateway).Type(typeof(string)).ColumnName("payment_gateway");
                builder.Add(a => a.unsubscribed_by).Type(typeof(string)).ColumnName("unsubscribed_by");
                builder.Add(a => a.first_subscription_subject_id).Type(typeof(int)).ColumnName("first_subscription_subject_id");
                builder.Add(a => a.utm_source_first).Type(typeof(string)).ColumnName("utm_source_first");
                builder.Add(a => a.utm_source_last).Type(typeof(string)).ColumnName("utm_source_last");
                builder.Add(a => a.utm_medium_first).Type(typeof(string)).ColumnName("utm_medium_first");
                builder.Add(a => a.utm_medium_last).Type(typeof(string)).ColumnName("utm_medium_last");
                builder.Add(a => a.phone_confirmed_by_tpay_enreachment).Type(typeof(int)).ColumnName("phone_confirmed_by_tpay_enreachment");
                builder.Add(a => a.tpay_activated_date).Type(typeof(string)).ColumnName("tpay_activated_date");
                builder.Add(a => a.tpay_activated_time).Type(typeof(string)).ColumnName("tpay_activated_time");
                builder.Add(a => a.operator_name).Type(typeof(string)).ColumnName("operator_name");
                builder.Add(a => a.subscription_log_type).Type(typeof(string)).ColumnName("subscription_log_type");
                builder.Add(a => a.tpay_operator_detail_id).Type(typeof(int)).ColumnName("tpay_operator_detail_id");


            }, true, ',', subscriptions);

            List<SubscriptionRevenue> subs = await CsvFactory.Parse(new SubscriptionRevenueParser());

            return subs;

        }

        private static async Task<List<OperatorPayout>> ParseOperatorPayouts(string[] payouts)
        {

            CsvFactory.Register<OperatorPayout>(builder =>
            {

                builder.Add(a => a.operator_name).Type(typeof(string)).ColumnName("operator_name");
                builder.Add(a => a.payout_percentage).Type(typeof(string)).ColumnName("payout_percentage");

            }, true, ',', payouts);

            List<OperatorPayout> pays = await CsvFactory.Parse<OperatorPayout>();

            return pays;

        }

        private static async Task<List<Currency>> ParseCurrency(string[] currency)
        {

            CsvFactory.Register<Currency>(builder =>
            {

                builder.Add(a => a.index).Type(typeof(string)).ColumnName("index");
                builder.Add(a => a.quotes).Type(typeof(double)).ColumnName("quotes");
                builder.Add(a => a.currency_code).Type(typeof(string)).ColumnName("currency_code");


            }, true, ',', currency);

            List<Currency> currencies = await CsvFactory.Parse<Currency>();

            return currencies;

        }

        public static string TruncateLongString(string str, int minLenght, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return str.Substring(minLenght, Math.Min(str.Length, maxLength));
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
            var exportPath = Path.Combine(@"C:\temp\Jawabsale\OutData\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
            }
        }
    }
}
