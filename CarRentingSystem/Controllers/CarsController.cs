namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Infrastrucutre;
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Dealer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly ICarService cars;
        private readonly IDealerService dealers;
        public CarsController(CarRentingDbContext data, ICarService cars, IDealerService dealers)
        {
            this.data = data;
            this.cars = cars;
            this.dealers = dealers;

        }
        [Authorize]
        public IActionResult Add()
        {
            var userIsDealer = dealers.IsDealer(this.User.GetId());
            if (!userIsDealer)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }
            return View(new CarFormModel
            {
                Categories = this.cars.GetCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car)
        {
            var dealerId = this.dealers.GetIdByUser(this.User.GetId());
            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealer");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = cars.GetCategories();
                return View(car);
            }

            if (!this.cars.CategoryExsist(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "This car category does't exist in our database");
            }

          

            this.cars.Create(car.Brand,
                car.Model, 
                car.Year,
                car.ImageUrl,
                car.Description, 
                car.CategoryId,
                dealerId);

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
        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();
            var userIsDealer = dealers.IsDealer(userId);

            if (!userIsDealer)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }
            var car = this.cars.GetDetails(id);
            if(car.UserId != userId)
            {
                return BadRequest();
            }

            return View(new CarFormModel
            {
                Brand = car.Brand,
                Model = car.Model,
                Description = car.Description,
                ImageUrl =car.ImageUrl,
                CategoryId = car.CategoryId,
                Categories = this.cars.GetCategories()
            });
        }
    }
}
