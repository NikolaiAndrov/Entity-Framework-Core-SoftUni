using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillainNames
{
    public static class SqlQueries
    {
        public const string GetVillainNamesAndTheirMinionsCount =
            @"
                SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                    FROM Villains AS v 
                    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                    GROUP BY v.Id, v.Name 
                    HAVING COUNT(mv.VillainId) > 3 
                    ORDER BY COUNT(mv.VillainId)
            ";

        public const string GetVillainNameById = 
            @"
                SELECT Name FROM Villains WHERE Id = @Id
            ";

        public const string GetMinionsNameByVillainId =
            @"
                SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name
            ";

        public const string GetTownIdByTownName =
            @"
                SELECT Id FROM Towns WHERE Name = @townName
            ";

        public const string AddNewTown =
            @"
                INSERT INTO Towns (Name) VALUES (@townName)
            ";

        public const string GetVillainIdByVillainName =
            @"
                SELECT Id FROM Villains WHERE Name = @Name
            ";

        public const string AddVillainWithDefaultEvilnessFactor =
            @"
                INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)
            ";

        public const string AddNewMinion =
            @"
                INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)
            ";

        public const string GetMinionIdByMinionName =
            @"
                SELECT Id FROM Minions WHERE Name = @Name
            ";

        public const string SetServantMinionToVillain =
            @"
                INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)
            ";

        public const string SetTownNamesToUpperCaseForGivenCountry =
            @"
                UPDATE Towns
                    SET Name = UPPER(Name)
                    WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)
            ";

        public const string GetAllTownsForGivenCountry =
            @"
                 SELECT t.Name 
                    FROM Towns as t
                    JOIN Countries AS c ON c.Id = t.CountryCode
                    WHERE c.Name = @countryName
            ";

        public const string GetVillainNameToBeDeleted =
            @"
                SELECT Name FROM Villains WHERE Id = @villainId
            ";

        public const string ReleaseMinions =
            @"
                DELETE FROM MinionsVillains 
                    WHERE VillainId = @villainId
            ";

        public const string DeleteVillainById =
            @"
                DELETE FROM Villains
                    WHERE Id = @villainId
            ";

        public const string GetAllMinionsName =
            @"
                SELECT Name FROM Minions
            ";

        public const string UpdateMinionsNameAndAge =
            @"
                UPDATE Minions
                    SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                    WHERE Id = @Id
            ";

        public const string GetMinionsNameAndAge =
            @"
                SELECT Name, Age FROM Minions
            ";

        public const string IncreaseMinionAgeUsingProcedure =
            @"
                 EXEC usp_GetOlder @id = @givenId;
            ";

        public const string GetMinionNameAgeByGivenId =
            @"
                SELECT Name, Age FROM Minions WHERE Id = @Id
            ";
    }
}
