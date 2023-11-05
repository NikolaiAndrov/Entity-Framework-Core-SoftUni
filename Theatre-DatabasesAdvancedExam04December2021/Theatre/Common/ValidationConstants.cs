namespace Theatre.Common
{
    public class ValidationConstants
    {
        //Theatre
        public const byte TheatreNameMinLength = 4;
        public const byte TheatreNameMaxLength = 30;
        public const sbyte TheatreNumberOfHallsMinValue = 1;
        public const sbyte TheatreNumberOfHallsMaxValue = 10;
        public const byte TheatreDirectorMinLength = 4;
        public const byte TheatreDirectorMaxLength = 30;

        //Play
        public const byte PlayTitleMinLength = 4;
        public const byte PlayTitleMaxLength = 50;
        public const double PlayRatingMinValue = 0;
        public const double PlayRatingMaxValue = 10;
        public const short PlayDescriptionMaxLength = 700;
        public const byte PlayScreenwriterMinLength = 4;
        public const byte PlayScreenwriterMaxLength = 30;

        //Cast
        public const short CastFullNameMinLength = 4;
        public const short CastFullNameMaxLength = 30;
        public const short CastPhoneNumberLength = 15;
        public const string CastPhoneNumberRegex = @"^\+44-\d{2}-\d{3}-\d{4}$";

        //Ticket
        public const string TicketPriceMinValue = "1.00";
        public const string TicketPriceMaxValue = "100.00";
        public const byte TicketRowNumberMinValue = 1;
        public const byte TicketRowNumberMaxValue = 10;
    }
}
