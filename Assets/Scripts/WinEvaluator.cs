using System.Collections.Generic;
using UnityEngine;

public class WinEvaluator 
{


    public string EvaluateWinnerAndGetResultText(Dictionary<string, IHandDataProvider> playersData)
    {
        List<string> winners = GetWinners(playersData);
        return GetResultText(winners);
    }

    private List<string> GetWinners(Dictionary<string, IHandDataProvider> playersData)
    {
        int bestScore = -1;
        const int maxValidScore = 21;
        List<string> winners = new List<string>();

        foreach (var kvp in playersData)
        {
            string player = kvp.Key;
            int score = kvp.Value.GetHandScore();

            if (score > maxValidScore)
                continue; // Busted

            if (score > bestScore)
            {
                bestScore = score;
                winners.Clear();
                winners.Add(player);
            }
            else if (score == bestScore)
            {
                winners.Add(player);
            }
        }

        return winners;
    }

    private string GetResultText(List<string> winners)
    {
        string resultMessage = "";
        switch (winners.Count)
        {
            case 0:
                resultMessage = "All players busted!";
                break;
            case 1:
                resultMessage = winners[0] + " Won!";
                break;
            default:
                resultMessage = "Game Tied!";
                break;

        }
        return resultMessage;
    }

}
