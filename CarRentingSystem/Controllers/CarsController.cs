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
    using System.Threading.Tasks;

    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IDealerService dealers;
        private readonly CarRentingDbContext data;
        public CarsController(CarRentingDbContext data, ICarService cars, IDealerService dealers)
        {
            this.cars = cars;
            this.dealers = dealers;
            this.data = data;

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
            if (!this.cars.CategoryExsist(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "This car category does't exist in our database");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = cars.GetCategories();
                return View(car);
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
        public IActionResult Delete(int id, DeletFormModel car)
        {
            var dealerId = this.dealers.GetIdByUser(this.User.GetId());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealer");
            }
            if (!this.cars.CategoryExsist(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "This car category does't exist in our database");
            }
            if (!ModelState.IsValid)
            {
                return View(car);
            }
            if (!this.cars.IsByDealer(id, dealerId))
            {
                return BadRequest();
            }

            var carDelte = this.cars.Delete(id);

            return RedirectToAction(nameof(Mine));
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var car = this.data.Cars
                .Where(c => c.Id == id)
                .Select(c => c.Id)
                .FirstOrDefault();

            if (car == 0)
            {
                return NotFound();

            }

            this.cars.Delete(car);
            await this.data.SaveChangesAsync();

            return RedirectToAction(nameof(Mine));
        }
        [Authorize]

        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();
            var userIsDealer = dealers.IsDealer(userId);
            var isAdmin = this.User.IsAdmin();

            if (!userIsDealer && !isAdmin)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }
            var car = this.cars.GetDetails(id);
            if (car.UserId != userId && !isAdmin)
            {
                return BadRequest();
            }

            return View(new CarFormModel
            {
                Brand = car.Brand,
                Model = car.Model,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId,
                Categories = this.cars.GetCategories()
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, CarFormModel car)
        {
            var dealerId = this.dealers.GetIdByUser(this.User.GetId());
            var userIsAdmin = this.User.IsAdmin();
            if (dealerId == 0 && !userIsAdmin)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealer");
            }
            if (!this.cars.CategoryExsist(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "This car category does't exist in our database");
            }
            if (!ModelState.IsValid)
            {
                car.Categories = cars.GetCategories();
                return View(car);
            }
            if (!this.cars.IsByDealer(id, dealerId) && !userIsAdmin)
            {
                return BadRequest();
            }
            this.cars
                .Edit(
                       id,
                       car.Brand,
                       car.Model,
                       car.Year,
                       car.ImageUrl,
                       car.Description,
                       car.CategoryId
                    );

            return RedirectToAction(nameof(All));
        }
    }
}
