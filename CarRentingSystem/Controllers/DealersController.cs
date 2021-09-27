namespace CarRentingSystem.Controllers
{
    using CarRentingSystem.Data;
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
            var userIsAlreadyDealer = this.dealers.IsDealer(this.User.GetId());
            if (userIsAlreadyDealer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }
            dealers.BecomeDealer(dealer.Name,dealer.PhoneNumber);

            return RedirectToAction(nameof(CarsController.Add), "Cars");
        }
    }
}
