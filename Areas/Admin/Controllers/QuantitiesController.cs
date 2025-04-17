using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;
using LibraryManagement.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("LibraryAPI/[controller]")]
    [ApiController]
    public class QuantitiesController : ControllerBase
    {
        private readonly QuantitiesService _quantitiesService;

        public QuantitiesController(QuantitiesService quantitiesService)
        {
            _quantitiesService = quantitiesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Quantity>>> GetAll()
        {
            var quantities = await _quantitiesService.GetAllAsync();
            return Ok(quantities);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Quantity>> GetById(string id)
        {
            var quantity = await _quantitiesService.GetByIdAsync(id);
            if (quantity == null) return NotFound();
            return Ok(quantity);
        }

        [HttpPost]
        public async Task<ActionResult<Quantity>> Create(QuantityDTO dto)
        {
            try
            {
                var created = await _quantitiesService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, QuantityDTO dto)
        {
            try
            {
                var result = await _quantitiesService.UpdateAsync(id, dto);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _quantitiesService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}