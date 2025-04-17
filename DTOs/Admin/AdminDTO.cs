using AutoMapper;
using LibraryManagement.Areas.Admin.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace LibraryManagement.DTOs.Admin
{
    public class AdminDTO
    {
        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string AdminName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [DefaultValue("Admin")]
        public string Role { get; set; } = "Admin";
    }

    public class MappingProfileAdmin: Profile
    {
        public MappingProfileAdmin()
        {
            CreateMap<AdminDTO, AdminModel>();
        }
    }
}
