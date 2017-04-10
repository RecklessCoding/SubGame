using UnityEngine;
using UnityEngine.UI;

public class AgentsCountersTxtboxesUpdater : MonoBehaviour
{
    public GameObject endGameTxtbox;

    public GameObject populationCounterTxtbox;
    public GameObject infantsCounterTxtbox;
    public GameObject eatenCounterTxtbox;
    public GameObject starvedCounterTxtbox;
    public GameObject deathsCounterTxtbox;
    public GameObject averageLifespanTxtbox;
    public GameObject daysPassedTxtbox;

    public GameObject ScoreBoardPanel;
    public GameObject HSManager;

    private Text populationCounterTxt;
    private Text infantsCounterTxt;
    private Text eatenCounterTxt;
    private Text starvedCounterTxt;
    private Text deathsCounterTxt;
    private Text averageLifespaneTxt;
    private Text daysPassedTxt;

    private Text endGameTxt;
    private ScoreManager scoreManager;


    private AgentsManager agentsManager;
    private AgentsCreator agentsCreator;
    private AgentsDeathsHandler deathsHandler;
    private TimeDistribution timeDistribution;

    private float totalAgentsDied = 0;
    private float totalPopulation = 0;

    // Use this for initialization
    void Start()
    {
        agentsCreator = gameObject.GetComponent("AgentsCreator") as AgentsCreator;
        agentsManager = gameObject.GetComponent("AgentsManager") as AgentsManager;
        deathsHandler = gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
        timeDistribution = gameObject.GetComponent("TimeDistribution") as TimeDistribution;


        populationCounterTxt = populationCounterTxtbox.GetComponent<Text>();
        populationCounterTxt = populationCounterTxtbox.GetComponent<Text>();

        infantsCounterTxt = infantsCounterTxtbox.GetComponent<Text>();

        eatenCounterTxt = eatenCounterTxtbox.GetComponent<Text>();
        starvedCounterTxt = starvedCounterTxtbox.GetComponent<Text>();
        deathsCounterTxt = deathsCounterTxtbox.GetComponent<Text>();
        averageLifespaneTxt = averageLifespanTxtbox.GetComponent<Text>();

        daysPassedTxt = daysPassedTxtbox.GetComponent<Text>();

        endGameTxt = endGameTxtbox.GetComponent<Text>();
        scoreManager = HSManager.GetComponent("ScoreManager") as ScoreManager;
    }

    // Update is called once per frame
    void Update()
    {
        totalPopulation = agentsManager.HowManyAgentsAreALive;
        populationCounterTxt.text = totalPopulation.ToString();

        infantsCounterTxt.text = agentsCreator.HowManyAgentsWereBorned.ToString();

        eatenCounterTxt.text = deathsHandler.HowManyAgentsWereEaten.ToString();
        starvedCounterTxt.text = deathsHandler.HowManyAgentsWereStarved.ToString();
        deathsCounterTxt.text = deathsHandler.HowManyAgentsDied.ToString();
        averageLifespaneTxt.text = deathsHandler.HowManyDaysAgentsLived.ToString("0.0");

        daysPassedTxt.text = timeDistribution.DaysPassed.ToString();

        totalAgentsDied = deathsHandler.HowManyAgentsWereEaten + deathsHandler.HowManyAgentsDied
            + deathsHandler.HowManyAgentsWereStarved;

        totalPopulation = totalPopulation + totalAgentsDied;

        scoreManager.SetScore(PlayerPrefs.GetString("UserName"), Mathf.FloorToInt(totalPopulation), agentsCreator.HowManyAgentsWereBorned, new int[] { deathsHandler.HowManyAgentsDied, deathsHandler.HowManyAgentsWereEaten, deathsHandler.HowManyAgentsWereStarved, Mathf.FloorToInt(totalPopulation) }, 
            new float[] { timeDistribution.AverageFoodTime, timeDistribution.AverageBridgesTime, timeDistribution.AverageHouseTime });
    }

    internal void EndGame()
    {
        endGameTxtbox.active = true;

        endGameTxt.text = "Total agents: " + totalAgentsDied.ToString() + "\n"
            + "Over days: " + timeDistribution.DaysPassed.ToString() + "\n" +
            "Note: Text here is just for testing purposes of ending the game";

        ScoreBoardPanel.SetActive(true);
    }
}