namespace Footballers.Common
{
    public class ValidationConstants
    {
        //Footballer
        public const int FootballerNameMinLength = 2;
        public const int FootballerNameMaxLength = 40;
        public const int BestSkillTypeMinValue = 0;
        public const int BestSkillTypeMaxValue = 4;
        public const int PositionTypeMinValue = 0;
        public const int PositionTypeMaxValue = 3;

        //Team
        public const int TeamNameMinLength = 3;
        public const int TeamNameMaxLength = 40;
        public const string TeamNameRegex = @"^[A-Za-z\d\s\.\-]{3,}$";
        public const int TeamNationalityMinLength = 2;
        public const int TeamNationalityMaxLength = 40;

        //Coach
        public const int CoachNameMinLength = 2;
        public const int CoachNameMaxLength = 40;
    }
}
