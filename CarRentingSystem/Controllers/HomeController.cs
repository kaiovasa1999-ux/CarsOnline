namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IStatisticsService service;
        public HomeController(IStatisticsService service, CarRentingDbContext data)
        {
            this.data = data;
            this.service = service;
        }
        public IActionResult Index()
        {
            var cars = this.data.Cars
               .OrderByDescending(c => c.Id)
               .Select(c => new CarIndexViewModel
               {
                   Id = c.Id,
                   Brand = c.Brand,
                   Model = c.Model,
                   Year = c.Year,
                   ImageUrl = c.ImageUrl,
               })
               .Take(3)
               .ToList();

            var totalStatistics = service.Total();

            return View(new IndexViewModel
            {
                Cars = cars,
                TotalCars = totalStatistics.TotalCars,
                TotalDealers = totalStatistics.TotalDealers,
                TotalUsers = totalStatistics.TotalUsers,
            });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
