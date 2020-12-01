using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelebuHubChat.DbContexts;
using TelebuHubChat.Models;

namespace TelebuHubChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultWidgetController : ControllerBase
    {
        private readonly TelebuHubChatContext _context;

        public DefaultWidgetController(TelebuHubChatContext context)
        {
            _context = context;
        }

        // GET: api/DefaultWidget
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Widgets>>> GetWidgets()
        {
            return await _context.Widgets.ToListAsync();
        }

        // GET: api/DefaultWidget/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Widgets>> GetWidgets(int id)
        {
            var widgets = await _context.Widgets.FindAsync(id);

            if (widgets == null)
            {
                return NotFound();
            }

            return widgets;
        }

        // PUT: api/DefaultWidget/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWidgets(int id, Widgets widgets)
        {
            if (id != widgets.Id)
            {
                return BadRequest();
            }

            _context.Entry(widgets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WidgetsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DefaultWidget
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Widgets>> PostWidgets(Widgets widgets)
        {
            _context.Widgets.Add(widgets);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWidgets", new { id = widgets.Id }, widgets);
        }

        // DELETE: api/DefaultWidget/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Widgets>> DeleteWidgets(int id)
        {
            var widgets = await _context.Widgets.FindAsync(id);
            if (widgets == null)
            {
                return NotFound();
            }

            _context.Widgets.Remove(widgets);
            await _context.SaveChangesAsync();

            return widgets;
        }

        private bool WidgetsExists(int id)
        {
            return _context.Widgets.Any(e => e.Id == id);
        }
    }
}
