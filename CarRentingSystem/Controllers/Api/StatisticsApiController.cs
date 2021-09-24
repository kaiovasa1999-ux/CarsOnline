using CarRentingSystem.Data;
using CarRentingSystem.Data.Models.Api.Statistics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarRentingSystem.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]

    public class StatisticsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;
        public StatisticsApiController(CarRentingDbContext data)
        {
            this.data = data;
        }
        [HttpGet]
        public StatisticsResponeModel GetStatistics()
        {
            var totalCars = this.data.Cars.Count();
            var totalDealers = this.data.Dealers.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsResponeModel
            {
                TotalCars = totalCars,
                TotalDealers = totalDealers,
                TotaUsers = totalUsers,
                TotalRents = 0,
            };
        }
    }
}
