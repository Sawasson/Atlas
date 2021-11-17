using CsvFramework;
using InitialPriceLTV.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InitialPriceLTV
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GroupLtvOnPrice();
        }

        public static async Task GroupLtvOnPrice()
        {


            var AllSubsCSVJawab = await System.IO.File.ReadAllLinesAsync(@"C:\temp\users_subscriptions.csv");
            var jawab = new List<SubscriptionRevenue>();
            try
            {
                jawab = await ParseSubscriptions(AllSubsCSVJawab);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            var AllSubsCSVSale = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Jawabsale\Base\SALE-subs-55419400.csv");

            var jawabsale = await ParseSubscriptions(AllSubsCSVSale);


            foreach (var item in jawab)
            {
                item.Parked = ParseParkedDays(item.created_date, item.tpay_activated_date, false);
                item.initial_charge = item.currency_amount;
                item.source = Checkers.CategoryChecker(item.utm_source_at_subscription);
            }

            foreach (var item in jawabsale)
            {
                item.Parked = ParseParkedDays(item.created_date, item.tpay_activated_date, false);
                item.initial_charge = item.currency_amount;
                item.source = Checkers.CategoryChecker(item.utm_source_at_subscription);
            }


            var jawab_spend = jawab.GroupBy(x => new
            {
                x.user_id,
            }).Select(x => new SubscriptionRevenue
            {
                user_id = x.Key.user_id,
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            var sale_spend = jawabsale.GroupBy(x => new
            {
                x.user_id,
            }).Select(x => new SubscriptionRevenue
            {
                user_id = x.Key.user_id,
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();


            var jawab_first_sub = jawab.Where(x => x.is_first_sub == 1).ToList();

            var sale_first_sub = jawabsale.Where(x => x.is_first_sub == 1).ToList();


            var lookupJawabSpend = jawab_spend.ToLookup(p => p.user_id);

            var lookupSaleSpend = sale_spend.ToLookup(p => p.user_id);


            var jawab_ltv = jawab;

            foreach (var item in jawab_ltv)
            {
                var spend = lookupJawabSpend[item.user_id].FirstOrDefault();
                item.currency_amount = spend.currency_amount;
            }

            var sale_ltv = jawabsale;

            foreach (var item in sale_ltv)
            {
                var spend = lookupSaleSpend[item.user_id].FirstOrDefault();
                item.currency_amount = spend.currency_amount;
            }

            foreach (var item in jawab_ltv)
            {
                item.tpay_activated_dateTime = DateTime.ParseExact(item.tpay_activated_date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }

            foreach (var item in sale_ltv)
            {
                item.tpay_activated_dateTime = DateTime.ParseExact(item.tpay_activated_date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }

            jawab_ltv = jawab_ltv.Where(x => x.created_dateTime >= new DateTime(2018, 01, 01)).ToList();

            sale_ltv = sale_ltv.Where(x => x.created_dateTime >= new DateTime(2018, 01, 01)).ToList();


            jawab_ltv = jawab_ltv.GroupBy(x => new
            {
                x.created_date,
                x.tpay_activated_date,
                x.period_type,
                x.country_code,
                x.operator_name,
                x.currency_code,
                x.first_subscription_subject_id,
                x.source,
                x.site_lang,
                x.Parked,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                tpay_activated_date = x.Key.tpay_activated_date,
                period_type = x.Key.period_type,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                currency_code = x.Key.currency_code,
                first_subscription_subject_id = x.Key.first_subscription_subject_id,
                source = x.Key.source,
                site_lang = x.Key.site_lang,
                Parked = x.Key.Parked,
                user_id = x.Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();



            sale_ltv = sale_ltv.GroupBy(x => new
            {
                x.created_date,
                x.tpay_activated_date,
                x.period_type,
                x.country_code,
                x.operator_name,
                x.currency_code,
                x.first_subscription_subject_id,
                x.source,
                x.site_lang,
                x.Parked,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                tpay_activated_date = x.Key.tpay_activated_date,
                period_type = x.Key.period_type,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                currency_code = x.Key.currency_code,
                first_subscription_subject_id = x.Key.first_subscription_subject_id,
                source = x.Key.source,
                site_lang = x.Key.site_lang,
                Parked = x.Key.Parked,
                user_id = x.Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();



            jawab_ltv = jawab_ltv.Where(x => x.tpay_activated_dateTime >= new DateTime(2018, 01, 01)).ToList();

            sale_ltv = sale_ltv.Where(x => x.tpay_activated_dateTime >= new DateTime(2018, 01, 01)).ToList();

            jawab_ltv = jawab_ltv.Where(x => x.created_dateTime >= new DateTime(2018, 01, 01)).ToList();

            sale_ltv = sale_ltv.Where(x => x.created_dateTime >= new DateTime(2018, 01, 01)).ToList();

            foreach (var item in jawab_ltv)
            {
                item.Project = "Jawabkom";
                item.Model = "OldModel";
            }

            foreach (var item in sale_ltv)
            {
                item.Project = "Jawabsale";
                item.Model = "OldModel";
            }

            var jawab_ltv_group_same = jawab_ltv;

            foreach (var item in jawab_ltv_group_same)
            {
                item.created_date = DatetimeMonthlyParse(item.created_date);
                item.tpay_activated_date = DatetimeMonthlyParse(item.tpay_activated_date);
            }

            jawab_ltv_group_same = jawab_ltv_group_same.Where(x => x.created_date == x.tpay_activated_date).ToList();

            foreach (var item in jawab_ltv_group_same)
            {
                item.Model = "SameMonth";
            }

            var sale_ltv_group_same = sale_ltv;

            foreach (var item in sale_ltv_group_same)
            {
                item.created_date = DatetimeMonthlyParse(item.created_date);
                item.tpay_activated_date = DatetimeMonthlyParse(item.tpay_activated_date);
            }

            sale_ltv_group_same = sale_ltv_group_same.Where(x => x.created_date == x.tpay_activated_date).ToList();

            foreach (var item in sale_ltv_group_same)
            {
                item.Model = "SameMonth";
            }

            var groups = jawab_ltv.Concat(sale_ltv).ToList();
            groups = groups.Concat(jawab_ltv_group_same).ToList();
            groups = groups.Concat(sale_ltv_group_same).ToList();

            groups = groups.OrderByDescending(x => x.created_date).ToList();


            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");

            var currencyList = await ParseCurrency(AllCurrencyCSV);

            var lookupCurrency = currencyList.ToLookup(p => p.currency_code);

            var groups_currency = groups;

            int i = 0;
            int z = 0;

            foreach (var item in groups_currency)
            {
                var currency = lookupCurrency[item.currency_code].FirstOrDefault();

                if (currency != null)
                {
                    try
                    {
                        item.usd_amount = item.currency_amount / (decimal)currency.quotes;
                        z++;
                    }
                    catch (Exception)
                    {
                        i++;
                        if (i >= groups_currency.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }
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



                //builder.Add(a => a.unique_id).Type(typeof(int)).ColumnName("id");
                builder.Add(a => a.created_date).Type(typeof(string)).ColumnName("created_date");
                builder.Add(a => a.is_first_sub).Type(typeof(int)).ColumnName("is_first_sub");
                //builder.Add(a => a.utm_source_at_activation).Type(typeof(string)).ColumnName("utm_source_at_activation");
                builder.Add(a => a.utm_source_at_subscription).Type(typeof(string)).ColumnName("utm_source_at_subscription");
                //builder.Add(a => a.utm_medium_at_activation).Type(typeof(string)).ColumnName("utm_medium_last");

                builder.Add(a => a.is_demo).Type(typeof(int)).ColumnName("is_demo");
                builder.Add(a => a.country_code).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.first_subscription_subject_id).Type(typeof(string)).ColumnName("first_subscription_subject_id");
                builder.Add(a => a.tpay_activated_date).Type(typeof(string)).ColumnName("tpay_activated_date");
                //builder.Add(a => a.Language).Type(typeof(string)).ColumnName("ei_language_code");




            }, true, ',', subscriptions);

            List<SubscriptionRevenue> subs = await CsvFactory.Parse(new SubscriptionRevenueParser());

            return subs;

        }

        private static bool ParseParkedDays(string date1, string date2, bool parked)
        {
            try
            {
                DateTime tpay_activated_date = DateTime.ParseExact(date1, "yyyy-MM-dd",
                           System.Globalization.CultureInfo.InvariantCulture);
                DateTime created_date = DateTime.ParseExact(date2, "yyyy-MM-dd",
                       System.Globalization.CultureInfo.InvariantCulture);
                var ParkedDays = (tpay_activated_date - created_date).TotalDays;
                if (ParkedDays == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                return parked;
            }


        }

        public static string DatetimeMonthlyParse(string date)
        {
            DateTime yyyyMMdd = new DateTime();
            yyyyMMdd = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            int year = yyyyMMdd.Year;
            int month = yyyyMMdd.Month;
            if (month < 10)
            {
                date = year.ToString() + "-0" + month.ToString() + "-01";
            }
            else
            {
                date = year.ToString() + "-" + month.ToString() + "-01";
            }

            return date;
        }

        private static async Task<List<Currency>> CurrencyExport()
        {
            HttpClient httpClient = new HttpClient();

            //string currencyLink = "http://www.apilayer.net/api/live?access_key=744f84b0a99557d519fb07a7c4b9b3fa&format=1";

            string currencyLink = "http://www.apilayer.net/api/live?access_key=602a9c6d2e8698e65c4eaabf965d87ac&format=1";


            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(currencyLink));

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            var currency = JsonConvert.DeserializeObject<CurrencyJson>(data);

            List<Currency> currencies = new List<Currency>();

            if (currency.success)
            {
                currency.quotes["USDLYD"] = 7.9;
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


    }
}
