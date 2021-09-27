namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Infrastrucutre;
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Services.Cars;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly ICarService cars;
        public CarsController(CarRentingDbContext data, ICarService cars)
        {
            this.data = data;
            this.cars = cars;

        }
        [Authorize]
        public IActionResult Add()
        {
            var userIsDealer = UserIsDealer();
            if (!userIsDealer)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }
            return View(new AddCarFormModel
            {
                Categories = this.GetCarCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car)
        {
            var dealerId = this.data.Dealers
                .Where(d => d.UserId == this.User.GetId())
                .Select(d => d.Id)
                .FirstOrDefault();

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealer");
            }


            if (!this.data.Categories.Any(c => c.Id == car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "This car category does't exist in our database");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = GetCarCategories();
                return View(car);
            }

            var carData = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl,
                Description = car.Description,
                CategoryId = car.CategoryId,
                DealerId = dealerId,
            };

            this.data.Cars.Add(carData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
       
        [HttpGet]
        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = this.cars.All
                (query.Brand,
                query.SearchTherm,
                query.Sorting,
                query.CarCategory,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);


            var carCategories = this.cars.AllCarcategoreis();
            var carBrands = this.cars.AllCarBrands();
            var totalCars = queryResult.TotalCars;

            query.Cars = queryResult.Cars;
            query.CarCategories = carCategories;
            query.Brands = carBrands;
            query.TotalCars = totalCars;

            return View(query);
        }
        public IActionResult Mine()
        {
            var myCars = this.cars.ByUser(this.User.GetId());
            return View(myCars);
        }
        private bool UserIsDealer()
            => this.data.Dealers
                .Any(d => d.UserId == this.User.GetId());
        public IEnumerable<CarCategoryViewModel> GetCarCategories()
        {
            var categoreis = this.data.Categories.Select(c => new CarCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
                 .ToList();
            return categoreis;
        }
    }
}
