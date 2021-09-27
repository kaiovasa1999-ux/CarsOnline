﻿namespace CarRentingSystem.Services.Dealer
{
    using CarRentingSystem.Data;
    using System.Linq;
    public class DealerService : IDealerService
    {
        private readonly CarRentingDbContext data;
        public DealerService(CarRentingDbContext data)
        {
            this.data = data;
        }


        bool IDealerService.IsDealer(string userId)
        {
            return this.data.Dealers.Any(d => d.UserId == userId);
        }
    }
}