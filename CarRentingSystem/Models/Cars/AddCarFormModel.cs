using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Models.Cars
{
    using static Data.DatabaseConstants.CarConst;
    public class AddCarFormModel
    {
        [Required]
        [StringLength(BrandMaxLength, MinimumLength = BrandMinLength, ErrorMessage = "The Brand length msut be between {2} and {1} symbols long!")]

        public string Brand { get; set; }
        [Required]
        [StringLength(ModelMaxLength,MinimumLength = ModelMinLength,ErrorMessage ="The model length msut be between {2} and {1} symbols long!")]
        public string Model { get; set; }
        [Required]
        public string ImageUrl { get; init; }
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; init; }
        [Range(MinYear,MaxYear)]
        public int Year { get; init; }
        public int CategoryId { get; set; }
        public IEnumerable<CarCategoryViewModel> Categories { get; set; }
    }
}
