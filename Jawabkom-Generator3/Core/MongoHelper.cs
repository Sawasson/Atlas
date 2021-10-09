﻿using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    public static class MongoHelper
    {

        static MongoDbGenericRepository db = new MongoDbGenericRepository("mongodb://marketing:Marketing2019!@157.90.29.241:27017", "NewProject");

        public static async Task<Subscription> GetSubscriptionLastRecord(string Project)
        {
            var filterBuilder = Builders<Subscription>.Filter;
            var filter = filterBuilder.Eq(d => d.Project, Project);
            var row = await db.GetTable<Subscription>(nameof(Subscription)).Find(filter).SortByDescending(c => c.unique_id).FirstOrDefaultAsync();
            return row;
        }
    }
}
