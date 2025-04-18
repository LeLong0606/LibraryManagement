using AutoMapper;
using LibraryManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;

namespace LibraryManagement.Services.Admin
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly IMongoCollection<Category> _categoriesCollection;
        private readonly IMapper _mapper;

        public BookService(IOptions<LMDSettings> libraryManagementDatabaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(libraryManagementDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(libraryManagementDatabaseSettings.Value.DatabaseName);
            _booksCollection = mongoDatabase.GetCollection<Book>(libraryManagementDatabaseSettings.Value.Collections.Books);
            _categoriesCollection = mongoDatabase.GetCollection<Category>(libraryManagementDatabaseSettings.Value.Collections.Categories);
            _mapper = mapper;
        }

        public async Task<List<Book>> GetAsync() => await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) => await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Book> CreateAsync(BookDTO newBookDTO)
        {
            var categoryExists = await _categoriesCollection
                .Find(c => c.Name == newBookDTO.Category)
                .AnyAsync();

            if (!categoryExists)
                throw new Exception("Invalid category please enter another valid category");

            var newBook = _mapper.Map<Book>(newBookDTO);
            await _booksCollection.InsertOneAsync(newBook);
            return newBook;
        }

        public async Task UpdateAsync(string id, BookDTO updateBookDTO)
        {
            var categoryExists = await _categoriesCollection
                .Find(c => c.Name == updateBookDTO.Category)
                .AnyAsync();

            if (!categoryExists)
                throw new Exception("Invalid category please enter another valid category");

            var updateBook = _mapper.Map<Book>(updateBookDTO);
            updateBook.Id = id;
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updateBook);
        }

        public async Task RemoveAsync (string id) => await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
