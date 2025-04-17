using AutoMapper;
using LibraryManagement.Areas.Admin.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace LibraryManagement.DTOs.Admin
{
    public class QuantityDTO
    {
        [BsonElement("Book Name")]
        [JsonPropertyName("Book Name")]
        public string BookTitle { get; set; } = null!;
        public int QuantityAvailable { get; set; }
    }

    public class MappingProfileQuantity : Profile
    {
        public MappingProfileQuantity()
        {
            CreateMap<QuantityDTO, Quantity>();
        }
    }
}
