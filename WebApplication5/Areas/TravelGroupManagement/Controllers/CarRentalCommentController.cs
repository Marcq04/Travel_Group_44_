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
    public class CarRentalCommentController : Controller
    {
        private readonly AppDbContext _context;

        public CarRentalCommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int carRentalId)
        {
            var comments = await _context.CarRentalComments
                                         .Where(c => c.CarRentalId == carRentalId)
                                         .OrderByDescending(c => c.CreatedDate)
                                         .ToListAsync();
            return Json(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CarRentalComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.Now;
                _context.CarRentalComments.Add(comment);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Comment added successfully." });
            }

            // Log ModelState Errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid comment data.", errors = errors });

        }
    }
}
