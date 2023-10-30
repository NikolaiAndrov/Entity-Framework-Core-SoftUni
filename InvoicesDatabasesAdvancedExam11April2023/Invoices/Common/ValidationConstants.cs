namespace Invoices.Common
{
    public class ValidationConstants
    {
        //Product
        public const int ProductNameMinLength = 9;
        public const int ProductNameMaxLength = 30;
        public const decimal ProductPriceMinValue = 5;
        public const decimal ProductPriceMaxValue = 1000;

        //Address
        public const int StreetNameMinLength = 10;
        public const int StreetNameMaxLength = 20;
        public const int CityNameMinLength = 5;
        public const int CityNameMaxLength = 15;
        public const int CountryNameMinLength = 5;
        public const int CountryNameMaxLength = 15;

        //Invoices
        public const int InvoiceNumberMinValue = 1000000000;
        public const int InvoiceNumberMaxValue = 1500000000;

        //Client
        public const int ClientNameMinLength = 10;
        public const int ClientNameMaxLength = 25;
        public const int ClientNumberVatMinLength = 10;
        public const int ClientNumberVatMaxLength = 15;
    }
}
