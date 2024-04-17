using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Areas.TravelGroupManagement.Models;
using WebApplication5.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Areas.TravelGroupManagement.Controllers
{
    [Area("TravelGroupManagement")]
    [Route("[area]/[controller]/[action]")]
    public class FlightCommentController : Controller
    {
        private readonly AppDbContext _context;

        public FlightCommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int flightId)
        {
            var comments = await _context.FlightComments
                                         .Where(c => c.FlightId == flightId)
                                         .OrderByDescending(c => c.CreatedDate)
                                         .Select(c => new { Content = c.Content })
                                         .ToListAsync();
            return Json(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] FlightComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.Now;
                _context.FlightComments.Add(comment);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Comment added successfully." });
            }

            // Log ModelState Errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid comment data.", errors = errors });

        }
    }
}
