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
    public class HotelBookingsController : Controller
    {
        private readonly AppDbContext _db;

        public HotelBookingsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("Index/{hotelId:int}")]
        public async Task<IActionResult> Index(int? hotelId)
        {
            var bookingsQuery = _db.HotelBookings.AsQueryable();

            if (hotelId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.HotelId == hotelId.Value);
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.HotelId = hotelId;
            return View(bookings);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _db.HotelBookings
                                    .Include(b => b.Hotel)
                                    .FirstOrDefaultAsync(b => b.HBookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpGet("Create/{hotelId:int}")]
        public async Task<IActionResult> Create(int hotelId)
        {
            var hotel = await _db.Hotels.FindAsync(hotelId);

            if (hotel == null)
            {
                return NotFound();
            }
            var booking = new HotelBooking
            {
                HotelId = hotelId,
            };
            return View(booking);
        }

        [HttpPost("Create/{hotelId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelId", "Guestname", "GuestEmail", "CheckInDate", "CheckOutDate")] HotelBooking booking)
        {
            if (ModelState.IsValid)
            {
                await _db.HotelBookings.AddAsync(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { hotelId = booking.HotelId });
            }
            ViewBag.Hotel = new SelectList(_db.Hotels, "HotelId", "Name", booking.HotelId);
            return View(booking);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _db.HotelBookings
                                    .Include(b => b.Hotel)
                                    .FirstOrDefaultAsync(b => b.HBookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.Hotel = new SelectList(_db.Hotels, "HotelId", "Name", booking.HotelId);
            return View(booking);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HBookingId", "HotelId", "Guestname", "GuestEmail", "CheckInDate", "CheckOutDate")] HotelBooking booking)
        {
            if (id != booking.HBookingId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { hotelId = booking.HotelId });
            }
            ViewBag.Hotel = new SelectList(_db.Hotels, "HotelId", "Name", booking.HotelId);
            return View(booking);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _db.HotelBookings
                                    .Include(b => b.Hotel)
                                    .FirstOrDefaultAsync(b => b.HBookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int HBookingId)
        {
            var booking = await _db.HotelBookings.FindAsync(HBookingId);
            if (booking != null)
            {
                _db.HotelBookings.Remove(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { hotelId = HBookingId });
            }
            return NotFound();
        }

        [HttpGet("Search/{hotelId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int hotelId, string searchString)
        {
            var bookingsQuery = _db.HotelBookings.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookingsQuery = bookingsQuery.Where(b => b.Guestname.Contains(searchString) || b.GuestEmail.Contains(searchString));
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.HotelId = hotelId;
            return View("Index", bookings);
        }
    }
}
