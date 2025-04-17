using AutoMapper;
using LibraryManagement.Models;

namespace LibraryManagement.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class MappingProfileCategory : Profile
    {
        public MappingProfileCategory()
        {
            CreateMap<CategoryDTO, Category>();
        }
    }
}
