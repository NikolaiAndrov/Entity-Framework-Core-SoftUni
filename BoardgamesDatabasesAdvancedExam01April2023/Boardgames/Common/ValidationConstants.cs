namespace Boardgames.Common
{
    public class ValidationConstants
    {
        //BoardGame
        public const int BoardGameNameMinLength = 10;
        public const int BoardGameNameMaxLength = 20;
        public const double BoardGameRatingMinValue = 1;
        public const double BoardGameRatingMaxValue = 10;
        public const int BoardGameYearPublishedMinValue = 2018;
        public const int BoardGameYearPublishedMaxValue = 2023;

        //Seller
        public const int SellerNameMinLength = 5;
        public const int SellerNameMaxLength = 20;
        public const int SellerAddressMinLength = 2;
        public const int SellerAddressMaxLength = 30;
        public const string SellerWebsiteRegex = @"www\.[A-Za-z\d-]+\.com";

        //Creator
        public const int CreatorFirstNameMinLength = 2;
        public const int CreatorFirstNameMaxLength = 7;
        public const int CreatorLastNameMinLength = 2;
        public const int CreatorLastNameMaxLength = 7;

        //ImportBoardgameDto
        public const int BoardGameCategoryTypeMinValue = 0;
        public const int BoardGameCategoryTypeMaxValue = 4;
    }
}
