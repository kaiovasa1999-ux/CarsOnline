namespace CarRentingSystem.Services.Cars
{
    using CarRentingSystem.Models;
    using CarRentingSystem.Models.Cars;
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
        CarDetailsServiceModel GetDetails(int id);
        bool CategoryExsist(int categoryId);
        int Create(string brand,
                string model,
                int year,
                string imageUrl,
                string description,
                int categoryId,
                int dealerId);
        bool Edit(int carId,
                string brand,
                string model,
                int year,
                string imageUrl,
                string description,
                int categoryId);
        int Delete(int carId);
        bool IsByDealer(int carId, int dealerId);
        IEnumerable<CarServiceModel> ByUser(string userId);
        IEnumerable<string> AllCarBrands();
        IEnumerable<string> AllCarcategoreis();
        IEnumerable<CarCategoriesServiceModel> GetCategories();
    }
}
