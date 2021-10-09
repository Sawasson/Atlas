using CsvFramework;

using Jawabkom_Generator3.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3
{
    public class Jawabkom
    {

        public static async Task Execute()

        {
            //await DownloadCsvFile();

            var csv = await System.IO.File.ReadAllLinesAsync(@"C:\temp\users_subscriptions.csv");

            List<SubscriptionRevenue> allSubs = await ParseSubscriptions(csv);
            //await this.repository.InsertMany(allSubs, nameof(SubscriptionRevenue), false);


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
                builder.Add(a => a.Country).Type(typeof(string)).ColumnName("country_code");
                builder.Add(a => a.first_subscription_subject_id).Type(typeof(string)).ColumnName("first_subscription_subject_id");
                builder.Add(a => a.tpay_activated_date).Type(typeof(string)).ColumnName("tpay_activated_date");
                builder.Add(a => a.Language).Type(typeof(string)).ColumnName("ei_language_code");




            }, true, ',', subscriptions);

            List<SubscriptionRevenue> subs = await CsvFactory.Parse(new SubscriptionRevenueParser());

            return subs;

        }

        private static async Task DownloadCsvFile()
        {
            int limit = 5000;
            string url = SubsHelper.jawabcom_url_subs;
            string project = ProjectType.JAWAB.ToString();
            Subscription lastRecord = await MongoHelper.GetSubscriptionLastRecord(project);
            url = $@"{url}?export=1&ftr[limit]={limit}&ftr[id_greater]={lastRecord.unique_id}";
            var csv = FileHelper.ReadAllLinesFromUrl(url, $"./temp/{project}-subs-{lastRecord.unique_id}.csv");
        }
    }
}
