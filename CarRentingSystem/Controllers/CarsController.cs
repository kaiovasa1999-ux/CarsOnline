namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Infrastrucutre;
    using CarRentingSystem.Models.Cars;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;
        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }
        [Authorize]
        public IActionResult Add()
        {
            var userIsDealer = UserIsDealer();
            if (!userIsDealer)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealer");
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
            var carsQueriable = this.data.Cars.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query.CarCategory))
            {
                carsQueriable = carsQueriable.Where(c => c.Category.Name == query.CarCategory);
            }

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                carsQueriable = carsQueriable.Where(c => c.Brand == (query.Brand));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTherm))
            {
                carsQueriable = carsQueriable
                    .Where(c => c.Description.ToLower().Contains(query.SearchTherm.ToLower())
                    || (c.Brand + " " + c.Model).ToLower().Contains(query.SearchTherm));
            }
            carsQueriable = query.Sorting switch
            {
                CarSorting.DateAdded => carsQueriable.OrderByDescending(c => c.Id),
                CarSorting.BrandAndModel => carsQueriable.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                CarSorting.Year => carsQueriable.OrderByDescending(c => c.Year),
                CarSorting.CarCategory => carsQueriable.OrderByDescending(c=> c.Category.Name),
                _ => carsQueriable.OrderByDescending(b => b.Id)
            };

            var cars = carsQueriable
                .Skip((query.CurrentPage - 1) * AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
                .Select(c => new CarListingViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    Categpry = c.Category.Name,
                    ImageUrl = c.ImageUrl,

                }).ToList();

            
            var carCategories = this.data.Categories
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            var carBrands = this.data.Cars
                .Select(c => c.Brand)
                .Distinct()
                .ToList();

            var totalCars = carsQueriable.Count();

            query.Cars = cars;
            query.CarCategories = carCategories;
            query.Brands = carBrands;
            query.TotalCars = totalCars;

            return View(query);
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
