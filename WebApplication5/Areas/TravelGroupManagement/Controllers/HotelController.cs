using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Areas.TravelGroupManagement.Models;
using WebApplication5.Data;
using WebApplication5.Services;

namespace WebApplication5.Areas.TravelGroupManagement.Controllers
{
    [Area("TravelGroupManagement")]
    [Route("[area]/[controller]/[action]")]
    public class HotelController : Controller
    {

        private readonly AppDbContext _db;
        private readonly ILogger<HotelController> _logger;
        private readonly ISessionService _sessionService;

        public HotelController(AppDbContext db, ILogger<HotelController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Calling hotel index action");
            try
            {
                var hotels = await _db.Hotels.ToListAsync();

                var value = _sessionService.GetSessionData<int?>("Visited") ?? 0;
                _sessionService.SetSessionData("Visited", value + 1);

                ViewBag.mysession = value + 1;

                _logger.LogInformation("Hello");
                return View(hotels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(null);
            }
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Calling hotel index action");
            var hotel = await _db.Hotels.FirstOrDefaultAsync(f => f.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel.ImagePath = $"/images/{hotel.Name.ToUpper()}.jpg";
            return View(hotel);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                await _db.Hotels.AddAsync(hotel);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _db.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId, Name, Location, CheckInDate, CheckOutDate, NumberOfGuests, Rating, PricePerNight")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(hotel);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await HotelExists(hotel.HotelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        private async Task<bool> HotelExists(int id)
        {
            return _db.Hotels.Any(e => e.HotelId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _db.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel.ImagePath = $"/images/{hotel.Name.ToUpper()}.jpg";
            return View(hotel);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int HotelId)
        {
            var hotel = _db.Hotels.Find(HotelId);
            if (hotel != null)
            {
                _db.Hotels.Remove(hotel);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var hotelsQuery = from h in _db.Hotels
                              select h;

            bool searchPerformed = !string.IsNullOrEmpty(searchString);

            if (searchPerformed)
            {
                hotelsQuery = hotelsQuery.Where(h => h.Name.Contains(searchString) || h.Location.Contains(searchString));
            }

            var hotels = await hotelsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", hotels);
        }
    }
}
