using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;
using LibraryManagement.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("LibraryAPI/[Area]/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoriesService;

        public CategoryController(CategoryService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            var categories = await _categoriesService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Category>> GetById(string id)
        {
            var category = await _categoriesService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(CategoryDTO dto)
        {
            var created = await _categoriesService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, CategoryDTO dto)
        {
            var result = await _categoriesService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoriesService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{categoryName}/Books")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Book>>> GetBooksByCategory(string categoryName)
        {
            var books = await _categoriesService.GetBooksByCategoryAsync(categoryName);
            if (books.Count == 0) return NotFound("No books found in this category");
            return Ok(books);
        }
    }
}
