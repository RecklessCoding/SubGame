using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{

    // The map we're building is going to look like:
    //
    //	LIST OF USERS -> A User -> LIST OF SCORES for that user
    //

    Dictionary<string, string> displayNames;

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

        displayNames = new Dictionary<string, string>();
        playerScores = new Dictionary<string, Dictionary<string, float>>();

        string displayName = "";
        for (int i = 0; i < PlayerPrefs.GetInt("saved_total"); i++)
        {
            displayName = PlayerPrefs.GetString("UserName" + (i));

            SetScore(displayName+i,displayName, PlayerPrefs.GetInt("TotalPopulation" + i), PlayerPrefs.GetInt("TotalBabies"+i),
              new int[] { PlayerPrefs.GetInt("DeathsAge" + i), PlayerPrefs.GetInt("DeathsEaten"+ (i)), PlayerPrefs.GetInt("DeathsStarved"+i), PlayerPrefs.GetInt("TotalDeaths"+i) }
              , new float[] { PlayerPrefs.GetFloat("AverageFood"+i), PlayerPrefs.GetFloat("AverageBridges"+i), PlayerPrefs.GetFloat("AverageHouses" +i), PlayerPrefs.GetFloat("AverageProcreation"+i) });
        }
    }

    public void Reset()
    {
        changeCounter++;
        playerScores = null;
        PlayerPrefs.DeleteAll();
    }

    public float GetScore(string username, string scoreType)
    {
        Init();

        if (playerScores.ContainsKey(username) == false)
        {
            return 0;
        }

        if (playerScores[username].ContainsKey(scoreType) == false)
        {
            return 0;
        }

        return playerScores[username][scoreType];       
    }

    public string GetName(string username)
    {
        Init();

        if (displayNames.ContainsKey(username) == false)
        {
            return username;
        }

        return displayNames[username];
    }


    public void SetScore(string username, string displayName, int totalPopulation, int totalBabies, int[] deaths, float[] averages)
    {
        Init();

        changeCounter++;

        if (!playerScores.ContainsKey(username))
        {
            playerScores[username] = new Dictionary<string, float>();
        }

        if (displayNames.ContainsKey(username) == false)
        {
            displayNames.Add(username, displayName);
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
        playerScores[username]["AverageProcreation"] = averages[3];
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
