using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Areas.TravelGroupManagement.Models;
using WebApplication5.Data;

namespace WebApplication5.Areas.TravelGroupManagement.Controllers
{
    [Area("TravelGroupManagement")]
    [Route("[area]/[controller]/[action]")]
    public class CarRentalController : Controller
    {
        private readonly AppDbContext _db;

        public CarRentalController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var cars = await _db.Cars.ToListAsync();
            return View(cars);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.CarRentalId == id);
            if (car == null)
            {
                return NotFound();
            }
            string modelForImagePath = car.Model.ToUpper();
            car.ImagePath = $"/images/{modelForImagePath}.jpg";
            return View(car);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarRental car)
        {
            if (ModelState.IsValid)
            {
                await _db.Cars.AddAsync(car);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarRentalId, Model, Location, PricePerDay")] CarRental car)
        {
            if (id != car.CarRentalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(car);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CarExists(car.CarRentalId))
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
            return View(car);
        }

        private async Task<bool> CarExists(int id)
        {
            return await _db.Cars.AnyAsync(e => e.CarRentalId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.CarRentalId == id);
            if (car == null)
            {
                return NotFound();
            }
            string modelForImagePath = car.Model.ToUpper();
            car.ImagePath = $"/images/{modelForImagePath}.jpg";
            return View(car);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CarRentalId)
        {
            var car = _db.Cars.Find(CarRentalId);
            if (car != null)
            {
                _db.Cars.Remove(car);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var carsQuery = from c in _db.Cars
                            select c;

            bool searchPerformed = !string.IsNullOrEmpty(searchString);

            if (searchPerformed)
            {
                carsQuery = carsQuery.Where(c => c.Model.Contains(searchString) || c.Location.Contains(searchString));
            }

            var cars = await carsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", cars);
        }

    }
}