using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Services.Dealer;
using System.Collections.Generic;
using System.Linq;

namespace CarRentingSystem.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly CarRentingDbContext data;
        public CarService(CarRentingDbContext data, IDealerService dealers)
        {
            this.data = data;
        }

        public CarQueryServiceModel
            All(string brand,
            string searchTherm, 
            CarSorting sorting,
            string carCategory,
            int currentPage,
            int carsPerPage) 

        {
            var carsQueriable = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(carCategory))
            {
                carsQueriable = carsQueriable.Where(c => c.Category.Name == carCategory);
            }

            if (!string.IsNullOrWhiteSpace(brand))
            {
                carsQueriable = carsQueriable.Where(c => c.Brand == (brand));
            }

            if (!string.IsNullOrWhiteSpace(searchTherm))
            {
                carsQueriable = carsQueriable
                    .Where(c => c.Description.ToLower().Contains(searchTherm.ToLower())
                    || (c.Brand + " " + c.Model).ToLower().Contains(searchTherm));
            }
            carsQueriable = sorting switch
            {
                CarSorting.DateAdded => carsQueriable.OrderByDescending(c => c.Id),
                CarSorting.BrandAndModel => carsQueriable.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                CarSorting.Year => carsQueriable.OrderByDescending(c => c.Year),
                CarSorting.CarCategory => carsQueriable.OrderByDescending(c => c.Category.Name),
                _ => carsQueriable.OrderByDescending(b => b.Id)
            };

            var totalCars = carsQueriable.Count();

            var cars = carsQueriable
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage)
                .Select(c => new CarServiceModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    CategoryName = c.Category.Name,
                    ImageUrl = c.ImageUrl,

                }).ToList();

            return new CarQueryServiceModel
            {
                CarsPerPage = carsPerPage,
                Cars = cars,
                TotalCars = totalCars,
                CurrentPage = currentPage,
            };
        }
        public CarDetailsServiceModel GetDetails(int id)
        {
            var car = this.data.Cars.Where(c => c.Id == id)
                .Select(c => new CarDetailsServiceModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    Description = c.Description,
                    DealerId = c.DealerId,
                    DealerName = c.Dealer.Name,
                    UserId = c.Dealer.UserId,
                    CategoryName = c.Category.Name,
                    ImageUrl = c.ImageUrl,
                })
                .FirstOrDefault();

            return car;
        }
        public int Create(string brand,
            string model, 
            int year,
            string imageUrl,
            string description, 
            int categoryId, 
            int dealerId)
        {
            var carData = new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                ImageUrl = imageUrl,
                Description = description,
                CategoryId = categoryId,
                DealerId = dealerId,
            };

            this.data.Cars.Add(carData);
            this.data.SaveChanges();

            return carData.Id;
        }
        public int Delete(int carId)
        {
            var carForDelete = this.data.Cars.First(c => c.Id == carId);
            if(carForDelete != null)
            {
                this.data.Cars.Remove(carForDelete);
                this.data.SaveChanges();
            }
            return carId;
        }

        public bool Edit(int carId, string brand, string model, int year, string imageUrl, string description, int categoryId )
        {
            var carData = this.data.Cars.Find(carId);
            if(carData == null)
            {
                return false;
            }

            carData.Brand = brand;
            carData.Model = model;
            carData.Year = year;
            carData.ImageUrl = imageUrl;
            carData.Description = description;
            carData.CategoryId = categoryId;

            this.data.SaveChanges();
            return true;
        }
        public IEnumerable<CarServiceModel> ByUser(string userId)
        {
           return GetCars(this.data.Cars.Where(c => c.Dealer.UserId == userId));
        }
        public bool IsByDealer(int carId, int dealerId)
        {
            return this.data.Cars.Any(c => c.Id == carId && c.DealerId == dealerId);
        }
        public IEnumerable<string> AllCarBrands()
        {
            return this.data
               .Cars
               .Select(c => c.Brand)
               .Distinct()
               .ToList();
        }
        public IEnumerable<string> AllCarcategoreis()
        {
            return this.data.Categories
              .Select(c => c.Name)
              .Distinct()
              .OrderBy(c => c)
              .ToList();
        }
        public IEnumerable<CarCategoriesServiceModel> GetCategories()
        {
            var categoreis = this.data.Categories.Select(c => new CarCategoriesServiceModel
            {
                Id = c.Id,
                Name = c.Name,
            })
                .ToList();
            return categoreis;
        }
        public bool CategoryExsist(int categoryId)
        {
           return this.data.Categories.Any(c => c.Id == categoryId);
        }
        public static IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carsQuery)
        {
            return carsQuery.Select(c => new CarServiceModel
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                CategoryName = c.Category.Name,
                ImageUrl = c.ImageUrl,
            })
                .ToList();
        }
    }
}
