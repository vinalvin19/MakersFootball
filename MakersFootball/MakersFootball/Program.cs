using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MakersFootball
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);

            var fileName = Path.Combine(directory.FullName, "SoccerGameResults.csv");

            var fileContents = ReadFootballResults(fileName);

            Console.ReadLine();
        }

        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<GameResult> ReadFootballResults(string fileName)
        {
            var soccerResults = new List<GameResult>();
            using (var reader = new StreamReader(fileName))
            {
                var line = "";
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    var GameResult = new GameResult();
                    DateTime gameDate;
                    string[] values = line.Split(',');

                    if (DateTime.TryParse(values[0], out gameDate))
                    {
                        GameResult.GameDate = gameDate;
                    }

                    GameResult.TeamName = values[1];

                    HomeOrAway HomeOrAway;
                    if (Enum.TryParse(values[2], out HomeOrAway))
                    {
                        GameResult.HomeOrAway = HomeOrAway;
                    }
                    soccerResults.Add(GameResult);

                    int parseInt;
                    if (int.TryParse(values[3], out parseInt))
                    {
                        GameResult.Goals = parseInt;
                    }
                    if (int.TryParse(values[4], out parseInt))
                    {
                        GameResult.GoalAttempts = parseInt;
                    }
                    if (int.TryParse(values[5], out parseInt))
                    {
                        GameResult.ShotsOnGoal = parseInt;
                    }
                    if (int.TryParse(values[6], out parseInt))
                    {
                        GameResult.ShotsOffGoal = parseInt;
                    }

                }
            }
            return soccerResults;
        }
    }
}
