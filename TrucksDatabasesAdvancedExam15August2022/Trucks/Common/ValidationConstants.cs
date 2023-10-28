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
