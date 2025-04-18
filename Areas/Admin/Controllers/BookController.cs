using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;
using LibraryManagement.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("LibraryAPI/[Area]/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _booksService;

        public BookController(BookService booksService) => _booksService = booksService;

        [HttpGet]
        public async Task<List<Book>> Get() => await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookDTO newBookDTO)
        {
            try
            {
                var createdBook = await _booksService.CreateAsync(newBookDTO);
                return CreatedAtAction(nameof(Get), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookDTO updatedBookDTO)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            try
            {
                await _booksService.UpdateAsync(id, updatedBookDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);

            return NoContent();
        }
    }
}
