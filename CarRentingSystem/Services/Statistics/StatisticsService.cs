namespace CarRentingSystem.Services.Statistics
{
    using CarRentingSystem.Data;
    using System.Linq;
    public class StatisticsService : IStatisticsService
    {
        private readonly CarRentingDbContext data;
        public StatisticsService(CarRentingDbContext data)
        {
            this.data = data;
        }
        public StatisticsServiceModel Total()
        {
            var totalCars = this.data.Cars.Count();
            var totalDealers = this.data.Dealers.Count();
            var totalUsers = this.data.Users.Count();
            return new StatisticsServiceModel
            {
                TotalCars = totalCars,
                TotalDealers = totalDealers,
                TotalUsers = totalUsers,
            };
        }
    }
}
