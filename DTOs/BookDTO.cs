using AutoMapper;
using LibraryManagement.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace LibraryManagement.DTOs
{
    public class BookDTO
    {
        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string BookName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Author { get; set; } = null!;
    }

    public class MappingProfileBook: Profile
    {
        public MappingProfileBook()
        {
            CreateMap<BookDTO, Book>();
        }
    }
}
