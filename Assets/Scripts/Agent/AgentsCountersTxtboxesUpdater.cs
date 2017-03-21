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


    private Text populationCounterTxt;
    private Text infantsCounterTxt;
    private Text eatenCounterTxt;
    private Text starvedCounterTxt;
    private Text deathsCounterTxt;
    private Text averageLifespaneTxt;
    private Text daysPassedTxt;

    private Text endGameTxt;


    private AgentsManager agentsManager;
    private AgentsCreator agentsCreator;
    private AgentsDeathsHandler deathsHandler;
    private TimeDistribution timeDistribution;

    private float totalAgentsDied = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        populationCounterTxt.text = agentsManager.HowManyAgentsAreALive.ToString();

        infantsCounterTxt.text = agentsCreator.HowManyAgentsWereBorned.ToString();

        eatenCounterTxt.text = deathsHandler.HowManyAgentsWereEaten.ToString();
        starvedCounterTxt.text = deathsHandler.HowManyAgentsWereStarved.ToString();
        deathsCounterTxt.text = deathsHandler.HowManyAgentsDied.ToString();
        averageLifespaneTxt.text = deathsHandler.HowManyDaysAgentsLived.ToString("0.0");

        daysPassedTxt.text = timeDistribution.DaysPassed.ToString();

        totalAgentsDied = deathsHandler.HowManyAgentsWereEaten + deathsHandler.HowManyAgentsDied
            + deathsHandler.HowManyAgentsWereStarved;
    }

    internal void EndGame()
    {
        endGameTxtbox.active = true;

        endGameTxt.text = "Total agents: " + totalAgentsDied.ToString() + "\n"
            + "Over days: " + timeDistribution.DaysPassed.ToString() + "\n" +
            "Note: Text here is just for testing purposes of ending the game";
    }
}