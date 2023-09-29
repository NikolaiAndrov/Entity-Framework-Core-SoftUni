using Microsoft.Data.SqlClient;
using System.Text;

namespace VillainNames
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            await sqlConnection.OpenAsync();

            // P04
            //string[] minionInfo = Console.ReadLine()
            //    .Split(": ", StringSplitOptions.RemoveEmptyEntries);

            //string[] villainInfo = Console.ReadLine()
            //    .Split(": ", StringSplitOptions.RemoveEmptyEntries);

            //string result = await AddNewMinionAsync(sqlConnection, minionInfo[1], villainInfo[1]);
            //Console.WriteLine(result);
        }


        // P09
        static async Task IncreaseAgeStoredProcedureAsync(SqlConnection sqlConnection, int minionId)
        {
            SqlCommand increaseAgeCmd = new SqlCommand(SqlQueries.IncreaseMinionAgeUsingProcedure, sqlConnection);
            increaseAgeCmd.Parameters.AddWithValue("@givenId", minionId);
            await increaseAgeCmd.ExecuteNonQueryAsync();

            SqlCommand getNameAgeCmd = new SqlCommand(SqlQueries.GetMinionNameAgeByGivenId, sqlConnection);
            getNameAgeCmd.Parameters.AddWithValue("@Id", minionId);
            SqlDataReader reader = await getNameAgeCmd.ExecuteReaderAsync();
            reader.Read();
            await Console.Out.WriteLineAsync($"{reader["Name"]} – {reader["Age"]} years old");
        }


        // P08
        static async Task IncreaseMinionAgeAsync(SqlConnection sqlConnection)
        {
            int[] ids = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();


            for (int i = 0; i < ids.Length; i++)
            {
                SqlCommand updateMinionsCmd = new SqlCommand(SqlQueries.UpdateMinionsNameAndAge, sqlConnection);
                updateMinionsCmd.Parameters.AddWithValue("@Id", ids[i]);
                await updateMinionsCmd.ExecuteNonQueryAsync();
            }

            SqlCommand getMinonsNameAndAgeCmd = new SqlCommand(SqlQueries.GetMinionsNameAndAge, sqlConnection);
            SqlDataReader reader = await getMinonsNameAndAgeCmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                await Console.Out.WriteLineAsync($"{reader["Name"]} {reader["Age"]}");
            }
        }


        // P07
        static async Task PrintAllMinionNamesAsync(SqlConnection sqlConnection)
        {
            SqlCommand printMinionsCmd = new SqlCommand(SqlQueries.GetAllMinionsName, sqlConnection);
            SqlDataReader reader = await printMinionsCmd.ExecuteReaderAsync();

            List<string> names = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string currentName = (string)reader["Name"];
                    names.Add(currentName);
                }
            }

            for (int i = 0; i < names.Count / 2; i++)
            {
                await Console.Out.WriteLineAsync(names[i]);
                await Console.Out.WriteLineAsync(names[names.Count - i - 1]);
            }
        }


        // P06
        static async Task<string> RemoveVillainByIdAsync(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand getVillainNameByIdCmd = new SqlCommand(SqlQueries.GetVillainNameToBeDeleted, sqlConnection);
            getVillainNameByIdCmd.Parameters.AddWithValue("@villainId", villainId);
            string? villainName = (string?)await getVillainNameByIdCmd.ExecuteScalarAsync();

            if (villainName == null)
            {
                sb.AppendLine("No such villain was found.");
            }
            else
            {
                sb.AppendLine($"{villainName} was deleted.");

                SqlCommand releaseMinionsCmd = new SqlCommand(SqlQueries.ReleaseMinions, sqlConnection);
                releaseMinionsCmd.Parameters.AddWithValue("@villainId", villainId);
                int minionsReleasedCount = await releaseMinionsCmd.ExecuteNonQueryAsync();

                sb.AppendLine($"{minionsReleasedCount} minions were released.");

                SqlCommand deleteVillainCmd = new SqlCommand(SqlQueries.DeleteVillainById, sqlConnection);
                deleteVillainCmd.Parameters.AddWithValue("@villainId", villainId);

                await deleteVillainCmd.ExecuteNonQueryAsync();
            }

            return sb.ToString().TrimEnd();
        }


        // P05
        static async Task<string> ChangeTownNamesCasingAsync(SqlConnection sqlConnection, string countryName)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand setTownNamesCmd = new SqlCommand(SqlQueries.SetTownNamesToUpperCaseForGivenCountry, sqlConnection);
            setTownNamesCmd.Parameters.AddWithValue("@countryName", countryName);
            int townsCountUpdated = await setTownNamesCmd.ExecuteNonQueryAsync();

            if (townsCountUpdated > 0)
            {
                sb.AppendLine($"{townsCountUpdated} town names were affected.");

                SqlCommand getAllTownsCmd = new SqlCommand(SqlQueries.GetAllTownsForGivenCountry, sqlConnection);
                getAllTownsCmd.Parameters.AddWithValue("@countryName", countryName);

                List<string> towns = new List<string>();
                SqlDataReader reader = await getAllTownsCmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string currentTown = (string)reader["Name"];
                    towns.Add(currentTown);
                }

                sb.AppendLine($"[{string.Join(", ", towns)}]");
            }
            else
            {
                sb.AppendLine("No town names were affected.");
            }

            return sb.ToString().TrimEnd();
        }

        
        // P04
        static async Task<string> AddNewMinionAsync(SqlConnection sqlConnection, string minionInfo, string villainName)
        {
            string[] minionArgs = minionInfo
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string minionName = minionArgs[0];
            int minionAge = int.Parse(minionArgs[1]);
            string townName = minionArgs[2];

            StringBuilder sb = new StringBuilder();

            SqlTransaction transaction = sqlConnection.BeginTransaction();
            try
            {
                int townId = await GetTownIdByNameAsync(sqlConnection, transaction, townName, sb);
                int villainId = await GetVillainIdByNameAsync(sqlConnection, transaction, villainName, sb);
                int minionId = await AddNewMinionAndReturnIdAsync(sqlConnection, transaction, minionName, minionAge, townId);
                await SetServantMinionToVillainAsync(sqlConnection, transaction, minionId, villainId);

                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
            }

            return sb.ToString().TrimEnd();
        }

        static async Task SetServantMinionToVillainAsync(SqlConnection sqlConnection, SqlTransaction transaction, int minionId, int villainId)
        {
            SqlCommand addMinionAndVillainCmd = new SqlCommand(SqlQueries.SetServantMinionToVillain, sqlConnection, transaction);
            addMinionAndVillainCmd.Parameters.AddWithValue("@minionId", minionId);
            addMinionAndVillainCmd.Parameters.AddWithValue("@villainId", villainId);
            await addMinionAndVillainCmd.ExecuteNonQueryAsync();
        }
        static async Task<int> AddNewMinionAndReturnIdAsync(SqlConnection sqlConnection, SqlTransaction transaction, string minionName, int minionAge, int townId)
        {
            SqlCommand addMinionCmd = new SqlCommand(SqlQueries.AddNewMinion, sqlConnection, transaction);
            addMinionCmd.Parameters.AddWithValue("@name", minionName);
            addMinionCmd.Parameters.AddWithValue("@age", minionAge);
            addMinionCmd.Parameters.AddWithValue("@townId", townId);

            await addMinionCmd.ExecuteNonQueryAsync();

            SqlCommand getMinionIdCmd = new SqlCommand(SqlQueries.GetMinionIdByMinionName, sqlConnection, transaction);
            getMinionIdCmd.Parameters.AddWithValue("@Name", minionName);

            int minionId = (int)await getMinionIdCmd.ExecuteScalarAsync();

            return minionId;
        }
        static async Task<int> GetVillainIdByNameAsync(SqlConnection sqlConnection, SqlTransaction transaction, string villainName, StringBuilder sb)
        {
            SqlCommand getVillainIdByNameCmd = new SqlCommand(SqlQueries.GetVillainIdByVillainName, sqlConnection, transaction);
            getVillainIdByNameCmd.Parameters.AddWithValue("@Name", villainName);

            int? villainId = (int?)await getVillainIdByNameCmd.ExecuteScalarAsync();
            if (villainId == null)
            {
                SqlCommand addNewVillainCmd = new SqlCommand(SqlQueries.AddVillainWithDefaultEvilnessFactor, sqlConnection, transaction);
                addNewVillainCmd.Parameters.AddWithValue("@villainName", villainName);

                await addNewVillainCmd.ExecuteNonQueryAsync();
                villainId = (int?)await getVillainIdByNameCmd.ExecuteScalarAsync();

                sb.AppendLine($"Villain {villainName} was added to the database.");
            }

            return villainId.Value;
        }
        static async Task<int> GetTownIdByNameAsync(SqlConnection sqlConnection, SqlTransaction transaction, string townName, StringBuilder sb)
        {
            SqlCommand getTownIdByName = new SqlCommand(SqlQueries.GetTownIdByTownName, sqlConnection, transaction);
            getTownIdByName.Parameters.AddWithValue("@townName", townName);
            object? townId = await getTownIdByName.ExecuteScalarAsync();

            if (townId == null)
            {
                SqlCommand addNewTown = new SqlCommand(SqlQueries.AddNewTown, sqlConnection, transaction);
                addNewTown.Parameters.AddWithValue("@townName", townName);
                await addNewTown.ExecuteNonQueryAsync();
                townId = await getTownIdByName.ExecuteScalarAsync();

                sb.AppendLine($"Town {townName} was added to the database.");
            }

            return (int)townId;
        }


        // P03
        static async Task<string> GetAllMinionNamesAndAgesForGivenVillainIdAsync(SqlConnection sqlConnection, int id)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand getVillainNameCmd = new SqlCommand(SqlQueries.GetVillainNameById, sqlConnection);
            getVillainNameCmd.Parameters.AddWithValue("@Id", id);

            object? villainNameObj = await getVillainNameCmd.ExecuteScalarAsync();

            if (villainNameObj == null)
            {
                return $"No villain with ID {id} exists in the database.";
            }

            string villainName = (string)villainNameObj;
            sb.AppendLine($"Villain: {villainName}");

            SqlCommand getMinionsNameCmd = new SqlCommand(SqlQueries.GetMinionsNameByVillainId, sqlConnection);
            getMinionsNameCmd.Parameters.AddWithValue("Id", id);

            SqlDataReader minionsReader = await getMinionsNameCmd.ExecuteReaderAsync();

            if (!minionsReader.HasRows)
            {
                sb.AppendLine("(no minions)");
            }
            else
            {
                while (minionsReader.Read())
                {
                    long rowNum = (long)minionsReader["RowNum"];
                    string minionName = (string)minionsReader["Name"];
                    int minionAge = (int)minionsReader["Age"];

                    sb.AppendLine($"{rowNum}. {minionName} {minionAge}");
                }
            }

            minionsReader.Close();
            return sb.ToString().TrimEnd();
        }


        // P02
        static async Task<string> GetVillainNamesAndTMinionsCountAsync(SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand sqlCommand = new SqlCommand(SqlQueries.GetVillainNamesAndTheirMinionsCount, sqlConnection);
            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            while (reader.Read())
            {
                string villainName = (string)reader["Name"];
                int minionsCount = (int)reader["MinionsCount"];
                sb.AppendLine($"{villainName} - {minionsCount}");
            }

            reader.Close();
            return sb.ToString().TrimEnd();
        }
    }
}