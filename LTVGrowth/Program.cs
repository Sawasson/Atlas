using CsvFramework;
using CsvHelper;
using LTVGrowth.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LTVGrowth
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await LTVGrowthRawSameMonth();
        }

        public static async Task LTVGrowthRawSameMonth()
        {
            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\users_subscriptions.csv");
            var dataf = await ParseSubscriptions(AllSubsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");
            var currencyList = await ParseCurrency(AllCurrencyCSV);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");
            List<OperatorPayout> allPayouts = await ParseOperatorPayouts(PayoutsCSV);


            var lookupCurrency = currencyList.ToLookup(p => p.currency_code);

            var lookupPayouts = allPayouts.ToLookup(p => p.operator_name);

            int e = 0;
            int p = 0;
            foreach (var sub in dataf)
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
                        if (e >= dataf.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }

                if (payout != null)
                {
                    sub.net_usd_amount = sub.usd_amount * decimal.Parse(payout.payout_percentage.Replace("%", "")) / 100;
                }
            }

            var userSpendingList = dataf.GroupBy(x => new
            {
                x.user_id,
            }).Select(x => new UserSpending
            {
                user_id = x.Key.user_id,
                total_spending = x.Sum(x => x.usd_amount),
            }).ToList();

            var dfx_first_date = dataf.Where(x => x.is_first_sub == 1).ToList();

            dfx_first_date = dfx_first_date.OrderBy(x => x.created_date).ThenBy(x => x.user_id).ToList();

            dfx_first_date = dfx_first_date.GroupBy(x => new
            {
                x.user_id,
            }).Select(x => new SubscriptionRevenue
            {
                user_id = x.Key.user_id,
                first_sub_date = x.Min(x => x.created_date),
            }).ToList();

            var lookupDfxFirstDate = dfx_first_date.ToLookup(p => p.user_id);


            int i = 0;
            int z = 0;
            foreach (var item in dataf)
            {
                var firstDate = lookupDfxFirstDate[item.user_id].FirstOrDefault();

                if (firstDate != null)
                {
                    try
                    {
                        item.first_sub_date = firstDate.first_sub_date;
                        z++;
                    }
                    catch (Exception)
                    {
                        i++;
                        if (i >= dataf.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }
            }

            int[] DaysPeriod = new int[] { 7, 14, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };

            foreach (var item in dataf)
            {
                
                item.Days7 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(7)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days14 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(14)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days30 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(30)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days60 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(60)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days90 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(90)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days120 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(120)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days150 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(150)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days180 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(180)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days210 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(210)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days240 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(240)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days270 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(270)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days300 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(300)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days330 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(330)).Select(x => x.usd_amount).FirstOrDefault();
                item.Days360 = dataf.Where(x => x.created_date <= item.first_sub_date.AddDays(360)).Select(x => x.usd_amount).FirstOrDefault();

                item.tpay_activated_date = DatetimeMonthlyParse(item.tpay_activated_date);

                item.first_sub_date = DatetimeMonthlyParse(item.first_sub_date);

            }

            /////////////////for same_month//////////////////////////////////////////////
            var orj_dataf = dataf;

            dataf = dataf.Where(x => x.tpay_activated_date == x.first_sub_date).ToList();


            dataf = dataf.GroupBy(x => new
            {
                x.user_id,
                x.first_sub_date,
                x.country_code,
                x.operator_name,
                x.currency_code,
                x.period_type,
                x.first_subscription_subject_id,
            }).Select(x => new SubscriptionRevenue
            {
                user_id = x.Key.user_id,
                first_sub_date = x.Key.first_sub_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                currency_code = x.Key.currency_code,
                period_type = x.Key.period_type,
                first_subscription_subject_id = x.Key.first_subscription_subject_id,
                Days7 = x.Sum(x => x.Days7),
                Days14 = x.Sum(x => x.Days14),
                Days30 = x.Sum(x => x.Days30),
                Days60 = x.Sum(x => x.Days60),
                Days90 = x.Sum(x => x.Days90),
                Days120 = x.Sum(x => x.Days120),
                Days150 = x.Sum(x => x.Days150),
                Days180 = x.Sum(x => x.Days180),
                Days210 = x.Sum(x => x.Days210),
                Days240 = x.Sum(x => x.Days240),
                Days270 = x.Sum(x => x.Days270),
                Days300 = x.Sum(x => x.Days300),
                Days330 = x.Sum(x => x.Days330),
                Days360 = x.Sum(x => x.Days360),
            }).ToList();


            var lookupUserSpendingList = userSpendingList.ToLookup(p => p.user_id);


            foreach (var data in dataf)
            {

                var UserSpending = lookupUserSpendingList[data.user_id].FirstOrDefault();
                data.total_spending = UserSpending.total_spending;
            }

            dataf = dataf.GroupBy(x => new
            {
                x.first_sub_date,
                x.country_code,
                x.operator_name,
                x.currency_code,
                x.period_type,
                x.first_subscription_subject_id,
            }).Select(x => new SubscriptionRevenue
            {
                first_sub_date = x.Key.first_sub_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                currency_code = x.Key.currency_code,
                period_type = x.Key.period_type,
                first_subscription_subject_id = x.Key.first_subscription_subject_id,
                user_id = x.Count(),
                Days7 = x.Sum(x => x.Days7),
                Days14 = x.Sum(x => x.Days14),
                Days30 = x.Sum(x => x.Days30),
                Days60 = x.Sum(x => x.Days60),
                Days90 = x.Sum(x => x.Days90),
                Days120 = x.Sum(x => x.Days120),
                Days150 = x.Sum(x => x.Days150),
                Days180 = x.Sum(x => x.Days180),
                Days210 = x.Sum(x => x.Days210),
                Days240 = x.Sum(x => x.Days240),
                Days270 = x.Sum(x => x.Days270),
                Days300 = x.Sum(x => x.Days300),
                Days330 = x.Sum(x => x.Days330),
                Days360 = x.Sum(x => x.Days360),
                total_spending = x.Sum(x => x.total_spending),
            }).ToList();


            foreach (var item in dataf)
            {

                item.Days7 = item.Days7 / item.user_id;
                item.Days14 = item.Days14 / item.user_id;
                item.Days30 = item.Days30 / item.user_id;
                item.Days60 = item.Days60 / item.user_id;
                item.Days90 = item.Days90 / item.user_id;
                item.Days120 = item.Days120 / item.user_id;
                item.Days150 = item.Days150 / item.user_id;
                item.Days180 = item.Days180 / item.user_id;
                item.Days210 = item.Days210 / item.user_id;
                item.Days240 = item.Days240 / item.user_id;
                item.Days270 = item.Days270 / item.user_id;
                item.Days300 = item.Days300 / item.user_id;
                item.Days330 = item.Days330 / item.user_id;
                item.Days360 = item.Days360 / item.user_id;
                item.spending_360 = item.Days360 * item.user_id;

            }


            await CsvWriter(dataf, "LTV_Growth_Raw_Same_month_sheet");

            var dfx_samemonth = dataf;

            /////////////////for old_model//////////////////////////////////////////////
            dataf = orj_dataf;

            dataf = dataf.Where(x => x.tpay_activated_date == x.first_sub_date).ToList();


            dataf = dataf.GroupBy(x => new
            {
                x.user_id,
                x.first_sub_date,
                x.country_code,
                x.operator_name,
                x.currency_code,
                x.period_type,
            }).Select(x => new SubscriptionRevenue
            {
                user_id = x.Key.user_id,
                first_sub_date = x.Key.first_sub_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                currency_code = x.Key.currency_code,
                period_type = x.Key.period_type,
                Days7 = x.Sum(x => x.Days7),
                Days14 = x.Sum(x => x.Days14),
                Days30 = x.Sum(x => x.Days30),
                Days60 = x.Sum(x => x.Days60),
                Days90 = x.Sum(x => x.Days90),
                Days120 = x.Sum(x => x.Days120),
                Days150 = x.Sum(x => x.Days150),
                Days180 = x.Sum(x => x.Days180),
                Days210 = x.Sum(x => x.Days210),
                Days240 = x.Sum(x => x.Days240),
                Days270 = x.Sum(x => x.Days270),
                Days300 = x.Sum(x => x.Days300),
                Days330 = x.Sum(x => x.Days330),
                Days360 = x.Sum(x => x.Days360),
            }).ToList();


            lookupUserSpendingList = userSpendingList.ToLookup(p => p.user_id);


            foreach (var data in dataf)
            {

                var UserSpending = lookupUserSpendingList[data.user_id].FirstOrDefault();
                data.total_spending = UserSpending.total_spending;
            }

            dataf = dataf.GroupBy(x => new
            {
                x.first_sub_date,
                x.country_code,
                x.operator_name,
                x.currency_code,
                x.period_type,
            }).Select(x => new SubscriptionRevenue
            {
                first_sub_date = x.Key.first_sub_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                currency_code = x.Key.currency_code,
                period_type = x.Key.period_type,
                user_id = x.Count(),
                Days7 = x.Sum(x => x.Days7),
                Days14 = x.Sum(x => x.Days14),
                Days30 = x.Sum(x => x.Days30),
                Days60 = x.Sum(x => x.Days60),
                Days90 = x.Sum(x => x.Days90),
                Days120 = x.Sum(x => x.Days120),
                Days150 = x.Sum(x => x.Days150),
                Days180 = x.Sum(x => x.Days180),
                Days210 = x.Sum(x => x.Days210),
                Days240 = x.Sum(x => x.Days240),
                Days270 = x.Sum(x => x.Days270),
                Days300 = x.Sum(x => x.Days300),
                Days330 = x.Sum(x => x.Days330),
                Days360 = x.Sum(x => x.Days360),
                total_spending = x.Sum(x => x.total_spending),
            }).ToList();


            foreach (var item in dataf)
            {

                item.Days7 = item.Days7 / item.user_id;
                item.Days14 = item.Days14 / item.user_id;
                item.Days30 = item.Days30 / item.user_id;
                item.Days60 = item.Days60 / item.user_id;
                item.Days90 = item.Days90 / item.user_id;
                item.Days120 = item.Days120 / item.user_id;
                item.Days150 = item.Days150 / item.user_id;
                item.Days180 = item.Days180 / item.user_id;
                item.Days210 = item.Days210 / item.user_id;
                item.Days240 = item.Days240 / item.user_id;
                item.Days270 = item.Days270 / item.user_id;
                item.Days300 = item.Days300 / item.user_id;
                item.Days330 = item.Days330 / item.user_id;
                item.Days360 = item.Days360 / item.user_id;
                item.spending_360 = item.Days360 * item.user_id;

            }


            await CsvWriter(dataf, "LTV_Growth_Raw_Sheet");


            var categoryList = await GetCategories();

            var lookupCategories = categoryList.ToLookup(p => p.CategoryID);

            foreach (var item in dfx_samemonth)
            {
                try
                {
                    var category = lookupCategories[Int32.Parse(item.first_subscription_subject_id)].FirstOrDefault();

                    if (category != null)
                    {
                        try
                        {
                            item.CategoryID = category.CategoryID;
                            item.CategoryName = category.CategoryName;
                            z++;
                        }
                        catch (Exception ex)
                        {
                            i++;
                            if (i >= dfx_samemonth.Count * 0.01)
                            {
                                throw ex;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }


            }

            foreach (var item in dfx_samemonth)
            {
                item.operator_country = item.operator_name.Substring(0, 2).ToUpper();
            }







        }

        private static async Task<List<Categories>> GetCategories()
        {
            var CsvCategories = await System.IO.File.ReadAllLinesAsync(@"C:\temp\categories.csv");

            var categories = await ParseCategories(CsvCategories);

            return categories;
        }

        private static async Task<List<Categories>> ParseCategories(string[] categories)
        {

            CsvFactory.Register<Categories>(builder =>
            {

                builder.Add(a => a.CategoryID).Type(typeof(int)).ColumnName("CategoryID");
                builder.Add(a => a.CategoryName).Type(typeof(string)).ColumnName("CategoryName");

            }, true, ',', categories);

            List<Categories> cats = await CsvFactory.Parse<Categories>();

            return cats;

        }


        public static async Task CsvWriter<T>(List<T> list, string fileName)
        {
            string filePath = fileName + ".csv";
            var exportPath = Path.Combine(@"C:\temp\Mehan\OutData\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
            }
        }


        private static async Task<List<SubscriptionRevenue>> ParseSubscriptions(string[] subscriptions)
        {

            CsvFactory.Register<SubscriptionRevenue>(builder =>
            {

                builder.Add(a => a.subscription_id).Type(typeof(int)).ColumnName("subscription_id");
                builder.Add(a => a.next_subscription_id).Type(typeof(int)).ColumnName("next_subscription_id");
                builder.Add(a => a.is_last_sub).Type(typeof(int)).ColumnName("is_last_sub");
                builder.Add(a => a.utm_medium_at_subscription).Type(typeof(string)).ColumnName("utm_medium_at_subscription");


                builder.Add(a => a.user_id).Type(typeof(int)).ColumnName("user_id");
                builder.Add(a => a.period_type).Type(typeof(string)).ColumnName("period_type");
                builder.Add(a => a.site_lang).Type(typeof(string)).ColumnName("site_lang");
                builder.Add(a => a.currency_code).Type(typeof(string)).ColumnName("currency_code");
                builder.Add(a => a.fully_paid).Type(typeof(int)).ColumnName("fully_paid");
                builder.Add(a => a.currency_amount).Type(typeof(decimal)).ColumnName("currency_amount");
                builder.Add(a => a.payment_gateway).Type(typeof(string)).ColumnName("payment_gateway");
                builder.Add(a => a.operator_name).Type(typeof(string)).ColumnName("operator_name");



                builder.Add(a => a.unique_id).Type(typeof(int)).ColumnName("id");
                builder.Add(a => a.created_date).Type(typeof(string)).ColumnName("created_date");
                builder.Add(a => a.is_first_sub).Type(typeof(int)).ColumnName("is_first_sub");
                builder.Add(a => a.utm_source_at_activation).Type(typeof(string)).ColumnName("utm_source_at_activation");
                builder.Add(a => a.utm_source_at_subscription).Type(typeof(string)).ColumnName("utm_source_at_subscription");
                builder.Add(a => a.utm_medium_at_activation).Type(typeof(string)).ColumnName("utm_medium_last");

                builder.Add(a => a.is_demo).Type(typeof(int)).ColumnName("is_demo");
                builder.Add(a => a.country_code).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.first_subscription_subject_id).Type(typeof(string)).ColumnName("first_subscription_subject_id");
                builder.Add(a => a.tpay_activated_date).Type(typeof(string)).ColumnName("tpay_activated_date");
                builder.Add(a => a.Language).Type(typeof(string)).ColumnName("ei_language_code");




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

        private static async Task<List<Currency>> CurrencyExport()
        {
            HttpClient httpClient = new HttpClient();

            string currencyLink = "http://www.apilayer.net/api/live?access_key=602a9c6d2e8698e65c4eaabf965d87ac&format=1";

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(currencyLink));

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            var currency = JsonConvert.DeserializeObject<CurrencyJson>(data);

            List<Currency> currencies = new List<Currency>();

            if (currency.success)
            {
                //currency.quotes["USDLYD"] = 7.9;
                currency.quotes["USDLYD"] = 6.3;
                currency.quotes["USDILS"] = 3.84;
                currency.quotes["USDTND"] = 2.85;
                currency.quotes["USDEGP"] = 16;
                currency.quotes["USDQAR"] = 3.72;

                foreach (var keyValuePair in currency.quotes)
                {
                    Currency currencyItem = new Currency()
                    {
                        index = keyValuePair.Key,
                        quotes = keyValuePair.Value,
                        currency_code = TruncateLongString(keyValuePair.Key, 3, 3)
                    };

                    currencies.Add(currencyItem);
                }

                //await this.repository.InsertMany(currencies, "Currency", true);
                //string csv = ToCsv<Currency>(",", currencies);
                //System.IO.File.WriteAllText(@"c:\reports\currency.csv", csv);
                return currencies;

            }
            else
            {
                //throw new BusinessException("currency fetch error", "10988");
                throw new Exception("currency fetch error");

            }

        }

        public static string TruncateLongString(string str, int minLenght, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return str.Substring(minLenght, Math.Min(str.Length, maxLength));
        }

        public static DateTime DatetimeMonthlyParse(DateTime date)
        {
            //DateTime yyyyMMdd = new DateTime();
            //yyyyMMdd = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            string str_date = "";
            int year = date.Year;
            int month = date.Month;
            if (month < 10)
            {
                str_date = year.ToString() + "-0" + month.ToString() + "-01";
            }
            else
            {
                str_date = year.ToString() + "-" + month.ToString() + "-01";
            }
            DateTime monthlyDate = new DateTime();
            monthlyDate = DateTime.ParseExact(str_date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            return monthlyDate;
        }


    }
}
