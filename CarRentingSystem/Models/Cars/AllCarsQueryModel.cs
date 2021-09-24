namespace CarRentingSystem.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 3;
        public string CarCategory { get; set; }
        public IEnumerable<string> CarCategories { get; set; }
        public string Brand { get; set; }      
        public IEnumerable<string> Brands { get; set; }
        public string Model { get; set; }
        [Display(Name ="Search by:")]
        public string SearchTherm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public CarSorting Sorting { get; init; }
        public int TotalCars { get; set; }
        public IEnumerable<CarListingViewModel> Cars { get; set; }
    }
}
