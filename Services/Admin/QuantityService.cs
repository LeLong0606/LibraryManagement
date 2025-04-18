using AutoMapper;
using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;
using LibraryManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryManagement.Services.Admin
{
    public class QuantityService
    {
        private readonly IMongoCollection<Quantity> _quantitiesCollection;
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly IMapper _mapper;

        public QuantityService(
            IOptions<LMDSettings> libraryManagementDatabaseSettings,
            IMapper mapper)
        {
            var mongoClient = new MongoClient(libraryManagementDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(libraryManagementDatabaseSettings.Value.DatabaseName);

            _quantitiesCollection = mongoDatabase.GetCollection<Quantity>(
                libraryManagementDatabaseSettings.Value.Collections.Quantities);
            _booksCollection = mongoDatabase.GetCollection<Book>(
                libraryManagementDatabaseSettings.Value.Collections.Books);

            _mapper = mapper;
        }

        public async Task<List<Quantity>> GetAllAsync()
        {
            return await _quantitiesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Quantity?> GetByIdAsync(string id)
        {
            return await _quantitiesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Quantity> CreateAsync(QuantityDTO dto)
        {
            var bookExists = await _booksCollection
                .Find(b => b.BookName == dto.BookTitle)
                .AnyAsync();

            if (!bookExists)
                throw new InvalidOperationException("No matching book title found. Please try again!");

            var quantity = _mapper.Map<Quantity>(dto);
            await _quantitiesCollection.InsertOneAsync(quantity);
            return quantity;
        }

        public async Task<bool> UpdateAsync(string id, QuantityDTO dto)
        {
            var bookExists = await _booksCollection
                .Find(b => b.BookName == dto.BookTitle)
                .AnyAsync();

            if (!bookExists)
                throw new InvalidOperationException("No matching book title found. Please try again!");

            var existingQuantity = await GetByIdAsync(id);
            if (existingQuantity == null) return false;

            _mapper.Map(dto, existingQuantity);
            var result = await _quantitiesCollection.ReplaceOneAsync(x => x.Id == id, existingQuantity);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _quantitiesCollection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
    }
}