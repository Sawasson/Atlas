using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB
{
    public class MongoDbGenericRepository
    {
        private readonly IMongoDatabase database = null;
        private readonly IMongoClient client = null;

        public MongoDbGenericRepository(string cnnString, string database)
        {
            client = new MongoClient(cnnString);
            this.database = this.client.GetDatabase(database);
        }


        public IMongoCollection<T> GetQuery<T>()
        {
            return database.GetCollection<T>(typeof(T).Name);
        }

        //public async Task BulkUpdateAsync<T>(string name, List<UpdateOneModel<T>> list,bool Is)
        //{
        //    this.GetQuery<T>(name).BulkWriteAsync(list,)
        //}

        public IMongoCollection<T> GetTable<T>(string tableName)
        {
            return database.GetCollection<T>(tableName);
        }

        public IMongoCollection<T> GetQuery<T>(string tableName, string database)
        {
            var db = this.client.GetDatabase(database);
            return db.GetCollection<T>(tableName);
        }


        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.GetQuery<T>().Find(_ => true).ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter)
        {
            return await this.GetQuery<T>().Find(filter).ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName)
        {
            return await this.GetTable<T>(tableName).Find(_ => true).ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter, string tableName)
        {
            return await this.GetTable<T>(tableName).Find(filter).ToListAsync();

        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filter)
        {
            return await this.GetQuery<T>().Find(filter).SingleOrDefaultAsync();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> filter, string tableName)
        {
            return await this.GetTable<T>(tableName).Find(filter).SingleOrDefaultAsync();
        }

        //public async Task<T> GetAsync<T>(Guid id) where T : IIFSystemTable
        //{

        //    return await this.GetQuery<T>().Find(log => log.UniqueId == id).SingleOrDefaultAsync();

        //}

        public async Task AddAsync<T>(T item)
        {
            await this.GetQuery<T>().InsertOneAsync(item);
        }

        public async Task AddAsync<T>(T item, string tableName)
        {
            await this.GetTable<T>(tableName).InsertOneAsync(item).ConfigureAwait(false);
        }

        public async Task AddManyAsync<T>(List<T> item)
        {
            await this.GetQuery<T>().InsertManyAsync(item).ConfigureAwait(false);
        }

        public async Task AddManyAsync<T>(List<T> item, string tableName)
        {
            await this.GetTable<T>(tableName).InsertManyAsync(item).ConfigureAwait(false);
        }

        public void Add<T>(T item)
        {
            this.GetQuery<T>().InsertOne(item);
        }

        public void Add<T>(T item, string tableName)
        {
            this.GetTable<T>(tableName).InsertOne(item);
        }

        public async Task DropDatabaseAsync(string dbName)
        {
            await this.client.DropDatabaseAsync(dbName);
        }

        public async Task DropManyAsync<T>(string tableName)
        {
            await this.GetTable<T>(tableName).DeleteManyAsync(tableName).ConfigureAwait(false);
        }

        public void DropTable(string tableName)
        {
            database.DropCollection(tableName);
        }
    }
}
