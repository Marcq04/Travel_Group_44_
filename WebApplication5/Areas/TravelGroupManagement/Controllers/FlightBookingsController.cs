using Microsoft.AspNetCore.Mvc;
using WebApplication5.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using WebApplication5.Areas.TravelGroupManagement.Models;

namespace WebApplication5.Areas.TravelGroupManagement.Controllers
{
    [Area("TravelGroupManagement")]
    [Route("[area]/[controller]/[action]")]
    public class FlightBookingsController : Controller
    {
        private readonly AppDbContext _db;

        public FlightBookingsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("Index/{flightId:int}")]
        public async Task<IActionResult> Index(int? flightId)
        {
            var bookingsQuery = _db.FlightBookings.AsQueryable();

            if (flightId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.FlightId == flightId.Value);
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.FlightId = flightId;
            return View(bookings);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var bookingQuery = _db.FlightBookings.AsQueryable();
            var booking = await _db.FlightBookings
                                    .Include(b => b.Flight)
                                    .FirstOrDefaultAsync(b => b.FBookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpGet("Create/{flightId:int}")]
        public async Task<IActionResult> Create(int flightId)
        {
            var flight = await _db.Flights.FindAsync(flightId);

            if (flight == null)
            {
                return NotFound();
            }
            var booking = new FlightBooking
            {
                FlightId = flightId,
            };
            return View(booking);
        }

        [HttpPost("Create/{flightId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId", "PassengerName", "PassengerEmail")] FlightBooking booking)
        {
            if (ModelState.IsValid)
            {
                await _db.FlightBookings.AddAsync(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { flightId = booking.FlightId });
            }
            ViewBag.Flights = new SelectList(_db.Flights, "FlightId", "DepartureLocation", booking.FlightId);
            return View(booking);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _db.FlightBookings
                                    .Include(b => b.Flight)
                                    .FirstOrDefaultAsync(b => b.FBookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Flights = new SelectList(_db.Flights, "FlightId", "DepartureLocation", booking.FlightId);
            return View(booking);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FBookingId", "FlightId", "PassengerName", "PassengerEmail")] FlightBooking booking)
        {
            if (id != booking.FBookingId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { flightId = booking.FlightId });
            }
            ViewBag.Flights = new SelectList(_db.Flights, "FlightId", "DepartureLocation", booking.FlightId);
            return View(booking);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _db.FlightBookings
                                    .Include(b => b.Flight)
                                    .FirstOrDefaultAsync(b => b.FBookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FBookingId)
        {
            var booking = await _db.FlightBookings.FindAsync(FBookingId);
            if (booking != null)
            {
                _db.FlightBookings.Remove(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { flightId = FBookingId });
            }
            return NotFound();
        }

        [HttpGet("Search/{flightId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int flightId, string searchString)
        {
            var bookingsQuery = _db.FlightBookings.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookingsQuery = bookingsQuery.Where(b => b.PassengerName.Contains(searchString) || b.PassengerEmail.Contains(searchString));
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.FlightId = flightId;
            return View("Index", bookings);
        }
    }
}
