using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CarRentingSystem.Controllers.Api
{
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
        public IEnumerable<Car> GetCars()
        {
            return this.data.Cars.ToList();
        }
        [HttpGet]
        [Route("{id}")]
        public object GetDetails(int id)
        {
            return this.data.Cars.Find(id);
        }
    }
}
