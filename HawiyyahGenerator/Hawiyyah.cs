using CsvFramework;
using CsvHelper;
using HawiyyahGenerator.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HawiyyahGenerator
{
    public class Hawiyyah
    {
        //string[] CPA_TOOL_SETTING = new string[]{ "HAWEYYA_ENGLISH", "HAWEYYA_ARABIC", "HAWEYYA_CC_ENGLISH" };
        
        //string[] CPA_TOOL_SETTING_HAWEYYA = new string[] { "HAWEYYA_ENGLISH", "HAWEYYA_ARABIC" };

        //string[] CPA_TOOL_SETTING_PEOPLEREVEAL = new string[] { "HAWEYYA_CC_ENGLISH" };

        public static async Task<Tuple<List<RevenuesLast>, List<RevenuesLast>, List<RevenuesLast>>> RevenuesLast()
        {
            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Hawiyyah\Base\HAWIYYAH-subs-2304771.csv");
            var allSubs = await ParseSubscriptions(AllSubsCSV);


            allSubs = DomainParser(allSubs);

            allSubs = TpayActivitedDateFill(allSubs);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");

            var allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");

            var currencyList = await ParseCurrency(AllCurrencyCSV);

            //copy list
            var revenue = allSubs;

            //Unıque List
            var uniques = UniqueItems(revenue);

            foreach (var item in revenue)
            {
                item.Parked = ParseParkedDays(item.created_date, item.tpay_activated_date);
            }

            //Unıque List
            var uniques2 = UniqueItems(revenue);

            foreach (var item in revenue)
            {
                if (string.IsNullOrEmpty(item.operator_name))
                {
                    item.operator_name = "NoOperator";
                }
            }

            revenue = revenue.GroupBy(x => new
            {
                x.domain,
                x.created_date,
                x.payment_gateway,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
            }).Select(x => new SubscriptionRevenue
            {
                domain = x.Key.domain,
                created_date = x.Key.created_date,
                payment_gateway = x.Key.payment_gateway,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                //user_id = x.GroupBy(c => c.user_id).Where(grp => grp.Count() > 1).Select(grp => grp.Key).FirstOrDefault(),
                user_id = x.GroupBy(x=>x.user_id).Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            //Unıque List
            var uniques3 = UniqueItems(revenue);

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
                        sub.quotes = currency.quotes;
                        sub.Index = currency.index;
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
                    sub.payout_percentage = (decimal.Parse(payout.payout_percentage.Replace("%", "")) / 100).ToString();
                }



                sub.UTM5 = TruncateLongString(sub.utm_source_at_subscription, 0, 5);

            }

            //Unıque List
            var uniques4 = UniqueItems(revenue);


            //HAWIYYAH ALL

            List<RevenuesLast> revenuesLast = new List<RevenuesLast>();

            int k = 0;
            foreach (var item in revenue)
            {
                RevenuesLast rev = new RevenuesLast();
                rev.index_0 = k;
                k++;
                rev.created_date = item.created_date;
                rev.payment_gateway = item.payment_gateway;
                rev.country_code = item.country_code;
                rev.utm_source_at_subscription = item.utm_source_at_subscription;
                rev.currency_code = item.currency_code;
                rev.country_code = item.country_code;
                rev.operator_name = item.operator_name;
                rev.Parked = item.Parked;
                rev.user_id = item.user_id;
                rev.currency_amount = item.currency_amount;
                rev.Index = item.Index;
                rev.quotes = item.quotes;
                rev.payout_percentage = item.payout_percentage;
                rev.usd_amount = item.usd_amount;
                rev.net_usd_amount = item.net_usd_amount;
                rev.UTM5 = item.UTM5;
                revenuesLast.Add(rev);
            }

            await CsvWriter(revenuesLast, "revenues_last");

            //HAWIYYAH ONLY

            List<RevenuesLast> revenuesLast_hawiyyahOnly = new List<RevenuesLast>();

            int m = 0;
            foreach (var item in revenue.Where(x => x.domain != "PEOPLEREVEAL"))
            {
                RevenuesLast rev = new RevenuesLast();
                rev.index_0 = m;
                m++;
                rev.created_date = item.created_date;
                rev.payment_gateway = item.payment_gateway;
                rev.country_code = item.country_code;
                rev.utm_source_at_subscription = item.utm_source_at_subscription;
                rev.currency_code = item.currency_code;
                rev.country_code = item.country_code;
                rev.operator_name = item.operator_name;
                rev.Parked = item.Parked;
                rev.user_id = item.user_id;
                rev.currency_amount = item.currency_amount;
                rev.Index = item.Index;
                rev.quotes = item.quotes;
                rev.payout_percentage = item.payout_percentage;
                rev.usd_amount = item.usd_amount;
                rev.net_usd_amount = item.net_usd_amount;
                rev.UTM5 = item.UTM5;
                revenuesLast_hawiyyahOnly.Add(rev);
            }

            await CsvWriter(revenuesLast_hawiyyahOnly, "revenuesLast_hawiyyahOnly");

            //PEOPLEREVEAL
            List<RevenuesLast> revenuesLast_peopleReveal = new List<RevenuesLast>();

            foreach (var item in revenue.Where(x => x.domain == "PEOPLEREVEAL"))
            {
                RevenuesLast rev = new RevenuesLast();
                rev.index_0 = m;
                m++;
                rev.created_date = item.created_date;
                rev.payment_gateway = item.payment_gateway;
                rev.country_code = item.country_code;
                rev.utm_source_at_subscription = item.utm_source_at_subscription;
                rev.currency_code = item.currency_code;
                rev.country_code = item.country_code;
                rev.operator_name = item.operator_name;
                rev.Parked = item.Parked;
                rev.user_id = item.user_id;
                rev.currency_amount = item.currency_amount;
                rev.Index = item.Index;
                rev.quotes = item.quotes;
                rev.payout_percentage = item.payout_percentage;
                rev.usd_amount = item.usd_amount;
                rev.net_usd_amount = item.net_usd_amount;
                rev.UTM5 = item.UTM5;
                revenuesLast_peopleReveal.Add(rev);
            }

            //await CsvWriter (revenuesLast_peopleReveal, "revenuesLast_peopleReveal");

            return Tuple.Create(revenuesLast, revenuesLast_hawiyyahOnly, revenuesLast_peopleReveal);


        }

        public static async Task<Tuple<List<NewLTVSameMonth>, List<SubscriptionRevenue>>> NewLTVSameMonth()
        {

            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Hawiyyah\Base\HAWIYYAH-subs-2304771.csv");

            var allSubs = await ParseSubscriptions(AllSubsCSV);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");

            var allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");

            var currencyList = await ParseCurrency(AllCurrencyCSV);

            allSubs = DomainParser(allSubs);

            allSubs = TpayActivitedDateFill(allSubs);

            var userSpendingList = allSubs.GroupBy(x => new
            {
                x.user_id,
            }).Select(x => new UserSpending
            {
                user_id = x.Key.user_id,
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            var firstSubList = allSubs.Where(x => x.is_first_sub == 1).ToList();

            foreach (var sub in firstSubList)
            {
                sub.Parked = ParseParkedDays(sub.tpay_activated_date, sub.created_date);


                if (string.IsNullOrEmpty(sub.operator_name))
                {
                    sub.operator_name = "NoOperator";
                }
            }

            ////copy to firstsublist
            //var subReportx = firstSubList;
            ////copy to firstsublist

            var counts = firstSubList;

            var lookupUserSpendingList = userSpendingList.ToLookup(p => p.user_id);

            foreach (var count in counts)
            {

                var UserSpending = lookupUserSpendingList[count.user_id].FirstOrDefault();
                count.currency_amount = UserSpending.currency_amount;
            }

            var dateCounts = counts.GroupBy(x => new
            {
                x.domain,
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
                domain = x.Key.domain,
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
                if (item.utm_source_at_subscription.ToLower().Contains("eng"))
                {
                    item.lang = "en";
                }
                else
                {
                    item.lang = "ar";
                }
            }

            var dateCountsx = dateCounts.GroupBy(x => new
            {
                x.domain,
                x.created_date,
                x.country_code,
                x.utm_source_at_subscription,
                x.currency_code,
                x.operator_name,
                x.Parked,
                x.lang,
                x.period_type,
                x.tpay_activated_date,
            }).Select(x => new SubscriptionRevenue
            {
                domain = x.Key.domain,
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                currency_code = x.Key.currency_code,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                lang = x.Key.lang,
                period_type = x.Key.period_type,
                tpay_activated_date = x.Key.tpay_activated_date,
                user_id = x.Sum(x=>x.user_id),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();

            List<SubscriptionRevenue> dateCountsNewLtv = new List<SubscriptionRevenue>();

            //Todo: Clonlanacak
            foreach (var item in dateCountsx)
            {
                dateCountsNewLtv.Add(new SubscriptionRevenue

                {
                    Category = item.Category,
                    country_code = item.country_code,
                    created_date = item.created_date,
                    currency_amount = item.currency_amount,
                    currency_code = item.currency_code,
                    first_subscription_subject_id = item.first_subscription_subject_id,
                    fully_paid = item.fully_paid,
                    Id = item.Id,
                    is_demo = item.is_demo,
                    is_first_sub = item.is_first_sub,
                    is_last_sub = item.is_last_sub,
                    model = item.model,
                    net_usd_amount = item.net_usd_amount,
                    next_subscription_id = item.next_subscription_id,
                    operator_name = item.operator_name,
                    Parked = item.Parked,
                    payment_gateway = item.payment_gateway,
                    period_type = item.period_type,
                    site_lang = item.site_lang,
                    source = item.source,
                    subscription_id = item.subscription_id,
                    tpay_activated_date = item.tpay_activated_date,
                    usd_amount = item.usd_amount,
                    user_id = item.user_id,
                    UTM5 = item.UTM5,
                    utm_medium_at_subscription = item.utm_medium_at_subscription,
                    utm_source_at_subscription = item.utm_source_at_subscription,
                });
            }

            int i = 0;
            int z = 0;

            var lookupCurrency = currencyList.ToLookup(p => p.currency_code);
            var lookupPayouts = allPayouts.ToLookup(p => p.operator_name);

            foreach (var item in dateCountsNewLtv)
            {

                item.tpay_activated_date = DatetimeMonthlyParse(item.tpay_activated_date);
                item.created_date = DatetimeMonthlyParse(item.created_date);

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


            }

            dateCountsNewLtv = dateCountsNewLtv.Where(x => x.tpay_activated_date == x.created_date).ToList();


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
                user_id = x.Sum(x=>x.user_id),
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();

            int index = 0;
            foreach (var item in dateCountsNewLtv_)
            {
                item.Category = Checkers.CategoryChecker(item.utm_source_at_subscription);
                item.model = "SAMEMONTH";
                item.index = index;
                index++;
            }

            //await CsvWriter(dateCountsNewLtv, "New_LTV_SAMEMONTH");

            return Tuple.Create(dateCountsNewLtv_, dateCountsx);
        }

        public static async Task<List<LTVModels>> LTVModels(Tuple<List<LTVModels>, List<SubscriptionRevenue>> lists)
        {
            List<LTVModels> lTVModels = new List<LTVModels>();

            var dateCountsNewLtv = lists.Item1;

            var dateCountsx2 = lists.Item2;

            //foreach (var item in dateCountsNewLtv)
            //{
            //    item.model = "SAMEMONTH";
            //}

            //foreach (var item in dateCountsx2)
            //{
            //    item.model = "OLDMODEL";
            //}

            int m = 0;
            int o = 0;
            int s = 0;

            foreach (var item in dateCountsx2)
            {
                LTVModels oldModel = new LTVModels();
                oldModel.main_index = m;
                m++;
                oldModel.index = o;
                o++;
                oldModel.created_date = item.created_date;
                oldModel.country_code = item.country_code;
                oldModel.utm_source_at_subscription = item.utm_source_at_subscription;
                oldModel.operator_name = item.operator_name;
                oldModel.Parked = item.Parked;
                oldModel.period_type = item.period_type;
                oldModel.user_id = item.user_id;
                oldModel.usd_amount = item.usd_amount;
                oldModel.net_usd_amount = item.net_usd_amount;
                oldModel.Category = item.Category;
                oldModel.model = "OLDMODEL";
                lTVModels.Add(oldModel);

            }

            foreach (var item in dateCountsNewLtv)
            {
                LTVModels samemonth = new LTVModels();
                samemonth.main_index = m;
                m++;
                samemonth.index = s;
                s++;
                samemonth.created_date = item.created_date;
                samemonth.country_code = item.country_code;
                samemonth.utm_source_at_subscription = item.utm_source_at_subscription;
                samemonth.operator_name = item.operator_name;
                samemonth.Parked = item.Parked;
                samemonth.period_type = item.period_type;
                samemonth.user_id = item.user_id;
                samemonth.usd_amount = item.usd_amount;
                samemonth.net_usd_amount = item.net_usd_amount;
                samemonth.Category = item.Category;
                samemonth.model = "SAMEMONTH";
                lTVModels.Add(samemonth);

            }

            await CsvWriter(lTVModels, "LTV_Models");

            return lTVModels;

        }

        public static async Task<Tuple<List<FirstSubReport>, List<FirstSubReport>, List<FirstSubReport>>> FirstSubReport(Tuple<List<NewLTVSameMonth>, List<SubscriptionRevenue>> lists)
        {

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");

            var allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");

            var currencyList = await ParseCurrency(AllCurrencyCSV);

            var dateCountsNewLtv = lists.Item1;

            var dateCountsx2 = lists.Item2;

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
                    catch (Exception ex)
                    {
                        i++;
                        if (i >= dateCountsx2.Count * 0.01)
                        {
                            throw ex;
                        }
                    }
                }


                if (payout != null)
                {
                    item.net_usd_amount = item.usd_amount * decimal.Parse(payout.payout_percentage.Replace("%", "")) / 100;
                }


                item.Category = Checkers.CategoryChecker(item.utm_source_at_subscription);

            }

            dateCountsx2 = dateCountsx2.GroupBy(x => new
            {
                x.domain,
                x.created_date,
                x.country_code,
                x.utm_source_at_subscription,
                x.operator_name,
                x.Parked,
                x.period_type,
                x.Category,
            }).Select(x => new SubscriptionRevenue
            {
                domain = x.Key.domain,
                created_date = x.Key.created_date,
                country_code = x.Key.country_code,
                utm_source_at_subscription = x.Key.utm_source_at_subscription,
                operator_name = x.Key.operator_name,
                Parked = x.Key.Parked,
                period_type = x.Key.period_type,
                Category = x.Key.Category,
                user_id = x.Sum(x=>x.user_id),
                usd_amount = x.Sum(x => x.usd_amount),
                net_usd_amount = x.Sum(x => x.net_usd_amount),
            }).ToList();


            //HAWIYYAH ALL

            List<FirstSubReport> firstSubReportList = new List<FirstSubReport>();

            int f = 0;
            foreach (var item in dateCountsx2)
            {
                FirstSubReport firstSubReport = new FirstSubReport();
                firstSubReport.index = f;
                f++;
                firstSubReport.created_date = item.created_date;
                firstSubReport.country_code = item.country_code;
                firstSubReport.utm_source_at_subscription = item.utm_source_at_subscription;
                firstSubReport.operator_name = item.operator_name;
                firstSubReport.Parked = item.Parked;
                firstSubReport.period_type = item.period_type;
                firstSubReport.Category = item.Category;
                firstSubReport.user_id = item.user_id;
                firstSubReport.usd_amount = item.usd_amount;
                firstSubReport.net_usd_amount = item.net_usd_amount;
                firstSubReport.domain = item.domain;
                firstSubReportList.Add(firstSubReport);


            }

            await CsvWriter(firstSubReportList, "first_sub_report");

            //HAWIYYAH ONLY

            List<FirstSubReport> firstSubReportList_Hawiyyah = new List<FirstSubReport>();

            int n = 0;
            foreach (var item in dateCountsx2.Where(x => x.domain != "PEOPLEREVEAL"))
            {
                FirstSubReport firstSubReport = new FirstSubReport();
                firstSubReport.index = n;
                n++;
                firstSubReport.created_date = item.created_date;
                firstSubReport.country_code = item.country_code;
                firstSubReport.utm_source_at_subscription = item.utm_source_at_subscription;
                firstSubReport.operator_name = item.operator_name;
                firstSubReport.Parked = item.Parked;
                firstSubReport.period_type = item.period_type;
                firstSubReport.Category = item.Category;
                firstSubReport.user_id = item.user_id;
                firstSubReport.usd_amount = item.usd_amount;
                firstSubReport.net_usd_amount = item.net_usd_amount;
                firstSubReport.domain = item.domain;
                firstSubReportList_Hawiyyah.Add(firstSubReport);


            }

            await CsvWriter(firstSubReportList_Hawiyyah, "first_sub_report_hawiyyah");

            //PEOPLEREVEAL

            List<FirstSubReport> firstSubReportList_PeopleReveal = new List<FirstSubReport>();

            foreach (var item in dateCountsx2.Where(x => x.domain == "PEOPLEREVEAL"))
            {
                FirstSubReport firstSubReport = new FirstSubReport();
                firstSubReport.index = n;
                n++;
                firstSubReport.created_date = item.created_date;
                firstSubReport.country_code = item.country_code;
                firstSubReport.utm_source_at_subscription = item.utm_source_at_subscription;
                firstSubReport.operator_name = item.operator_name;
                firstSubReport.Parked = item.Parked;
                firstSubReport.period_type = item.period_type;
                firstSubReport.Category = item.Category;
                firstSubReport.user_id = item.user_id;
                firstSubReport.usd_amount = item.usd_amount;
                firstSubReport.net_usd_amount = item.net_usd_amount;
                firstSubReport.domain = item.domain;
                firstSubReportList_PeopleReveal.Add(firstSubReport);


            }

            await CsvWriter(firstSubReportList_PeopleReveal, "first_sub_report_peopleReveal");


            return Tuple.Create(firstSubReportList, firstSubReportList_Hawiyyah,firstSubReportList_PeopleReveal);


        }














        private static bool ParseParkedDays(string date1, string date2)
        {
            try
            {
                if (date1 == date2)
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

                return false;
            }

        }

        private static string ParseParkedDaysToString(bool date)
        {
            try
            {
                if (date==false)
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
            catch (Exception)
            {

                return "0";
            }

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
            var exportPath = Path.Combine(@"C:\temp\Hawiyyah\OutData\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
            }
        }

        private static async Task<List<Currency>> CurrencyExport()
        {
            HttpClient httpClient = new HttpClient();

            string currencyLink = "http://www.apilayer.net/api/live?access_key=152056bca33a250da4dbbc38b8a021e0&format=1";

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(currencyLink));

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var data = await response.Content.ReadAsStringAsync();

            var currency = JsonConvert.DeserializeObject<CurrencyJson>(data);

            List<Currency> currencies = new List<Currency>();

            if (currency.success)
            {
                currency.quotes["USDLYD"] = 5;
                //currency.quotes["USDILS"] = 3.84;
                //currency.quotes["USDTND"] = 2.85;
                //currency.quotes["USDEGP"] = 16;
                //currency.quotes["USDQAR"] = 3.72;

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

        public static List<SubscriptionRevenue> DomainParser(List<SubscriptionRevenue> list)
        {
            foreach (var item in list)
            {
                if (item.domain_pattern.ToLower().Contains("peoplereveal"))
                {
                    item.domain = "PEOPLEREVEAL";
                }
                else
                {
                    item.domain = "HAWIYYAH";
                }
            }

            return list;
        }

        public static List<SubscriptionRevenue> TpayActivitedDateFill(List<SubscriptionRevenue> list)
        {
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.tpay_activated_date))
                {
                    item.tpay_activated_date = item.created_date;
                }
            }

            return list;
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
                builder.Add(a => a.domain_pattern).Type(typeof(string)).ColumnName("domain_pattern");
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

        public static List<string> UniqueItems(IEnumerable<SubscriptionRevenue> list)
        {
            List<string> uniques = new List<string>();

            foreach (var item in list)
            {
                if (!uniques.Contains(item.payment_gateway)) uniques.Add(item.payment_gateway);
            }
            return uniques;
        }

        public static string TruncateLongString(string str, int minLenght, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return str.Substring(minLenght, Math.Min(str.Length, maxLength));
        }

        public static string DatetimeMonthlyParse(string date)
        {
            try
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
            catch (Exception)
            {

                return null;
            }

        }


    }
}
