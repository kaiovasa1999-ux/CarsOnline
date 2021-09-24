namespace CarRentingSystem.Models.Api.Cars
{
    public class CarResponseModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string ImageUrl { get; set; }
        public string Categpry { get; set; }
    }
}
