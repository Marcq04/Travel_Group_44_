using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication5.Areas.TravelGroupManagement.Models;
using WebApplication5.Data;

namespace WebApplication5.Areas.TravelGroupManagement.Controllers
{
    [Area("TravelGroupManagement")]
    [Route("[area]/[controller]/[action]")]
    public class FlightController : Controller
    {
        private readonly AppDbContext _db;

        public FlightController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var flights = await _db.Flights.ToListAsync();
            return View(flights);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var flight = await _db.Flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Flight flight)
        {
            if (ModelState.IsValid)
            {
                _db.Flights.Add(flight);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var flight = await _db.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId, DepartureLocation, DestinationLocation, DepartureDate, ArrivalDate, NumberOfPassengers, Price")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(flight);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FlightExists(flight.FlightId))
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
            return View(flight);
        }

        private async Task<bool> FlightExists(int id)
        {
            return await _db.Flights.AnyAsync(e => e.FlightId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _db.Flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FlightId)
        {
            var flight = _db.Flights.Find(FlightId);
            if (flight != null)
            {
                _db.Flights.Remove(flight);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var flightsQuery = from f in _db.Flights
                               select f;

            bool searchPerformed = !string.IsNullOrEmpty(searchString);

            if (searchPerformed)
            {
                flightsQuery = flightsQuery.Where(f => f.DepartureLocation.Contains(searchString) || f.DestinationLocation.Contains(searchString));
            }

            var flights = await flightsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", flights);
        }

    }
}
