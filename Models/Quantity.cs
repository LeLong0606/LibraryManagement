using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace LibraryManagement.Models
{
    public class Quantity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        [BsonElement("Book Name")]
        [JsonPropertyName("Book Name")]
        public string BookTitle { get; set; } = null!;
        public int QuantityAvailable { get; set; }
    }
}
