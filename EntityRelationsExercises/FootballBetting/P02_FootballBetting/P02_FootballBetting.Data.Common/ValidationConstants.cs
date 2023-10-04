using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Common
{
    public static class ValidationConstants
    {
        //Team
        public const int TeamNameMaxLength = 50;
        public const int TeamLogoUrlMaxLength = 256;
        public const int TeamInitialsMaxLength = 5;

        //Color
        public const int ColorNameMaxLength = 25;

        //Town
        public const int TownNameMaxLength = 80;

        //Country
        public const int CountryNameMaxLength = 56;

        //Player
        public const int PlayerNameMaxLength = 80;

        //Position
        public const int  PositionNameMaxLength = 25;

        //Game
        public const int GameResultMaxLength = 10;

        //Bet
        public const int BetPredictionMaxLength = 10;

        //User
        public const int UserNameMaxLength = 80;
        public const int UserPasswordMaxLength = 256;
        public const int UserEmailMaxLength = 256;
    }
}
