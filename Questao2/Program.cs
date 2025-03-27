using Newtonsoft.Json;
using Questao2;
using RestSharp;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;

        totalGoals += GetGoalsByTeamPosition(team, year, "team1");
        totalGoals += GetGoalsByTeamPosition(team, year, "team2");

        return totalGoals;
    }

    private static int GetGoalsByTeamPosition(string team, int year, string teamPosition)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 1;
        string goalsField = teamPosition == "team1" ? "team1goals" : "team2goals";

        var client = new RestClient("https://jsonmock.hackerrank.com/api/football_matches");

        while (currentPage <= totalPages)
        {
            var request = new RestRequest();
            request.AddParameter("year", year);
            request.AddParameter(teamPosition, team);
            request.AddParameter("page", currentPage);

            var response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

            foreach (var match in result.data)
            {
                if (int.TryParse(teamPosition == "team1" ? match.team1goals : match.team2goals, out int goals))
                {
                    totalGoals += goals;
                }
            }

            totalPages = result.total_pages;
            currentPage++;
        }

        return totalGoals;
    }

    

    



}