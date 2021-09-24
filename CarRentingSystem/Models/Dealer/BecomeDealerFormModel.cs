namespace CarRentingSystem.Models.Dealer
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DatabaseConstants.DealerConst;
    public class BecomeDealerFormModel
    {
        [Required]
        [StringLength(DealerNameMaxLength, MinimumLength = DealerNameMinLength, ErrorMessage =
            "Dealer's name must be between {1} and {2} symbols long!")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(DealerPhoneNumberMaxLength, MinimumLength = DealerPhonNumberMinLength, ErrorMessage =
            "Phone number can't be more the {1} and less them {2} symbols long!")]
        public string PhoneNumber { get; set; }
    }
}
