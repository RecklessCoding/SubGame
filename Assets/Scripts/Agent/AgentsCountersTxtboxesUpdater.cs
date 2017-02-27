using UnityEngine;
using UnityEngine.UI;

public class AgentsCountersTxtboxesUpdater : MonoBehaviour
{
    public GameObject populationCounter;
    public GameObject infantsCounter;
    public GameObject eatenCounter;
    public GameObject starvedCounter;
    public GameObject deathsCounter;

    private Text populationCounterTextbox;
    private Text infantsCounterTextbox;
    private Text eatenCounterTextbox;
    private Text starvedCounterTextbox;
    private Text deathsCounterTextbox;

    private AgentsCreator agentsCreator;
    private AgentsDeathsHandler deathsHandler;

    // Use this for initialization
    void Start()
    {
        agentsCreator = gameObject.GetComponent("AgentsCreator") as AgentsCreator;
        deathsHandler = gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;

        populationCounterTextbox = populationCounter.GetComponent<Text>();

        infantsCounterTextbox = infantsCounter.GetComponent<Text>();

        eatenCounterTextbox = eatenCounter.GetComponent<Text>();
        starvedCounterTextbox = starvedCounter.GetComponent<Text>();
        deathsCounterTextbox = deathsCounter.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        populationCounterTextbox.text = agentsCreator.HowManyAgentsAreALive.ToString();
        infantsCounterTextbox.text = agentsCreator.HowManyAgentsWereBorned.ToString();

        eatenCounterTextbox.text = deathsHandler.HowManyAgentsWereEaten.ToString();
        starvedCounterTextbox.text = deathsHandler.HowManyAgentsWereStarved.ToString();
        deathsCounterTextbox.text = deathsHandler.HowManyAgentsDied.ToString();
    }
}