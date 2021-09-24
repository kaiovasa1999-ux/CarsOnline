namespace CarRentingSystem.Models.Api.Cars
{
    using CarRentingSystem.Models.Cars;
    using System.Collections.Generic;
    public class AllCarsApiRequestModel
    {
        public string CarCategory { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SearchTherm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CarsPerPage { get; init; } = 10;
        public CarSorting Sorting { get; init; }
    }
}
