using UnityEngine;
using UnityEngine.UI;

public class AgentsCountersTxtboxesUpdater : MonoBehaviour
{
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

    private ScoreManager scoreManager;

    private AgentsManager agentsManager;
    private AgentsCreator agentsCreator;
    private AgentsDeathsHandler deathsHandler;
    private TimeDistribution timeDistribution;

    private int totalAgentsDied = 0;
    private int totalPopulation = 0;

    private int daysPassed;

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

        if (Time.timeScale > 0)
        {
            UpdateScoreManager();
        }
    }

    internal void UpdateScoreManager()
    {
        timeDistribution.UpdateAverages();

        PlayerPrefs.SetInt("TotalPopulation" + (PlayerPrefs.GetInt("saved_total") - 1), totalPopulation); //set the new total value
        PlayerPrefs.SetInt("TotalBabies" + (PlayerPrefs.GetInt("saved_total") - 1), agentsCreator.HowManyAgentsWereBorned); //set the new total value
        PlayerPrefs.SetInt("DeathsAge" + (PlayerPrefs.GetInt("saved_total") - 1), deathsHandler.HowManyAgentsDied); //set the new total value
        PlayerPrefs.SetInt("DeathsEaten" + (PlayerPrefs.GetInt("saved_total") - 1), deathsHandler.HowManyAgentsWereEaten); //set the new total value
        PlayerPrefs.SetInt("DeathsStarved" + (PlayerPrefs.GetInt("saved_total") - 1), deathsHandler.HowManyAgentsWereStarved); //set the new total value
        PlayerPrefs.SetInt("TotalDeaths" + (PlayerPrefs.GetInt("saved_total") - 1), totalAgentsDied); //set the new total value
        PlayerPrefs.SetFloat("AverageFood" + (PlayerPrefs.GetInt("saved_total") - 1), timeDistribution.AverageFoodTime); //set the new total value
        PlayerPrefs.SetFloat("AverageBridges" + (PlayerPrefs.GetInt("saved_total") - 1), timeDistribution.AverageBridgesTime); //set the new total value
        PlayerPrefs.SetFloat("AverageHouses" + (PlayerPrefs.GetInt("saved_total") - 1), timeDistribution.AverageHouseTime); //set the new total value
        PlayerPrefs.SetFloat("AverageProcreation" + (PlayerPrefs.GetInt("saved_total") - 1), timeDistribution.AverageProcreationTime); //set the new total value
        PlayerPrefs.Save();

        scoreManager.SetScore(PlayerPrefs.GetString("UserName"), PlayerPrefs.GetString("DisplayName"), totalPopulation, agentsCreator.HowManyAgentsWereBorned, new int[] { deathsHandler.HowManyAgentsDied, deathsHandler.HowManyAgentsWereEaten, deathsHandler.HowManyAgentsWereStarved, totalAgentsDied },
    new float[] { timeDistribution.AverageFoodTime, timeDistribution.AverageBridgesTime, timeDistribution.AverageHouseTime, timeDistribution.AverageProcreationTime });

        if (daysPassed < timeDistribution.DaysPassed)
        {
            daysPassed = timeDistribution.DaysPassed;

            LogfileWriter.GetInstance().WriteLogline(daysPassed, totalPopulation, agentsCreator.HowManyAgentsWereBorned,
               new int[] { deathsHandler.HowManyAgentsDied, deathsHandler.HowManyAgentsWereEaten, deathsHandler.HowManyAgentsWereStarved, totalAgentsDied },
          new float[] { timeDistribution.FoodTime, timeDistribution.BridgesTime, timeDistribution.HousesTime, timeDistribution.ProcreationTime },
          new float[] { timeDistribution.AverageFoodTime, timeDistribution.AverageBridgesTime, timeDistribution.AverageHouseTime, timeDistribution.AverageProcreationTime });
        }
    }

    internal void WriteLogBeforeFlood()
    {
        LogfileWriter.GetInstance().WriteFloodLine(daysPassed,"BeforeFlood");
        LogfileWriter.GetInstance().WriteLogline(daysPassed, totalPopulation, agentsCreator.HowManyAgentsWereBorned,
     new int[] { deathsHandler.HowManyAgentsDied, deathsHandler.HowManyAgentsWereEaten, deathsHandler.HowManyAgentsWereStarved, totalAgentsDied },
new float[] { timeDistribution.FoodTime, timeDistribution.BridgesTime, timeDistribution.HousesTime, timeDistribution.ProcreationTime },
new float[] { timeDistribution.AverageFoodTime, timeDistribution.AverageBridgesTime, timeDistribution.AverageHouseTime, timeDistribution.AverageProcreationTime });
        LogfileWriter.GetInstance().WriteFloodLine(daysPassed, "BeforeFlood");
    }

    internal void WriteLogAfterFlood()
    {
        LogfileWriter.GetInstance().WriteFloodLine(daysPassed, "AfterFlood");
        LogfileWriter.GetInstance().WriteLogline(daysPassed, totalPopulation, agentsCreator.HowManyAgentsWereBorned,
     new int[] { deathsHandler.HowManyAgentsDied, deathsHandler.HowManyAgentsWereEaten, deathsHandler.HowManyAgentsWereStarved, totalAgentsDied },
new float[] { timeDistribution.FoodTime, timeDistribution.BridgesTime, timeDistribution.HousesTime, timeDistribution.ProcreationTime },
new float[] { timeDistribution.AverageFoodTime, timeDistribution.AverageBridgesTime, timeDistribution.AverageHouseTime, timeDistribution.AverageProcreationTime });
        LogfileWriter.GetInstance().WriteFloodLine(daysPassed, "AfterFlood");
    }

    internal void EndGame()
    {
        UpdateScoreManager();

        ScoreBoardPanel.transform.parent.gameObject.SetActive(true);
        ScoreBoardPanel.SetActive(true);
    }
}