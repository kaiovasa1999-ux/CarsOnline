namespace CarRentingSystem.Data
{
    public class DatabaseConstants
    {
        public class CarConst
        {
            public const int BrandMaxLength = 25;
            public const int BrandMinLength = 0;
            public const int ModelMaxLength = 30;
            public const int ModelMinLength = 0;
            public const int MinYear = 1900;
            public const int MaxYear = 2021;
            public const int DescriptionMaxLength = 2500;
        }
        public class CategoryConst
        {
            public const int CategoryNameMaxLength = 20;
            public const int CategoryNameMinLength = 0;
        }
        public class DealerConst
        {
            public const int DealerNameMaxLength = 25;
            public const int DealerNameMinLength = 5;
            public const int DealerPhoneNumberMaxLength = 20;
            public const int DealerPhonNumberMinLength = 3;
        }
        public class User
        {
            public const int FullNameMaxLength = 35;
            public const int FullNameMinLength = 3;
            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 3;
        }
    }
}
