namespace CarRentingSystem.Controllers.Api
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Api.Cars;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;
        public CarsApiController(CarRentingDbContext data)
        {
            this.data = data;
        }
        [HttpGet]
        public ActionResult<AllCarsApiResponseModel> All([FromQuery] AllCarsApiRequestModel query)
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
                CarSorting.CarCategory => carsQueriable.OrderByDescending(c => c.Category.Name),
                _ => carsQueriable.OrderByDescending(b => b.Id)
            };

            var cars = carsQueriable
                .Skip((query.CurrentPage - 1) * query.CarsPerPage)
                .Take(query.CarsPerPage)
                .Select(c => new CarResponseModel
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

            return new AllCarsApiResponseModel
            {
                TotalCars = totalCars,
                CurrentPage = query.CurrentPage,
                CarsPerPage =query.CarsPerPage,
                Cars = cars,
            };
        }
    }
}
