namespace CarRentingSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Data.DatabaseConstants.DealerConst;
    public class Dealer
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(DealerNameMaxLength)]
        [MinLength(DealerNameMinLength)]
        public string Name { get; set; }
        [Required]
        [MaxLength(DealerNameMaxLength)]
        [MinLength(DealerNameMinLength)]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public IEnumerable<Car> Cars { get; init; } = new List<Car>();
    }
}
