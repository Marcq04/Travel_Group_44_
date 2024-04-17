using Microsoft.AspNetCore.Mvc;
using WebApplication5.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication5.Areas.TravelGroupManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Areas.TravelGroupManagement.Controllers
{
    [Area("TravelGroupManagement")]
    [Route("[area]/[controller]/[action]")]
    public class CarRentalBookingsController : Controller
    {
        private readonly AppDbContext _db;

        public CarRentalBookingsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("Index/{carRentalId:int}")]
        public async Task<IActionResult> Index(int? carRentalId)
        {
            var bookingsQuery = _db.CarRentalBookings.AsQueryable();

            if (carRentalId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.CarRentalId == carRentalId.Value);
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.CarRentalId = carRentalId;
            return View(bookings);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _db.CarRentalBookings
                                    .Include(b => b.Car)
                                    .FirstOrDefaultAsync(b => b.CBookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpGet("Create/{carRentalId:int}")]
        public async Task<IActionResult> Create(int carRentalId)
        {
            var carRental = await _db.Cars.FindAsync(carRentalId);

            if (carRental == null)
            {
                return NotFound();
            }
            var booking = new CarRentalBooking
            {
                CarRentalId = carRentalId,
            };
            return View(booking);
        }

        [HttpPost("Create/{carRentalId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarRentalId", "RenterName", "RenterEmail", "RenterStartDate", "RentalEndDate")] CarRentalBooking booking)
        {
            if (ModelState.IsValid)
            {
                await _db.CarRentalBookings.AddAsync(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { carRentalId = booking.CarRentalId });
            }
            ViewBag.CarRentalId = new SelectList(_db.Cars, "CarRentalId", "Model", booking.CarRentalId);
            return View(booking);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _db.CarRentalBookings
                .Include(t => t.Car)
                .FirstOrDefaultAsync(t => t.CBookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Cars = new SelectList(_db.Cars, "CarRentalId", "Name", booking.CarRentalId);
            return View(booking);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CBookingId", "CarRentalId", "RenterName", "RenterEmail", "RenterStartDate", "RenterEndDate")] CarRentalBooking booking)
        {
            if (id != booking.CBookingId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { carRentalId = booking.CarRentalId });
            }
            ViewBag.Cars = new SelectList(_db.Cars, "CarRentalId", "RenterName", booking.CarRentalId);
            return View(booking);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _db.CarRentalBookings
                .Include(t => t.Car)
                .FirstOrDefaultAsync(t => t.CBookingId == id);
            if (booking == null) { return NotFound(); }
            return View(booking);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CBookingId)
        {
            var booking = await _db.CarRentalBookings.FindAsync(CBookingId);
            if (booking != null)
            {
                _db.CarRentalBookings.Remove(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { CarRentalId = CBookingId });
            }
            return NotFound();
        }

        [HttpGet("Search/{carRentalId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int carRentalId, string searchString)
        {
            var bookingsQuery = _db.CarRentalBookings.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookingsQuery = bookingsQuery.Where(t => t.RenterName.Contains(searchString) || t.RenterEmail.Contains(searchString));
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.CarRentalId = carRentalId;
            return View("Index", bookings);
        }

    }
}
