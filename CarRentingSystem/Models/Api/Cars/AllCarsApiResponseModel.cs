using System.Collections.Generic;

namespace CarRentingSystem.Models.Api.Cars
{
    public class AllCarsApiResponseModel
    {
        public int CurrentPage { get; set; } 
        public int TotalCars { get; init; }
        public int CarsPerPage { get; init; }
        public IEnumerable<CarResponseModel> Cars{ get; init; }
    }
}
