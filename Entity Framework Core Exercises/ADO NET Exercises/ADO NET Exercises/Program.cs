using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADO_NET_Exercises
{
    class Program
    {
        private const string masterConnectionString = "Server=.;Integrated Security=true;Database=master";
        private const string minionsConnectionString = "Server=.;Integrated Security=true;Database=MinionsDB";
        static void Main(string[] args)
        {
            //Uncomment only the task you need to check

            //Creates DB, adds Tables and Inserts data. Do this method once
            //InnitialSetup();


            //Expected return : Jilly - 4
            //Check the comments in method to see why the doc file doesn't match
            //Exercise2();


            //No villians have no minions for some reason
            //Exercise3();


            //Have not done the bonus
            //Exercise4();


            //Norway has no towns, but is in the DB
            //Exercise5();


            //No idea what to put here but every other has one so here have some filler text
            //Exercise7();


            //I'm probably just lazy since half of these were done on a seperate day
            //Exercise8();


            //Apparently working with procedures is really easy
            //Exercise9();
        }

        private static void Exercise9()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                int id = int.Parse(Console.ReadLine());
                //Apparently only needs the name of the procedure
                using (SqlCommand cmd = new SqlCommand("usp_GetOlder", connection))
                {
                    //Changes how the command is seen i assume (googled) otherwise works the same
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                string getMinionSQL = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
                using (var sqlCommand = new SqlCommand(getMinionSQL, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var minName = reader["Name"] as string;
                        var minAge = reader["Age"] as int?;
                        Console.WriteLine($"{minName} – {minAge} years old");
                    }
                }
            }
        }

        private static void Exercise8()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                int[] minIds = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                string SQLUpdate = @" UPDATE Minions
   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
 WHERE Id = @Id";
                foreach (var id in minIds)
                {
                    ExecuteNonQuery(connection, SQLUpdate, "@Id", id.ToString());
                }

                string GetMinionsCommand = "SELECT Name, Age FROM Minions";
                using (var sqlCommand = new SqlCommand(GetMinionsCommand, connection))
                {
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var minName = reader["Name"] as string;
                        var minAge = reader["Age"] as int?;

                        Console.WriteLine($"{minName} {minAge}");
                    }
                }

            }
        }

        private static void Exercise7()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string GetMinionsCommand = @"SELECT Name FROM Minions";
                using (var sqlCommand = new SqlCommand(GetMinionsCommand, connection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    var minions = new List<string>();
                    while (reader.Read())
                    {
                        var minName = reader[0] as string;

                        minions.Add(minName);
                    }
                    Console.WriteLine(String.Join(", ", minions));
                    for (int i = 0; i < minions.Count / 2; i++)
                    {
                        Console.WriteLine(minions[i]);
                        Console.WriteLine(minions[minions.Count - i - 1]);
                    }

                    if (minions.Count % 2 == 1)
                    {
                        Console.WriteLine(minions[minions.Count / 2]);
                    }
                }
            }
        }

        private static void Exercise5()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string country = Console.ReadLine();

                string SelectCommand = @" SELECT t.Name 
   FROM Towns as t
   JOIN Countries AS c ON c.Id = t.CountryCode
  WHERE c.Name = @countryName";
                string hasOneTown = ExecuteScalar(connection, SelectCommand, "@countryName", country) as string;

                if (hasOneTown != null)
                {
                    string SQLUpdateCommand = @"UPDATE Towns
   SET Name = UPPER(Name)
 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                    int rows = ExecuteNonQuery(connection, SQLUpdateCommand, "@countryName", country);
                    Console.WriteLine($"{rows} town names were affected.");
                    using (var sqlCommand = new SqlCommand(SelectCommand, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@countryName", country);
                        var reader = sqlCommand.ExecuteReader();
                        var towns = new List<string>();
                        while (reader.Read())
                        {
                            var name = reader[0] as string;

                            towns.Add(name);
                        }
                        Console.WriteLine($"[{String.Join(", ", towns)}]");
                    }
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }

            }
        }

        private static void Exercise4()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string[] minionInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string minName = minionInfo[1];
                string minAge = minionInfo[2];
                string minTown = minionInfo[3];

                string[] villianInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string villName = villianInfo[1];


                //Gets TownId. If it doesn't exist it, creates it and gets it.
                string SQLTownQuery = @"SELECT Id FROM Towns WHERE Name = @townName";
                var townId = ExecuteScalar(connection, SQLTownQuery, "@townName", minTown) as int?;
                if (townId == null)
                {
                    string SQLTownCreationQuery = @"INSERT INTO Towns (Name) VALUES (@townName)";
                    ExecuteNonQuery(connection, SQLTownCreationQuery, "@townName", minTown);
                    Console.WriteLine($"Town {minTown} was added to the database.");
                    townId = ExecuteScalar(connection, SQLTownQuery, "@townName", minTown) as int?;
                }

                //Gets VillianId. If it doesn't exist it, creates it and gets it.
                string SQLGetVillianQuery = @"SELECT Id FROM Villains WHERE Name = @Name";
                var villianId = ExecuteScalar(connection, SQLGetVillianQuery, "@Name", villName) as int?;
                if (villianId == null)
                {
                    string SQLTownCreationQuery = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                    ExecuteNonQuery(connection, SQLTownCreationQuery, "@villainName", villName);
                    Console.WriteLine($"Villain {villName} was added to the database.");
                    villianId = ExecuteScalar(connection, SQLGetVillianQuery, "@Name", villName) as int?;
                }

                //Inserts minion into DB
                string SQLMinionInsertionQuery = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
                ExecuteNonQuery(connection, SQLMinionInsertionQuery, "@name", minName, "@age", minAge, "@townId", townId.ToString());
                //Gets minion Id
                string SQLMinionGetQuery = "SELECT Id FROM Minions WHERE Name = @Name";
                int? minId = ExecuteScalar(connection, SQLMinionGetQuery, "@Name", minName) as int?;
                //Inserts both Ids into MinionsVillains table
                string SQLMinionVilliansInsertionQuery = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";
                ExecuteNonQuery(connection, SQLMinionVilliansInsertionQuery, "@villainId", villianId.ToString(), "@minionId", minId.ToString());
                Console.WriteLine($"Successfully added {minName} to be minion of {villName}.");
            }
        }

        private static void Exercise3()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string id = Console.ReadLine();
                string commandVillian = @"SELECT Name FROM Villains WHERE Id = @Id";
                string villianName = ExecuteScalar(connection, commandVillian, "@Id", id) as string;
                if (villianName != null)
                {
                    Console.WriteLine($"Villain: {villianName}");
                    string commandMinions = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                    using (var sqlCommand = new SqlCommand(commandMinions, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Id", id);
                        var reader = sqlCommand.ExecuteReader();


                        bool hasRead = false;
                        while (reader.Read())
                        {
                            hasRead = true;
                            var row = reader["RowNum"];
                            var minionName = reader["Name"];
                            var count = reader["Age"];
                            Console.WriteLine($"{row}. {minionName} {count}");
                        }
                        if (!hasRead)
                        {
                            Console.WriteLine("(no minions)");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No villain with ID {id} exists in the database.");
                }

            }
        }

        private static void Exercise2()
        {
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string command = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
    FROM Villains AS v 
    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
GROUP BY v.Id, v.Name 
  HAVING COUNT(mv.VillainId) > 3 
ORDER BY COUNT(mv.VillainId)";

                using (var sqlCommand = new SqlCommand(command, connection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var name = reader[0] as string;
                        var count = reader[1] as int?;
                        //For some reason there is less data the in the doc file.
                        //Both the query seems correct and the insertions were copied correctly.
                        Console.WriteLine($"{name} - {count}");
                    }
                }
            }
        }

        private static void InnitialSetup()
        {
            // Creates DB
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                string command = "CREATE DATABASE MinionsDB";
                ExecuteNonQuery(connection, command);
            }

            //Adds Tables into DB
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string[] commands = GetTableCreationQueries();
                foreach (var command in commands)
                {
                    ExecuteNonQuery(connection, command);
                }
            }

            //Inserts Data
            using (var connection = new SqlConnection(minionsConnectionString))
            {
                connection.Open();
                string[] commands = GetInsertionQueries();
                foreach (var command in commands)
                {
                    ExecuteNonQuery(connection, command);
                }
            }
        }

        //SQL Query getters
        private static string[] GetTableCreationQueries()
        {
            return new string[]
            {
                "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))",
                "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",
                "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",
                "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))"
            };
        }

        private static string[] GetInsertionQueries()
        {
            return new string[]
            {
                "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",
                "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",
                "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",
                "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",
                "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)",
                "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)"
            };
        }

        //Utility Methods
        private static int ExecuteNonQuery(SqlConnection connection, string command, params string[] variables)
        {
            using (var sqlCommand = new SqlCommand(command, connection))
            {
                for (int i = 0; i < variables.Length; i += 2)
                {
                    sqlCommand.Parameters.AddWithValue(variables[i], variables[i + 1]);
                }
                return sqlCommand.ExecuteNonQuery();
            }
        }

        private static Object ExecuteScalar(SqlConnection connection, string command, params string[] variables)
        {
            using (var sqlCommand = new SqlCommand(command, connection))
            {
                for (int i = 0; i < variables.Length / 2; i++)
                {
                    sqlCommand.Parameters.AddWithValue(variables[i], variables[i + 1]);
                }
                
                var obj = sqlCommand.ExecuteScalar();
                return obj;
            }
        }
    }
}
