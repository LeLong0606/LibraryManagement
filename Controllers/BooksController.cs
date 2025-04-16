using LibraryManagement.DTOs;
using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("LabraryAPI/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService) => _booksService = booksService;

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
            var createdBook =  await _booksService.CreateAsync(newBookDTO);

            return CreatedAtAction(nameof(Get), new {id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookDTO updatedBookDTO)
        {
            var book = await _booksService.GetAsync(id);

            if (id is null)
            {
                return NotFound();
            }

            await _booksService.UpdateAsync(id, updatedBookDTO);

            return NoContent();
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
