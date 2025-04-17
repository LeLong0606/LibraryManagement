using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LibraryManagement.Areas.Admin.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
