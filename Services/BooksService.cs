using AutoMapper;
using LibraryManagement.Models;
using LibraryManagement.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace LibraryManagement.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly IMapper _mapper;

        public BooksService(IOptions<LMDSettings> libraryManagementDatabaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(libraryManagementDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(libraryManagementDatabaseSettings.Value.DatabaseName);
            _booksCollection = mongoDatabase.GetCollection<Book>(libraryManagementDatabaseSettings.Value.Collections.Books);
            _mapper = mapper;
        }

        public async Task<List<Book>> GetAsync() => await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) => await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Book> CreateAsync (BookDTO newBookDTO)
        {
            var newBookDto = _mapper.Map<Book>(newBookDTO);
            await _booksCollection.InsertOneAsync(newBookDto);
            return newBookDto;
        }

        public async Task UpdateAsync(string id, BookDTO updateBookDTO)
        {
            var updateBookDto = _mapper.Map<Book>(updateBookDTO);
            updateBookDto.Id = id;
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updateBookDto);
        } 

        public async Task RemoveAsync (string id) => await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
