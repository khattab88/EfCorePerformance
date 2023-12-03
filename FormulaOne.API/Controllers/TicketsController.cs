using FormulaOne.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TicketsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _db.Events
                .Include(e => e.Tickets)
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpPut]
        [Route("UpdateTicketPrices")]
        public async Task<IActionResult> UpdateTicketPrices(int eventId, double amount)
        {
            var mainEvent = await _db.Events
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (mainEvent == null)
            {
                return NotFound();
            }

            foreach (var ticket in mainEvent.Tickets)
            {
                ticket.Price *= amount;
                ticket.UpdatedAt = DateTime.UtcNow;
            }

            mainEvent.Location = mainEvent.Location + " - updated at " + DateTime.UtcNow.Millisecond;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
