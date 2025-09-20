using CosmosMongoCrud.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CosmosMongoCrud.Repository
{
    public class ItemRepository
    {
        private readonly IMongoCollection<Item> _items;

        public ItemRepository(IOptions<CosmosMongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _items = database.GetCollection<Item>(settings.Value.CollectionName);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            var filter = Builders<Item>.Filter.Empty;
            List<Item> allData = await _items.Find(filter).ToListAsync();
            return allData;
        }

        public async Task<Item> GetAsync(string id)
        {
            var filter = Builders<Item>.Filter.Eq("Id", id);
            return await _items.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item item)
        {
            await _items.InsertOneAsync(item);
        }

        public async Task UpdateAsync(string id, Item updatedItem)
        {
            var filter = Builders<Item>.Filter.Eq("Id", id);
            await _items.ReplaceOneAsync(filter, updatedItem);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Item>.Filter.Eq("Id", id);
            await _items.DeleteOneAsync(filter);
        }

    }

}
