namespace Artillery.Common
{
    public class ValidationConstants
    {
        //Country
        public const int CountryNameMinLength = 4;
        public const int CountryNameMaxLength = 60;
        public const int ArmySizeMinValue = 50000;
        public const int ArmySizeMaxValue = 10000000;

        //Manufacturer
        public const int ManufacturerNameMinLength = 4;
        public const int ManufacturerNameMaxLength = 40;
        public const int FoundedMinLength = 10;
        public const int FoundedMaxLength = 100;

        //Shell
        public const double ShellWeightMinValue = 2;
        public const double ShellWeightMaxValue = 1680;
        public const int ShellCaliberMinLength = 4;
        public const int ShellCaliberMaxLength = 30;

        //Gun
        public const int GunWeightMinValue = 100;
        public const int GunWeightMaxValue = 1350000;
        public const double GunBarrelLengthMinValue = 2;
        public const double GunBarrelLengthMaxValue = 35;
        public const int GunRangeMinValue = 1;
        public const int GunRangeMaxValue = 100000;
        public const int GunTypeEnumMinValue = 0;
        public const int GunTypeEnumMaxValue = 5;
    }
}
