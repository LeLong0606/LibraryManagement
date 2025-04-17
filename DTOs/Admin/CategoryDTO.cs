using AutoMapper;
using LibraryManagement.Areas.Admin.Models;

namespace LibraryManagement.DTOs.Admin
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
