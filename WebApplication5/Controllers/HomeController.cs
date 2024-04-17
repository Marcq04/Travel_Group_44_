using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GeneralSearch(string searchType, string searchString)
        {
            if (searchType == "Flights")
            {
                return RedirectToAction("Search", "Flight", new { area = "TravelGroupManagement", searchString });
            }
            else if (searchType == "Hotels")
            {
                return RedirectToAction("Search", "Hotel", new { area = "TravelGroupManagement", searchString });
            }
            else if (searchType == "Cars")
            {
                return RedirectToAction("Search", "CarRental", new { area = "TravelGroupManagement", searchString });
            }
            else if (searchString == "FlightBookings")
            {
                var url = Url.Action("Search", "FlightBookings", new { area = "TravelGroupManagement" }) + $"?searchString={searchString}";
                return Redirect(url);
            }
            else if (searchString == "HotelBookings")
            {
                var url = Url.Action("Search", "HotelBookings", new { area = "TravelGroupManagement" }) + $"?searchString={searchString}";
                return Redirect(url);
            }
            else if (searchString == "CarRentalBookings")
            {
                var url = Url.Action("Search", "CarRentalBookings", new { area = "TravelGroupManagement" }) + $"?searchString={searchString}";
                return Redirect(url);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotFound(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }

            return View("Error");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
