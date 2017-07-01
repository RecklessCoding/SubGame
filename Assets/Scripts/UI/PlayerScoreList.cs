using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreList : MonoBehaviour {

	public GameObject playerScoreEntryPrefab;

	ScoreManager scoreManager;

	int lastChangeCounter;

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

		if(scoreManager.GetChangeCounter() == lastChangeCounter) {
			// No change since last update!
			return;
		}

		lastChangeCounter = scoreManager.GetChangeCounter();

		while(this.transform.childCount > 0) {
			Transform c = this.transform.GetChild(0);
			c.SetParent(null);  // Become Batman
			Destroy (c.gameObject);
		}

		string[] names = scoreManager.GetPlayerNames("TotalPopulation");
		
		foreach(string name in names) {
			GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
			go.transform.SetParent(this.transform);
			go.transform.Find ("Username").GetComponent<Text>().text = name;
			go.transform.Find ("TotalPopulation").GetComponent<Text>().text = scoreManager.GetScore(name, "TotalPopulation").ToString();
            go.transform.Find("TotalBabies").GetComponent<Text>().text = scoreManager.GetScore(name, "TotalBabies").ToString();
            go.transform.Find("DeathsAge").GetComponent<Text>().text = scoreManager.GetScore(name, "DeathsAge").ToString();
            go.transform.Find("DeathsEaten").GetComponent<Text>().text = scoreManager.GetScore(name, "DeathsEaten").ToString();
            go.transform.Find("DeathsStarved").GetComponent<Text>().text = scoreManager.GetScore(name, "DeathsStarved").ToString();
            go.transform.Find ("TotalDeaths").GetComponent<Text>().text = scoreManager.GetScore(name, "TotalDeaths").ToString();
            go.transform.Find("AverageFood").GetComponent<Text>().text = scoreManager.GetScore(name, "AverageFood").ToString("0.##"); 
            go.transform.Find("AverageBridges").GetComponent<Text>().text = scoreManager.GetScore(name, "AverageBridges").ToString("0.##");
            go.transform.Find("AverageHouses").GetComponent<Text>().text = scoreManager.GetScore(name, "AverageHouses").ToString("0.##");
            go.transform.Find("AverageProcreation").GetComponent<Text>().text = scoreManager.GetScore(name, "AverageProcreation").ToString("0.##");


        }
    }
}
