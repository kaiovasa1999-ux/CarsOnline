namespace CarRentingSystem.Services.Cars
{
    using System.Collections.Generic;

    public class CarQueryServiceModel 
    {
        public int CurrentPage { get; set; } 
        public int TotalCars { get; init; }
        public int CarsPerPage { get; init; }
        public IEnumerable<CarServiceModel> Cars { get; init; }
    }
}
