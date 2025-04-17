using AutoMapper;
using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;
using LibraryManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LibraryManagement.Services.Admin
{
    public class CategoriesService
    {
        private IMongoCollection<Category> _categoriesCollection;
        private IMongoCollection<Book> _booksCollection;
        private readonly IMapper _mapper;

        public CategoriesService(IOptions<LMDSettings> libraryManagementDatabaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(libraryManagementDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(libraryManagementDatabaseSettings.Value.DatabaseName);
            _categoriesCollection = mongoDatabase.GetCollection<Category>(libraryManagementDatabaseSettings.Value.Collections.Categories);
            _booksCollection = mongoDatabase.GetCollection<Book>(libraryManagementDatabaseSettings.Value.Collections.Books);
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoriesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(string id)
        {
            return await _categoriesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> CreateAsync(CategoryDTO dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _categoriesCollection.InsertOneAsync(category);
            return category;
        }

        public async Task<bool> UpdateAsync(string id, CategoryDTO dto)
        {
            var updatedCategory = _mapper.Map<Category>(dto);
            updatedCategory.Id = id;

            var result = await _categoriesCollection.ReplaceOneAsync(x => x.Id == id, updatedCategory);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _categoriesCollection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<Book>> GetBooksByCategoryAsync(string categoryName)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Categories" },
                    { "localField", "Category" },
                    { "foreignField", "Name" },
                    { "as", "CategoryInfo" }
                }),
                new BsonDocument("$unwind", "$CategoryInfo"),
                new BsonDocument("$match", new BsonDocument("CategoryInfo.Name", categoryName)),
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 1 },
                    { "Name", 1 },
                    { "Price", 1 },
                    { "Author", 1 },
                    { "Category", "$CategoryInfo.Name" }
                })
            };

            var result = await _booksCollection.Aggregate<Book>(pipeline).ToListAsync();
            return result;
        }
    }
}
