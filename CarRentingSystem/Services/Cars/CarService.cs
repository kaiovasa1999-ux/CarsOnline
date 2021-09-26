using CarRentingSystem.Data;
using CarRentingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarRentingSystem.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly CarRentingDbContext data;
        public CarService(CarRentingDbContext data)
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
                    Category = c.Category.Name,
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
    }
}
