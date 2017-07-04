using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreList : MonoBehaviour {

	public GameObject playerScoreEntryPrefab;

	ScoreManager scoreManager;

	int lastChangeCounter;

    private bool setWhite = false;

	// Use this for initialization
	void Start () {
		scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		lastChangeCounter = scoreManager.GetChangeCounter();
	}
	
	// Update is called once per frame
	void Update () {
		if(scoreManager == null) {
			return;
		}

		if(scoreManager.GetChangeCounter() == lastChangeCounter)
        {          
            return;
		}

		lastChangeCounter = scoreManager.GetChangeCounter();

		while(this.transform.childCount > 0) {
			Transform c = this.transform.GetChild(0);
			c.SetParent(null); 
			Destroy (c.gameObject);
		}

		string[] scores = scoreManager.GetPlayerNames("TotalPopulation");
		
		foreach(string score in scores) {
			GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
			go.transform.SetParent(this.transform);
			go.transform.Find ("Username").GetComponent<Text>().text = scoreManager.GetName(score);
			go.transform.Find ("TotalPopulation").GetComponent<Text>().text = scoreManager.GetScore(score, "TotalPopulation").ToString();
            go.transform.Find("TotalBabies").GetComponent<Text>().text = scoreManager.GetScore(score, "TotalBabies").ToString();
            go.transform.Find("DeathsAge").GetComponent<Text>().text = scoreManager.GetScore(score, "DeathsAge").ToString();
            go.transform.Find("DeathsEaten").GetComponent<Text>().text = scoreManager.GetScore(score, "DeathsEaten").ToString();
            go.transform.Find("DeathsStarved").GetComponent<Text>().text = scoreManager.GetScore(score, "DeathsStarved").ToString();
            go.transform.Find ("TotalDeaths").GetComponent<Text>().text = scoreManager.GetScore(score, "TotalDeaths").ToString();
            go.transform.Find("AverageFood").GetComponent<Text>().text = scoreManager.GetScore(score, "AverageFood").ToString("0.##"); 
            go.transform.Find("AverageBridges").GetComponent<Text>().text = scoreManager.GetScore(score, "AverageBridges").ToString("0.##");
            go.transform.Find("AverageHouses").GetComponent<Text>().text = scoreManager.GetScore(score, "AverageHouses").ToString("0.##");
            go.transform.Find("AverageProcreation").GetComponent<Text>().text = scoreManager.GetScore(score, "AverageProcreation").ToString("0.##");


            if (!setWhite && score.Equals(PlayerPrefs.GetString("UserName")))
            {
                go.transform.Find("Username").GetComponent<Text>().color = Color.white;
                go.transform.Find("TotalPopulation").GetComponent<Text>().color = Color.white;
                go.transform.Find("TotalBabies").GetComponent<Text>().color = Color.white;
                go.transform.Find("DeathsAge").GetComponent<Text>().color = Color.white;
                go.transform.Find("DeathsEaten").GetComponent<Text>().color = Color.white;
                go.transform.Find("DeathsStarved").GetComponent<Text>().color = Color.white;
                go.transform.Find("TotalDeaths").GetComponent<Text>().color = Color.white;
                go.transform.Find("AverageFood").GetComponent<Text>().color = Color.white;
                go.transform.Find("AverageBridges").GetComponent<Text>().color = Color.white;
                go.transform.Find("AverageHouses").GetComponent<Text>().color = Color.white;
                go.transform.Find("AverageProcreation").GetComponent<Text>().color = Color.white;

                setWhite = true;
            }
        }
    }
}
