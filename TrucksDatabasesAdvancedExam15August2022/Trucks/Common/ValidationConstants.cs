namespace Trucks.Common
{
    public static class ValidationConstants
    {
        //Truck
        public const int RegistrationNumberLength = 8;
        public const int VinNumber = 17;
        public const int TankCapacityMinValue = 950;
        public const int TankCapacityMaxValue = 1420;
        public const int CargoCapacityMinValue = 5000;
        public const int CargoCapacityMaxValue = 29000;
        public const string RegistrationNumberRegex = @"[A-Z]{2}\d{4}[A-Z]{2}";
        public const int CategoryTypeMinValue = 0;
        public const int CategoryTypeMaxValue = 3;
        public const int MakeTypeMinValue = 0;
        public const int MakeTypeMaxValue = 4;

        //Client
        public const int ClientNameMinLength = 3;
        public const int ClientNameMaxLength = 40;
        public const int ClientNationalityMinLength = 2;
        public const int ClientNationalityMaxLength = 40;

        //Despatcher
        public const int DespatcherNameMinLength = 2;
        public const int DespatcherNameMaxLength = 40;
    }
}
