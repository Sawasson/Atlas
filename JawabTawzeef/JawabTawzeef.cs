using CsvFramework;
using CsvHelper;
using JawabTawzeef.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JawabTawzeef
{
    public class JawabTawzeef
    {
        public static async Task<Tuple<List<SubscriptionRevenue>>> RawRevenuesLastDaily()
        {
            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Tawzeef\Base\TAWZEEF-subs-6190862.csv");
            var allSubs = await ParseSubscriptions(AllSubsCSV);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");
            var allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");
            var currencyList = await ParseCurrency(AllCurrencyCSV);

            //copy list
            var revenue = allSubs;

            foreach (var item in revenue)
            {
                item.Parked = ParseParkedDays(item.created_date, item.tpay_activated_date, false);
            }

            revenue = revenue.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
                x.period_type,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                user_id = x.GroupBy(x => x.user_id).Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();



            var lookupCurrency = currencyList.ToLookup(p => p.currency_code);
            var lookupPayouts = allPayouts.ToLookup(p => p.operator_name);

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



                sub.UTM5 = TruncateLongString(sub.utm_source_at_subscription, 0, 5);
                sub.Category = Checkers.CategoryChecker(sub.utm_source_at_subscription);
                sub.UTM7 = TruncateLongString(sub.utm_source_at_subscription, 0, 7);
                sub.Google = Checkers.GoogleChecker(sub.utm_source_at_subscription);
                sub.Apps = Checkers.AppsChecker(sub.Category);
                sub.Apps2 = Checkers.AppsChecker(sub.Category);
            }

            //New List Name
            var MTD_Daily = revenue;

            var MTD_DailyX = MTD_Daily.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.operator_name,
                x.Parked,
                x.period_type,
                x.Category,
            }).Select(x => new RawRevenuesLastDaily
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                Category = x.Key.Category,
                currency_amount = x.Sum(x => x.currency_amount),
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();


            await CsvWriter(MTD_DailyX, "Raw_Revenues_Last_Daily");

            return Tuple.Create(revenue);

        }

        public static async Task RawRevenuesLastMonthly(Tuple<List<SubscriptionRevenue>> list) 
        {
            var revenue = list.Item1;

            foreach (var item in revenue)
            {

                item.created_datex = DateTime.ParseExact(item.created_date, "yyyy-MM-dd",
                                                       System.Globalization.CultureInfo.InvariantCulture);
                var mon = item.created_datex.Month;
            }

            revenue = revenue.GroupBy(x => new
            {
                x.created_datex.Month,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
                x.period_type,
            }).Select(x => new SubscriptionRevenue
            {
                //created_datex = x.GroupBy(x => new { Month = x.created_datex.Month, Year = x.created_datex.Year }).ToDictionary(g => g.Key, g => g.Count()),
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                user_id = x.GroupBy(x => x.user_id).Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            await CsvWriter(revenue, "Raw_Revenues_Last_Monthly");


        }

        public static async Task<Tuple<List<SubscriptionRevenue>, List<SubscriptionRevenue>>> New_LTV_SAMEMONTH()
        {

            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Tawzeef\Base\TAWZEEF-subs-6190862.csv");
            var allSubs = await ParseSubscriptions(AllSubsCSV);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");
            var allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");
            var currencyList = await ParseCurrency(AllCurrencyCSV);

            var revenue = allSubs;

            var userSpendingList = revenue.GroupBy(x => new
            {
                x.user_id,
            }).Select(x => new UserSpending
            {
                user_id = x.Key.user_id,
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();



            var firstSubList = revenue.Where(x => x.is_first_sub == 1).ToList();

            foreach (var sub in firstSubList)
            {
                sub.Parked = ParseParkedDays(sub.tpay_activated_date, sub.created_date, false);
            }


            //copy to firstsublist
            var counts = firstSubList;

            var lookupUserSpendingList = userSpendingList.ToLookup(p => p.user_id);

            foreach (var count in counts)
            {

                var UserSpending = lookupUserSpendingList[count.user_id].FirstOrDefault();
                count.currency_amount = UserSpending.currency_amount;
            }

            var dateCounts = counts.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
                x.period_type,
                x.tpay_activated_date,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                tpay_activated_date = x.Key.tpay_activated_date,
                user_id = x.Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            foreach (var item in dateCounts)
            {

                item.created_datex = DateTime.ParseExact(item.created_date, "yyyy-MM-dd",
                                                       System.Globalization.CultureInfo.InvariantCulture);
            }

            var dateCountsx = dateCounts.GroupBy(x => new
            {
                x.created_datex,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
                x.period_type,
                x.tpay_activated_date,
            }).Select(x => new SubscriptionRevenue
            {
                created_datex = x.Key.created_datex,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                tpay_activated_date = x.Key.tpay_activated_date,
                user_id = x.Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            var dateCountsNewLtv = dateCountsx;

            int i = 0;
            int z = 0;

            var lookupCurrency = currencyList.ToLookup(p => p.currency_code);
            var lookupPayouts = allPayouts.ToLookup(p => p.operator_name);

            foreach (var item in dateCountsNewLtv)
            {
                var currency = lookupCurrency[item.currency_code].FirstOrDefault();

                var payout = lookupPayouts[item.operator_name].FirstOrDefault();

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
                        if (i >= dateCountsNewLtv.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }


                if (payout != null)
                {
                    item.net_usd_amount = item.usd_amount * decimal.Parse(payout.payout_percentage.Replace("%", "")) / 100;
                }


                item.source = Checkers.CategoryChecker(item.utm_source_at_subscription);

                item.tpay_activated_date = item.created_date;

            }


            var dateCountsNewLtv_ = dateCountsNewLtv.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.utm_source_at_subscription,
                x.operator_name,
                x.Parked,
                x.period_type,
            }).Select(x => new NewLTVSameMonth
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                user_id = x.Count(),
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();

            foreach (var item in dateCountsNewLtv_)
            {
                item.Category = Checkers.CategoryChecker(item.utm_source_at_subscription);
            }

            int index = 0;
            foreach (var item in dateCountsNewLtv_)
            {
                item.index = index;
                index++;
            }

            await CsvWriter(dateCountsNewLtv_, "New_LTV_SAMEMONTH");


            return Tuple.Create(dateCountsNewLtv, dateCountsx);


        }

        public static async Task RawFinalReportMonthly(Tuple<List<SubscriptionRevenue>, List<SubscriptionRevenue>> lists)
        {

            var dateCountsNewLtv = lists.Item1;

            var dateCountsx2 = lists.Item2;

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");

            List<OperatorPayout> allPayouts = await ParseOperatorPayouts(PayoutsCSV);


            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");

            List<Currency> currencyList = await ParseCurrency(AllCurrencyCSV);

            var lookupCurrency = currencyList.ToLookup(p => p.currency_code);

            var lookupPayouts = allPayouts.ToLookup(p => p.operator_name);



            foreach (var item in dateCountsx2)
            {
                var currency = lookupCurrency[item.currency_code].FirstOrDefault();

                var payout = lookupPayouts[item.operator_name].FirstOrDefault();

                int i = 0;
                int k = 0;

                if (currency != null)
                {
                    try
                    {
                        item.usd_amount = item.currency_amount / (decimal)currency.quotes;
                        k++;
                    }
                    catch (Exception)
                    {
                        i++;
                        if (i >= dateCountsx2.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }


                if (payout != null)
                {
                    item.net_usd_amount = item.usd_amount * decimal.Parse(payout.payout_percentage.Replace("%", "")) / 100;
                }


                item.UTM5 = TruncateLongString(item.utm_source_at_subscription, 0, 5);
                item.Category = Checkers.CategoryChecker(item.utm_source_at_subscription);
                item.UTM7 = TruncateLongString(item.utm_source_at_subscription, 0, 7);
                item.Google = Checkers.GoogleChecker(item.utm_source_at_subscription);
                item.Apps = Checkers.AppsChecker(item.Category);
                item.Apps2 = Checkers.AppsChecker(item.Category);
                item.operator2 = TruncateLongString(item.country_code, 0, 2) + "-";
                item.country2 = TruncateLongString(item.country_code, 0, 2);
                item.taboola = Checkers.TaboolaChecker(item.utm_source_at_subscription);
                item.postquare = Checkers.PostquareChecker(item.utm_source_at_subscription);


            }

            dateCountsx2 = dateCountsx2.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
                x.utm_source_at_subscription,
                x.operator_name,
                x.Parked,
                x.period_type,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                user_id = x.Count(),
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();

            foreach (var item in dateCountsx2)
            {
                item.Category = Checkers.CategoryChecker(item.utm_source_at_subscription);
            }

            List<RawFinalReportMonthly> RawFinalReportMonthlyList = new List<RawFinalReportMonthly>();

            int f = 0;
            foreach (var item in dateCountsx2)
            {
                RawFinalReportMonthly rawFinalReportMonthly = new RawFinalReportMonthly();
                rawFinalReportMonthly.index = f;
                f++;
                rawFinalReportMonthly.created_date = item.created_date;
                rawFinalReportMonthly.country_code = item.country_code;
                rawFinalReportMonthly.utm_source_at_subscription = item.utm_source_at_subscription;
                rawFinalReportMonthly.operator_name = item.operator_name;
                rawFinalReportMonthly.Parked = item.Parked;
                rawFinalReportMonthly.period_type = item.period_type;
                rawFinalReportMonthly.user_id = item.user_id;
                rawFinalReportMonthly.usd_amount = item.usd_amount;
                rawFinalReportMonthly.net_usd_amount = item.net_usd_amount;
                rawFinalReportMonthly.Category = item.Category;
                RawFinalReportMonthlyList.Add(rawFinalReportMonthly);


            }

            await CsvWriter(RawFinalReportMonthlyList, "Raw_Final_Report_Monthly");


        }


        public static async Task RawDailyCost()
        {
            var DailyCostCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\DailyCost-csv");

            var dailyCost = await ParseDailyCost(DailyCostCSV);

            var DailyCostOldCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\DailyCostOld-csv");

            var dailyCostOld = await ParseDailyCost(DailyCostOldCSV);

            DateTime Date2021 = new DateTime(2021, 1, 1);


            dailyCostOld = dailyCostOld.Where(x => x.Date <= Date2021).ToList();
            dailyCost = dailyCost.Where(x => x.Date >= Date2021).ToList();

            dailyCost = dailyCost.Concat(dailyCostOld).ToList();

            dailyCost = dailyCost.Where(x => x.Dom == "TAWZEEF_EN" || x.Dom == "TAWZEEF_AR" ).ToList();

            await CsvWriter(dailyCost, "daily_costx");

            foreach (var item in dailyCost)
            {
                item.TotalCost = item.SearchCost + item.GDNCost;
            }

            await CsvWriter(dailyCost, "daily_cost_new");

            var dailyCostGroup = dailyCost.GroupBy(x => new
            {
                x.Date,
                x.Cou,
            }).Select(x => new RawDailyCost
            {
                created_date = x.Key.Date,
                country_code = x.Key.Cou,
                SearchCost = x.Sum(x => x.SearchCost),
                GDNCost = x.Sum(x => x.GDNCost),
            }).ToList();

            foreach (var item in dailyCostGroup)
            {
                item.TotalCost = item.SearchCost + item.GDNCost;
            }

            await CsvWriter(dailyCostGroup, "daily_cost");

            dailyCostGroup = dailyCostGroup.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
            }).Select(x => new RawDailyCost
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                SearchCost = x.Sum(x => x.SearchCost),
                GDNCost = x.Sum(x => x.GDNCost),
                TotalCost = x.Sum(x => x.TotalCost),
            }).ToList();

            await CsvWriter(dailyCostGroup, "daily_cost_monthly");

            await CsvWriter(dailyCostGroup, "Raw_daily_cost");

        }

        public static async Task RawMonthlyClicks()
        {

            var DailyCostCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\DailyCost-csv");

            var dailyCost = await ParseDailyCost(DailyCostCSV);

            var DailyCostOldCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\DailyCostOld-csv");

            var dailyCostOld = await ParseDailyCost(DailyCostOldCSV);

            DateTime Date2021 = new DateTime(2021, 1, 1);


            dailyCostOld = dailyCostOld.Where(x => x.Date <= Date2021).ToList();
            dailyCost = dailyCost.Where(x => x.Date >= Date2021).ToList();

            dailyCost = dailyCost.Concat(dailyCostOld).ToList();

            dailyCost = dailyCost.Where(x => x.Dom == "TAWZEEF_EN" || x.Dom == "TAWZEEF_AR").ToList();

            foreach (var item in dailyCost)
            {
                item.TotalCost = item.SearchCost + item.GDNCost;
            }


            var dailyclicksGroup = dailyCost.GroupBy(x => new
            {
                x.Date,
                x.Cou,
            }).Select(x => new RawMonthlyClicks
            {
                created_date = x.Key.Date,
                country_code = x.Key.Cou,
                Clicks = x.Sum(x => x.Clicks),
                GDNClicks = x.Sum(x => x.GDNClicks),
            }).ToList();

            foreach (var item in dailyclicksGroup)
            {
                item.Total_Clicks = item.Clicks + item.GDNClicks;
            }

            await CsvWriter(dailyclicksGroup, "daily_clicks");


            dailyclicksGroup = dailyclicksGroup.GroupBy(x => new
            {
                x.created_date,
                x.country_code,
            }).Select(x => new RawMonthlyClicks
            {
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                Clicks = x.Sum(x => x.Clicks),
                GDNClicks = x.Sum(x => x.GDNClicks),
                Total_Clicks = x.Sum(x => x.Total_Clicks),
            }).ToList();

            await CsvWriter(dailyclicksGroup, "daily_clicks_monthly");

            await CsvWriter(dailyclicksGroup, "Raw_monthly_clicks");

        }


        private static async Task<List<DailyCost>> ParseDailyCost(string[] costs)
        {

            CsvFactory.Register<DailyCost>(builder =>
            {

                builder.Add(a => a.Date).Type(typeof(DateTime)).ColumnName("Date");
                builder.Add(a => a.Dom).Type(typeof(string)).ColumnName("Dom");
                builder.Add(a => a.Cat).Type(typeof(string)).ColumnName("Cat");
                builder.Add(a => a.Cou).Type(typeof(string)).ColumnName("Cou");
                builder.Add(a => a.SearchCost).Type(typeof(decimal)).ColumnName("SearchCost");
                builder.Add(a => a.GDNCost).Type(typeof(decimal)).ColumnName("GDNCost");
                builder.Add(a => a.SearchSubs).Type(typeof(int)).ColumnName("SearchSubs");
                builder.Add(a => a.GDNSubs).Type(typeof(int)).ColumnName("GDNSubs");
                builder.Add(a => a.Clicks).Type(typeof(int)).ColumnName("Clicks");
                builder.Add(a => a.Posted).Type(typeof(int)).ColumnName("Posted");
                builder.Add(a => a.PaidCost).Type(typeof(decimal)).ColumnName("PaidCost");
                builder.Add(a => a.UnPaidCost).Type(typeof(decimal)).ColumnName("UnPaidCost");
                builder.Add(a => a.GDNClicks).Type(typeof(int)).ColumnName("GDNClicks");

            }, true, Convert.ToChar(9), costs);

            List<DailyCost> dailyCost = await CsvFactory.Parse<DailyCost>();

            return dailyCost;

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

        public static async Task<List<SubscriptionRevenue>> ParseSubscriptions(string[] subscriptions)
        {

            CsvFactory.Register<SubscriptionRevenue>(builder =>
            {
                builder.Add(a => a.Id).Type(typeof(object)).ColumnName("Id");
                builder.Add(a => a.user_id).Type(typeof(int)).ColumnName("user_id");
                //builder.Add(a => a.subscription_id).Type(typeof(int)).ColumnName("subscription_id");
                //builder.Add(a => a.next_subscription_id).Type(typeof(int)).ColumnName("next_subscription_id");
                builder.Add(a => a.created_date).Type(typeof(string)).ColumnName("created_date");
                //builder.Add(a => a.created_time).Type(typeof(string)).ColumnName("created_time");
                builder.Add(a => a.is_first_sub).Type(typeof(int)).ColumnName("is_first_sub");
                //builder.Add(a => a.is_last_sub).Type(typeof(int)).ColumnName("is_last_sub");
                builder.Add(a => a.utm_source_at_subscription).Type(typeof(string)).ColumnName("utm_source_at_subscription");
                //builder.Add(a => a.utm_medium_at_subscription).Type(typeof(string)).ColumnName("utm_medium_at_subscription");
                //builder.Add(a => a.expired_time).Type(typeof(string)).ColumnName("expired_time");
                builder.Add(a => a.period_type).Type(typeof(string)).ColumnName("period_type");
                builder.Add(a => a.site_lang).Type(typeof(string)).ColumnName("site_lang");
                builder.Add(a => a.is_demo).Type(typeof(int)).ColumnName("is_demo");
                builder.Add(a => a.country_code).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.currency_code).Type(typeof(string)).ColumnName("currency_code");
                //builder.Add(a => a.ei_language_code).Type(typeof(string)).ColumnName("ei_language_code");
                //builder.Add(a => a.ei_question_id).Type(typeof(int)).ColumnName("ei_question_id");
                //builder.Add(a => a.ei_step3_view_id).Type(typeof(int)).ColumnName("ei_step3_view_id");
                builder.Add(a => a.fully_paid).Type(typeof(int)).ColumnName("fully_paid");
                builder.Add(a => a.currency_amount).Type(typeof(decimal)).ColumnName("currency_amount");
                //builder.Add(a => a.status).Type(typeof(string)).ColumnName("status");
                //builder.Add(a => a.charging_status).Type(typeof(string)).ColumnName("charging_status");
                //builder.Add(a => a.payment_gateway).Type(typeof(string)).ColumnName("payment_gateway");
                //builder.Add(a => a.unsubscribed_by).Type(typeof(string)).ColumnName("unsubscribed_by");
                builder.Add(a => a.first_subscription_subject_id).Type(typeof(int)).ColumnName("first_subscription_subject_id");
                //builder.Add(a => a.utm_source_first).Type(typeof(string)).ColumnName("utm_source_first");
                //builder.Add(a => a.domain_pattern).Type(typeof(string)).ColumnName("domain_pattern");
                //builder.Add(a => a.utm_source_last).Type(typeof(string)).ColumnName("utm_source_last");
                //builder.Add(a => a.utm_medium_first).Type(typeof(string)).ColumnName("utm_medium_first");
                //builder.Add(a => a.utm_medium_last).Type(typeof(string)).ColumnName("utm_medium_last");
                //builder.Add(a => a.phone_confirmed_by_tpay_enreachment).Type(typeof(int)).ColumnName("phone_confirmed_by_tpay_enreachment");
                builder.Add(a => a.tpay_activated_date).Type(typeof(string)).ColumnName("tpay_activated_date");
                //builder.Add(a => a.tpay_activated_time).Type(typeof(string)).ColumnName("tpay_activated_time");
                builder.Add(a => a.operator_name).Type(typeof(string)).ColumnName("operator_name");
                //builder.Add(a => a.subscription_log_type).Type(typeof(string)).ColumnName("subscription_log_type");
                //builder.Add(a => a.tpay_operator_detail_id).Type(typeof(int)).ColumnName("tpay_operator_detail_id");



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

            string currencyLink = "http://www.apilayer.net/api/live?access_key=744f84b0a99557d519fb07a7c4b9b3fa&format=1";

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

        public static async Task CsvWriter<T>(List<T> list, string fileName)
        {
            //string path = "C:/Users/savas/Desktop/list.csv";

            //using (var writer = new StreamWriter(path))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(list);
            //}
            string filePath = fileName + ".csv";
            var exportPath = Path.Combine(@"C:\temp\Tawzeef\OutData\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
            }
        }


    }
}
