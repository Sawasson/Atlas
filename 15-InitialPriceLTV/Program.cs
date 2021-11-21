using _15_InitialPriceLTV.Core;
using CsvFramework;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV
{
    class Program
    {

        private static async Task<List<SubscriptionRevenue>> ParseSubscriptions2(string[] subscriptions)
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
        public static List<SubscriptionRevenue> allSubs { get; set; }

        static async Task Main(string[] args)
        {
            var list = await GroupLtvOnPrice();
            await LtvReportCategoryMergeTest2(list);
            await SaleandComLTV(list);

        }

        public static async Task<List<SubscriptionRevenue>> GroupLtvOnPrice()
        {
            var AllSubsCSVJawab = await System.IO.File.ReadAllLinesAsync(@"C:\temp\users_subscriptions.csv");


            var jawab = new List<SubscriptionRevenue>();
            try
            {
                jawab = await ParseSubscriptions2(AllSubsCSVJawab);

            }
            catch (Exception ex)
            {

                throw;
            }

            //var allsubs = await Jawabkom_Generator3.Jawabkom.AllSubs();

            //List<SubscriptionRevenue> jawab = new List<SubscriptionRevenue>();

            //foreach (var item in allSubs)
            //{
            //    SubscriptionRevenue sub = new SubscriptionRevenue();
            //    sub.created_date = item.created_date

            //}



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

            //foreach (var item in jawab_ltv)
            //{
            //    try
            //    {
            //        item.tpay_activated_dateTime = DateTime.ParseExact(item.tpay_activated_date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            //    }
            //    catch (Exception ex)
            //    {
            //        item.tpay_activated_dateTime = item.tpay_activated_dateTime;
            //    }
            //}

            //jawab_ltv = jawab_ltv.Where(x => x.tpay_activated_date >= new DateTime(2018, 01, 01).ToString()).ToList();



            //foreach (var item in sale_ltv)
            //{
            //    try
            //    {
            //        item.tpay_activated_dateTime = DateTime.ParseExact(item.tpay_activated_date, "dd-MM-yy", System.Globalization.CultureInfo.InvariantCulture);
            //    }
            //    catch (Exception ex)
            //    {
            //        item.tpay_activated_dateTime = item.tpay_activated_dateTime;
            //    }
            //}

            //sale_ltv = sale_ltv.Where(x => x.tpay_activated_dateTime >= new DateTime(2018, 01, 01)).ToList();




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
                x.utm_medium_at_subscription,
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
                utm_medium_at_subscription = x.Key.utm_medium_at_subscription,
                user_id = x.Count(),
                currency_amount = x.Sum(x => x.currency_amount),
            }).ToList();


            //jawab_ltv = jawab_ltv.Where(x => x.tpay_activated_dateTime >= new DateTime(2018, 01, 01)).ToList();

            //sale_ltv = sale_ltv.Where(x => x.tpay_activated_dateTime >= new DateTime(2018, 01, 01)).ToList();

            //jawab_ltv = jawab_ltv.Where(x => x.created_dateTime >= new DateTime(2018, 01, 01)).ToList();

            //sale_ltv = sale_ltv.Where(x => x.created_dateTime >= new DateTime(2018, 01, 01)).ToList();

            foreach (var item in jawab_ltv)
            {
                item.Project = "Jawabkom";
                item.model = "OldModel";
            }



            foreach (var item in sale_ltv)
            {
                item.Project = "Jawabsale";
                item.model = "OldModel";
            }

            var jawab_ltv_group_same = jawab_ltv;

            foreach (var item in jawab_ltv_group_same)
            {
                try
                {
                    item.created_date = DatetimeMonthlyParse(item.created_date);
                    item.tpay_activated_date = DatetimeMonthlyParse(item.tpay_activated_date);
                }
                catch (Exception)
                {
                }

            }

            jawab_ltv_group_same = jawab_ltv_group_same.Where(x => x.created_date == x.tpay_activated_date).ToList();

            foreach (var item in jawab_ltv_group_same)
            {
                item.model = "SameMonth";
            }

            var sale_ltv_group_same = sale_ltv;

            foreach (var item in sale_ltv_group_same)
            {
                try
                {
                    item.created_date = DatetimeMonthlyParse(item.created_date);
                    item.tpay_activated_date = DatetimeMonthlyParse(item.tpay_activated_date);
                }
                catch (Exception)
                {
                    item.tpay_activated_date = item.tpay_activated_date;
                }

            }

            sale_ltv_group_same = sale_ltv_group_same.Where(x => x.created_date == x.tpay_activated_date).ToList();

            foreach (var item in sale_ltv_group_same)
            {
                item.model = "SameMonth";
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
                        //if (i >= groups_currency.Count * 0.01)
                        {
                            throw;
                        }
                    }
                }
            }

            var categoryList = await GetCategories();

            var lookupCategories = categoryList.ToLookup(p => p.CategoryID);

            //int32 range test
            var xx = groups_currency.GroupBy(x => x.first_subscription_subject_id).ToList();

            foreach (var item in groups_currency)
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
                            if (i >= groups_currency.Count * 0.01)
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


            int uu = 0;
            foreach (var item in groups_currency)
            {
                if (item.Project == "Jawabsale")
                {
                    item.CategoryName2 = Checkers.JawabsaleCategories(item.utm_medium_at_subscription);
                    uu++;
                }
            }

            await CsvWriter(groups_currency, "group_ltvs_on_price");

            return groups_currency;



        }


        public static async Task LtvReportCategoryMerge(List<SubscriptionRevenue> list)
        {
            var ltv_report = list;

            ltv_report = ltv_report.Where(x => x.model == "SameMonth").ToList();

            //string[] ltv_months = new string[] { "2019-06-01", "2019-07-01", "2019-08-01" };

            //ltv_report = ltv_report.Where(x => x.created_date == ltv_months[0]).ToList();

            var ltv_report_category_ = ltv_report.GroupBy(x => new
            {
                x.Project,
                x.source,
                x.country_code,
                x.CategoryName,
                x.site_lang,
            }).Select(x => new SubscriptionRevenue
            {
                Project = x.Key.Project,
                source = x.Key.source,
                country_code = x.Key.country_code,
                CategoryName = x.Key.CategoryName,
                site_lang = x.Key.site_lang,
                usd_amount = x.Sum(x => x.usd_amount),
                user_id = x.Count(),
            }).ToList();


            var ltv_report_source_ = ltv_report.GroupBy(x => new
            {
                x.Project,
                x.source,
                x.country_code,
                x.site_lang,
            }).Select(x => new SubscriptionRevenue
            {
                Project = x.Key.Project,
                source = x.Key.source,
                country_code = x.Key.country_code,
                site_lang = x.Key.site_lang,
                usd_amount = x.Sum(x => x.usd_amount),
                user_id = x.Count(),
            }).ToList();


            var ltv_report_country_ = ltv_report.GroupBy(x => new
            {
                x.Project,
                x.country_code,
                x.site_lang,
            }).Select(x => new SubscriptionRevenue
            {
                Project = x.Key.Project,
                country_code = x.Key.country_code,
                site_lang = x.Key.site_lang,
                usd_amount = x.Sum(x => x.usd_amount),
                user_id = x.Count(),
            }).ToList();

            List<LTVReportCategory> ltv_report_category = new List<LTVReportCategory>();
            foreach (var item in ltv_report_category_)
            {
                LTVReportCategory lTVReportCategory = new LTVReportCategory();
                lTVReportCategory.CategoryName = item.CategoryName;
                lTVReportCategory.country_code = item.country_code;
                lTVReportCategory.Project = item.Project;
                lTVReportCategory.source = item.source;
                lTVReportCategory.site_lang = item.site_lang;
                lTVReportCategory.category_count = item.user_id;
                lTVReportCategory.usd_amount_category = item.usd_amount;
                lTVReportCategory.category_LTV = lTVReportCategory.usd_amount_category / lTVReportCategory.category_count;
                ltv_report_category.Add(lTVReportCategory);
            }

            List<LTVReportSource> ltv_report_source = new List<LTVReportSource>();
            foreach (var item in ltv_report_source_)
            {
                LTVReportSource lTVReportSource = new LTVReportSource();
                lTVReportSource.country_code = item.country_code;
                lTVReportSource.Project = item.Project;
                lTVReportSource.source = item.source;
                lTVReportSource.site_lang = item.site_lang;
                lTVReportSource.source_count = item.user_id;
                lTVReportSource.usd_amount_source = item.usd_amount;
                lTVReportSource.source_LTV = lTVReportSource.usd_amount_source / lTVReportSource.source_count;
                ltv_report_source.Add(lTVReportSource);
            }

            List<LTVReportCountry> ltv_report_country = new List<LTVReportCountry>();
            foreach (var item in ltv_report_country_)
            {
                LTVReportCountry lTVReportCountry = new LTVReportCountry();
                lTVReportCountry.country_code = item.country_code;
                lTVReportCountry.Project = item.Project;
                lTVReportCountry.site_lang = item.site_lang;
                lTVReportCountry.country_count = item.user_id;
                lTVReportCountry.usd_amount_country = item.usd_amount;
                lTVReportCountry.country_LTV = lTVReportCountry.usd_amount_country / lTVReportCountry.country_count;
                ltv_report_country.Add(lTVReportCountry);
            }

            //var lookupSource = ltv_report_source.ToLookup(p => p.Project , p => p.site_lang);

            List<LTVReportCategoryMerge> LTVReportCategoryMerge = new List<LTVReportCategoryMerge>();

            var lookupSource = GetLtvLookupSource(ltv_report_source);

            var lookupCountry = GetLtvLookupCountry(ltv_report_country);


            foreach (var item in ltv_report_category)
            {
                var source = lookupSource.Result[item.Project, item.source, item.country_code, item.site_lang].FirstOrDefault();
                var country = lookupCountry.Result[item.Project, item.country_code, item.site_lang].FirstOrDefault();


                LTVReportCategoryMerge lTVReportCategoryMerges = new LTVReportCategoryMerge();
                lTVReportCategoryMerges.Project = item.Project;
                lTVReportCategoryMerges.source = item.source;
                lTVReportCategoryMerges.country_code = item.country_code;
                lTVReportCategoryMerges.site_lang = item.site_lang;
                lTVReportCategoryMerges.category_count = item.category_count;
                lTVReportCategoryMerges.usd_amount_category = item.usd_amount_category;
                lTVReportCategoryMerges.category_LTV = item.category_LTV;
                lTVReportCategoryMerges.source_count = source.source_count;
                lTVReportCategoryMerges.usd_amount_source = source.usd_amount_source;
                lTVReportCategoryMerges.source_LTV = source.source_LTV;
                lTVReportCategoryMerges.country_count = country.country_count;
                lTVReportCategoryMerges.usd_amount_country = country.usd_amount_country;
                lTVReportCategoryMerges.country_LTV = country.country_LTV;
                LTVReportCategoryMerge.Add(lTVReportCategoryMerges);



            }

            LTVReportCategoryMerge = await LTVDecider(LTVReportCategoryMerge);


            LTVReportCategoryMerge = LTVReportCategoryMerge.Where(x => x.source == "Facebook").OrderBy(x=>x.LTV).ToList();


            var AllArifCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Data-Arif-Dont-Touch.csv");

            var ltvOther = await ParseDataArifDontTouch(AllArifCSV);


            LTVReportCategoryMerge = LTVReportCategoryMerge.Concat(ltvOther).ToList();

            await CsvWriter(LTVReportCategoryMerge, "ltv_report_category_merge");



        }

        public static async Task LtvReportCategoryMergeTest2(List<SubscriptionRevenue> list)
        {
            var ltv_report = list;

            ltv_report = ltv_report.Where(x => x.model == "SameMonth").ToList();

            var ltv_report_category_ = ltv_report.GroupBy(x => new
            {
                x.created_date,
                x.Project,
                x.source,
                x.country_code,
                x.CategoryName,
                x.site_lang,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                Project = x.Key.Project,
                source = x.Key.source,
                country_code = x.Key.country_code,
                CategoryName = x.Key.CategoryName,
                site_lang = x.Key.site_lang,
                usd_amount = x.Sum(x => x.usd_amount),
                user_id = x.Count(),
            }).ToList();


            var ltv_report_source_ = ltv_report.GroupBy(x => new
            {
                x.created_date,
                x.Project,
                x.source,
                x.country_code,
                x.site_lang,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                Project = x.Key.Project,
                source = x.Key.source,
                country_code = x.Key.country_code,
                site_lang = x.Key.site_lang,
                usd_amount = x.Sum(x => x.usd_amount),
                user_id = x.Count(),
            }).ToList();


            var ltv_report_country_ = ltv_report.GroupBy(x => new
            {
                x.created_date,
                x.Project,
                x.country_code,
                x.site_lang,
            }).Select(x => new SubscriptionRevenue
            {
                created_date = x.Key.created_date,
                Project = x.Key.Project,
                country_code = x.Key.country_code,
                site_lang = x.Key.site_lang,
                usd_amount = x.Sum(x => x.usd_amount),
                user_id = x.Count(),
            }).ToList();

            List<LTVReportCategory> ltv_report_category = new List<LTVReportCategory>();
            foreach (var item in ltv_report_category_)
            {
                LTVReportCategory lTVReportCategory = new LTVReportCategory();
                lTVReportCategory.created_date = item.created_date;
                lTVReportCategory.CategoryName = item.CategoryName;
                lTVReportCategory.country_code = item.country_code;
                lTVReportCategory.Project = item.Project;
                lTVReportCategory.source = item.source;
                lTVReportCategory.site_lang = item.site_lang;
                lTVReportCategory.category_count = item.user_id;
                lTVReportCategory.usd_amount_category = item.usd_amount;
                lTVReportCategory.category_LTV = lTVReportCategory.usd_amount_category / lTVReportCategory.category_count;
                ltv_report_category.Add(lTVReportCategory);
            }

            List<LTVReportSource> ltv_report_source = new List<LTVReportSource>();
            foreach (var item in ltv_report_source_)
            {
                LTVReportSource lTVReportSource = new LTVReportSource();
                lTVReportSource.created_date = item.created_date;
                lTVReportSource.country_code = item.country_code;
                lTVReportSource.Project = item.Project;
                lTVReportSource.source = item.source;
                lTVReportSource.site_lang = item.site_lang;
                lTVReportSource.source_count = item.user_id;
                lTVReportSource.usd_amount_source = item.usd_amount;
                lTVReportSource.source_LTV = lTVReportSource.usd_amount_source / lTVReportSource.source_count;
                ltv_report_source.Add(lTVReportSource);
            }

            List<LTVReportCountry> ltv_report_country = new List<LTVReportCountry>();
            foreach (var item in ltv_report_country_)
            {
                LTVReportCountry lTVReportCountry = new LTVReportCountry();
                lTVReportCountry.created_date = item.created_date;
                lTVReportCountry.country_code = item.country_code;
                lTVReportCountry.Project = item.Project;
                lTVReportCountry.site_lang = item.site_lang;
                lTVReportCountry.country_count = item.user_id;
                lTVReportCountry.usd_amount_country = item.usd_amount;
                lTVReportCountry.country_LTV = lTVReportCountry.usd_amount_country / lTVReportCountry.country_count;
                ltv_report_country.Add(lTVReportCountry);
            }


            List<LTVReportCategoryMerge> LTVReportCategoryMerge = new List<LTVReportCategoryMerge>();

            var lookupSource = GetLtvLookupSourceTest2(ltv_report_source);

            var lookupCountry = GetLtvLookupCountryTest2(ltv_report_country);


            foreach (var item in ltv_report_category)
            {
                var source = lookupSource.Result[item.created_date, item.Project, item.source, item.country_code, item.site_lang].FirstOrDefault();
                var country = lookupCountry.Result[item.created_date, item.Project, item.country_code, item.site_lang].FirstOrDefault();


                LTVReportCategoryMerge lTVReportCategoryMerges = new LTVReportCategoryMerge();
                lTVReportCategoryMerges.created_date = item.created_date;
                lTVReportCategoryMerges.Project = item.Project;
                lTVReportCategoryMerges.source = item.source;
                lTVReportCategoryMerges.country_code = item.country_code;
                lTVReportCategoryMerges.site_lang = item.site_lang;
                lTVReportCategoryMerges.category_count = item.category_count;
                lTVReportCategoryMerges.usd_amount_category = item.usd_amount_category;
                lTVReportCategoryMerges.category_LTV = item.category_LTV;
                lTVReportCategoryMerges.source_count = source.source_count;
                lTVReportCategoryMerges.usd_amount_source = source.usd_amount_source;
                lTVReportCategoryMerges.source_LTV = source.source_LTV;
                lTVReportCategoryMerges.country_count = country.country_count;
                lTVReportCategoryMerges.usd_amount_country = country.usd_amount_country;
                lTVReportCategoryMerges.country_LTV = country.country_LTV;
                LTVReportCategoryMerge.Add(lTVReportCategoryMerges);



            }

            LTVReportCategoryMerge = await LTVDecider(LTVReportCategoryMerge);


            LTVReportCategoryMerge = LTVReportCategoryMerge.Where(x => x.source == "Facebook").OrderBy(x => x.LTV).ToList();


            var AllArifCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Data-Arif-Dont-Touch.csv");

            var ltvOther = await ParseDataArifDontTouch(AllArifCSV);


            LTVReportCategoryMerge = LTVReportCategoryMerge.Concat(ltvOther).ToList();

            await CsvWriter(LTVReportCategoryMerge, "test_ltv_report");



        }


        public static async Task SaleandComLTV(List<SubscriptionRevenue> list)
        {
            var distributionTeam = list;

            distributionTeam = distributionTeam.Where(x => x.model == "SameMonth").ToList();

            var distributionTeamGroup = distributionTeam.GroupBy(x => new
            {
                x.Project,
                x.created_date,
                x.tpay_activated_date,
                x.period_type,
                x.country_code,
                x.operator_name,
                x.source,
                x.site_lang,
                x.Parked,
                x.CategoryName,
            }).Select(x => new SubscriptionRevenue
            {
                Project = x.Key.Project,
                created_date = x.Key.created_date,
                tpay_activated_date = x.Key.tpay_activated_date,
                period_type = x.Key.period_type,
                country_code = x.Key.country_code,
                operator_name = x.Key.operator_name,
                source = x.Key.source,
                site_lang = x.Key.site_lang,
                Parked = x.Key.Parked,
                CategoryName = x.Key.CategoryName,
                user_id = x.Count(),
                usd_amount = x.Sum(x => x.usd_amount),
            }).ToList();

            distributionTeamGroup = distributionTeamGroup.Where(x => x.Project == "Jawabsale").ToList();
            //distributionTeamGroup = distributionTeamGroup.Where(x => x.Project == "Jawabkom").ToList();

            await CsvWriter(distributionTeamGroup, "SaleandComLTV");
        }


        public static async Task<List<LTVReportCategoryMerge>> LTVDecider(List<LTVReportCategoryMerge> list)
        {
            if (list.Select(x=>x.category_count).Count()>100)
            {
                //return list.Select(x => x.category_LTV).ToList();
                foreach (var item in list)
                {
                    item.LTV = item.category_LTV;
                }
                return list;
            }
            else if (list.Select(x => x.source_count).Count() > 100)
            {
                //return list.Select(x => x.source_LTV).ToList();
                foreach (var item in list)
                {
                    item.LTV = item.source_LTV;
                }
                return list;
            }
            else
            {
                //return list.Select(x => x.country_LTV).ToList();
                foreach (var item in list)
                {
                    item.LTV = item.country_LTV;
                }
                return list;
            }

        }

        public static async Task<LtvLookupSource<string, string, string, string, LTVReportSource>> GetLtvLookupSource(List<LTVReportSource> list)
        {
            //var data = await this.GetAllAsync<LTVReportSource>(nameof(LTVReportSource)).ConfigureAwait(false);
            var lookup = new LtvLookupSource<string, string, string, string, LTVReportSource>(list, item => Tuple.Create(item.Project, item.source, item.country_code, item.site_lang));

            return lookup;
        }

        public static async Task<LtvLookupCountry<string, string, string, LTVReportCountry>> GetLtvLookupCountry(List<LTVReportCountry> list)
        {
            //var data = await this.GetAllAsync<LTVReportSource>(nameof(LTVReportSource)).ConfigureAwait(false);
            var lookup = new LtvLookupCountry<string, string, string, LTVReportCountry>(list, item => Tuple.Create(item.Project, item.country_code, item.site_lang));

            return lookup;
        }

        public static async Task<LtvLookupSourceTest2<string, string, string, string, string, LTVReportSource>> GetLtvLookupSourceTest2(List<LTVReportSource> list)
        {
            //var data = await this.GetAllAsync<LTVReportSource>(nameof(LTVReportSource)).ConfigureAwait(false);
            var lookup = new LtvLookupSourceTest2<string, string, string, string, string, LTVReportSource>(list, item => Tuple.Create(item.created_date, item.Project, item.source, item.country_code, item.site_lang));

            return lookup;
        }

        public static async Task<LtvLookupCountryTest2<string, string, string, string, LTVReportCountry>> GetLtvLookupCountryTest2(List<LTVReportCountry> list)
        {
            //var data = await this.GetAllAsync<LTVReportSource>(nameof(LTVReportSource)).ConfigureAwait(false);
            var lookup = new LtvLookupCountryTest2<string, string, string, string, LTVReportCountry>(list, item => Tuple.Create(item.created_date, item.Project, item.country_code, item.site_lang));

            return lookup;
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
            var exportPath = Path.Combine(@"C:\temp\", filePath);
            using (TextWriter writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(list); // where values implements IEnumerable
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

        private static async Task<List<LTVReportCategoryMerge>> ParseDataArifDontTouch(string[] arif)
        {

            CsvFactory.Register<LTVReportCategoryMerge>(builder =>
            {

                builder.Add(a => a.Project).Type(typeof(string)).ColumnName("Project");
                builder.Add(a => a.source).Type(typeof(string)).ColumnName("Source");
                builder.Add(a => a.country_code).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.site_lang).Type(typeof(string)).ColumnName("site_lang");
                builder.Add(a => a.source_LTV).Type(typeof(decimal)).ColumnName("source_LTV");
                builder.Add(a => a.country_LTV).Type(typeof(decimal)).ColumnName("country_LTV");
                builder.Add(a => a.LTV).Type(typeof(decimal)).ColumnName("LTV");
            }, true, ',', arif);

            List<LTVReportCategoryMerge> datas = await CsvFactory.Parse<LTVReportCategoryMerge>();

            return datas;

        }


    }

}
