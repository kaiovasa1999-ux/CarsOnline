namespace CarRentingSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DatabaseConstants.CarConst;
    public class Car
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(BrandMaxLength)]
        public string Brand { get; set; }
        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }
        public string ImageUrl { get; set; }
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }
        [Range(MinYear, MaxYear)]
        public int Year { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; init; }
        public int DealerId { get; init; }
        public Dealer Dealer { get; init; }
    }
}
