﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;

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

            fileName = Path.Combine(directory.FullName, "players.json");
            var players = DeserializedPlayer(fileName);

            var topTenPlayers = GetTopTenPlayers(players);

            foreach (var player in topTenPlayers)
            {
                List<NewsResult> newsResults = GetNewsFromPlayer(string.Format("{0} {1}", player.FirstName, player.SecondName));
                foreach (var news in newsResults)
                {
                    Console.WriteLine(string.Format("Date: {0} \nHeadline: {1} \nSummary: {2} \r\n", news.datePublished, news.Headline, news.Summary));
                    Console.ReadKey();
                }
            }

            var fileTopTen = Path.Combine(directory.FullName, "topten.json");
            SerializePlayerToFile(topTenPlayers, fileTopTen);

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

                    Double PossessionPercent;
                    if (Double.TryParse(values[6], out PossessionPercent))
                    {
                        GameResult.PossessionPercent = PossessionPercent;
                    }

                    soccerResults.Add(GameResult);
                }
            }
            return soccerResults;
        }

        public static List<Player> DeserializedPlayer(string fileName)
        {
            var players = new List<Player>();
            var serializer = new JsonSerializer();

            using (var reader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(reader))
            {
                players = serializer.Deserialize<List<Player>>(jsonReader);
            }
            return players;
        }

        public static List<Player> GetTopTenPlayers(List<Player> players)
        {
            players.Sort(new PlayerComparer());
            var topTenPlayers = new List<Player>();
            var i = 0;

            foreach (var player in players)
            {
                topTenPlayers.Add(player);
                i++;

                if (i == 10)
                    break;
            }

            return topTenPlayers;
        }

        public static void SerializePlayerToFile(List<Player> players, string fileName)
        {
            var serializer = new JsonSerializer();

            using (var writer = new StreamWriter(fileName))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, players);
            }
        }

        public static string GetGoogleHomePage()
        {
            var webClient = new WebClient();
            byte[] googleHome = webClient.DownloadData("https://www.google.com");

            using (var stream = new MemoryStream(googleHome))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<NewsResult> GetNewsFromPlayer(string playerName)
        {
            var results = new List<NewsResult>();
            var serializer = new JsonSerializer();
            var webClient = new WebClient();
            webClient.Headers.Add("Ocp-Apim-Subscription-Key", "9e6467bf15cd4474a72b807d647ecae7");
            byte[] searchResult = webClient.DownloadData(string.Format("https://api.cognitive.microsoft.com/bing/v5.0/news/search?q={0}&mkt=en-us", playerName));

            using (var stream = new MemoryStream(searchResult))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                results = serializer.Deserialize<NewsSearch>(jsonReader).NewsResults;
            }
            return results;
        }
    }
}
