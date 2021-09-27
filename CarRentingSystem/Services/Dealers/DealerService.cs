﻿namespace CarRentingSystem.Services.Dealer
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
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
        public int GetIdByUser(string userId)
        {
           return this.data.Dealers
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
        }

        public string BecomeDealer(string dealerName, string phoneNumber)
        {
            var dealerData = new Dealer
            {
                Name = dealerName,
                PhoneNumber = phoneNumber
            };
            this.data.Dealers.Add(dealerData);
            this.data.SaveChanges();

            return dealerData.Name;
        }
    }
}
