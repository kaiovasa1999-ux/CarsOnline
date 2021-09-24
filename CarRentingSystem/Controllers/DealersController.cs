﻿namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Infrastrucutre;
    using CarRentingSystem.Models.Dealer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    public class DealersController : Controller
    {
        private readonly CarRentingDbContext data;
        public DealersController(CarRentingDbContext data)
        {
            this.data = data;
        }
        [Authorize]
        public IActionResult Become()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = this.User.GetId();
            var userIsAlreadyDealer = this.data.Dealers.Any(d => d.UserId == userId);
            if (userIsAlreadyDealer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }
            var dealerData = new Dealer
            {
                Name = dealer.Name,
                PhoneNumber = dealer.PhoneNumber,
                UserId = userId,
            };

            this.data.Dealers.Add(dealerData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(CarsController.Add), "Cars");
        }
    }
}
