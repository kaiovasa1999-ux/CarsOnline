namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Infrastrucutre;
    using CarRentingSystem.Models.Dealer;
    using CarRentingSystem.Services.Dealer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class DealersController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IDealerService dealers;
        public DealersController(CarRentingDbContext data, IDealerService dealers)
        {
            this.data = data;
            this.dealers = dealers;
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

            var userIdAlreadyDealer = this.data
                .Dealers
                .Any(d => d.UserId == userId);

            if (userIdAlreadyDealer)
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
                UserId = userId
            };

            this.data.Dealers.Add(dealerData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(CarsController.All), "Cars");
        }
    }
}