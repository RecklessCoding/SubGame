using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{

    // The map we're building is going to look like:
    //
    //	LIST OF USERS -> A User -> LIST OF SCORES for that user
    //

    Dictionary<string, Dictionary<string, float>> playerScores;

    int changeCounter = 0;

    void Start()
    {
        Init();
    }

    void Init()
    {
        if (playerScores != null)
            return;

        playerScores = new Dictionary<string, Dictionary<string, float>>();
    }

    public void Reset()
    {
        changeCounter++;
        playerScores = null;
    }

    public float GetScore(string username, string scoreType)
    {
        Init();

        if (playerScores.ContainsKey(username) == false)
        {
            // We have no score record at all for this username
            return 0;
        }

        if (playerScores[username].ContainsKey(scoreType) == false)
        {
            return 0;
        }

        return playerScores[username][scoreType];
    }

    public void SetScore(string username, int totalPopulation, int totalBabies, int [] deaths, float[] averages)
    {
        Init();

        changeCounter++;

        if (!playerScores.ContainsKey(username))
        {
            playerScores[username] = new Dictionary<string, float>();
        }

        playerScores[username]["TotalPopulation"] = totalPopulation;
        playerScores[username]["TotalBabies"] = totalBabies;
        playerScores[username]["DeathsAge"] = deaths[0];
        playerScores[username]["DeathsEaten"] = deaths[1];
        playerScores[username]["DeathsStarved"] = deaths[2];
        playerScores[username]["TotalDeaths"] = deaths[3];

        playerScores[username]["AverageFood"] = averages[0];
        playerScores[username]["AverageBridges"] = averages[1];
        playerScores[username]["AverageHouses"] = averages[2];
    }


    public void SetScore(string username, string scoreType, int value)
    {
        Init();

        changeCounter++;

        if (playerScores.ContainsKey(username) == false)
        {
            playerScores[username] = new Dictionary<string, float>();
        }

        playerScores[username][scoreType] = value;
    }
 
    public string[] GetPlayerNames()
    {
        Init();
        return playerScores.Keys.ToArray();
    }

    public string[] GetPlayerNames(string sortingScoreType)
    {
        Init();

        return playerScores.Keys.OrderByDescending(n => GetScore(n, sortingScoreType)).ToArray();
    }

    public int GetChangeCounter()
    {
        return changeCounter;
    }
}
