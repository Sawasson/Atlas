using MongoDB.Driver;

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

        public static async Task AddMany<T>(List<T> data, string tableName)
        {
            await db.AddManyAsync<T>(data,tableName);
        }

        public static async Task<IEnumerable<Currency>> GetAll()
        {
            var list = await db.GetAllAsync<Currency>();
            return list;
        }

        public static async Task<IEnumerable<T>> GetAll<T>(string tableName)
        {
            var list = await db.GetAllAsync<T>(tableName);
            return list;
        }

        public static void DropTable(string tableName)
        {
            db.DropTable(tableName);
        }

    }
}
