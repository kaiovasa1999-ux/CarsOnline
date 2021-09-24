using CarRentingSystem.Models.Home;
using System.Collections.Generic;

namespace CarRentingSystem.Models.Cars
{
    public class IndexViewModel
    {
        public int TotalCars { get; set; }
        public int TotalUsers { get; set; } 
        public int TotalRents { get; set; }
        public int TotalDealers { get; set; }
        public List<CarIndexViewModel> Cars {get;set; }
    }
}
