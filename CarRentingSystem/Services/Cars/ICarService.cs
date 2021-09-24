namespace CarRentingSystem.Services.Cars
{
    using CarRentingSystem.Models;
    using System.Collections.Generic;

    public interface ICarService
    {
     
        CarQueryServiceModel 
            All(
            string brand,
            string searchTherm, 
            CarSorting sorting,
            string carCategory,
            int carsPerPage,
            int currentPage);

        IEnumerable<string> AllCarBrands();
        IEnumerable<string> AllCarcategoreis();
    }
}
