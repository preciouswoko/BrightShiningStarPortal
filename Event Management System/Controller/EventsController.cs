using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Event_Management_System.Controller
{
   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        // POST: api/Events/Create
        [Authorize(Roles = "Organizer")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                await _eventService.CreateEventAsync(@event);
                return Ok(@event);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Events/Update/5
        [Authorize(Roles = "Organizer")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest("Event ID mismatch");
            }

            if (ModelState.IsValid)
            {
                await _eventService.UpdateEventAsync(@event);
                return Ok(@event);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Events/Delete/5
        [Authorize(Roles = "Organizer")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteEventAsync(id);
            return Ok();
        }

        // POST: api/Events/Register/5
        [Authorize]
        [HttpPost("Register/{eventId}")]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _eventService.RegisterForEventAsync(eventId, userId);

            if (!success)
            {
                return BadRequest("Unable to register for event.");
            }

            return Ok();
        }
    }

}
