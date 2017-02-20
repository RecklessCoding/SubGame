using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgentsCreator : MonoBehaviour
{
    public GameObject agent;

    public GameObject populationCounter;

    public GameObject infantsCounter;

    public GameObject deathsCounter;

    public int agentsCount = 0;

    public int infantsCount = 0;

    public int deathsCount = 0;

    private const int INITIAL_POPULATION = 200; 

    private Text populationCounterTextbox;

    private Text infantsCounterTextbox;

    private Text deathsCounterTextbox;

    // Use this for initialization
    void Start()
    {
        Transform posBoundaries = GameObject.Find("HousesManager").transform;
        Vector3 pos;

        populationCounterTextbox = populationCounter.GetComponent<Text>();

        infantsCounterTextbox = infantsCounter.GetComponent<Text>();

        deathsCounterTextbox = deathsCounter.GetComponent<Text>();

        for (int i = 0; i < INITIAL_POPULATION; i++)
        {
            pos = GetRandomPos(posBoundaries);

            SpawnAgent(pos);
        }
    }

    void Update()
    {
        agentsCount = gameObject.transform.childCount;
        populationCounterTextbox.text = agentsCount.ToString();

        infantsCounterTextbox.text = infantsCount.ToString();

        deathsCount = (INITIAL_POPULATION + infantsCount) - agentsCount;
        deathsCounterTextbox.text = deathsCount.ToString();
    }

    internal void BornAgent(Vector3 pos)
    {
        infantsCount++;
        SpawnAgent(pos);
    }

    private void SpawnAgent(Vector3 pos)
    {
        GameObject newAgent = Instantiate(agent, pos, Quaternion.Euler(90, 0, 0)) as GameObject;
        newAgent.transform.SetParent(gameObject.transform);
    }

    private Vector3 GetRandomPos(Transform posBoundaries)
    {
        float minX = posBoundaries.position.x - (posBoundaries.localScale.x / 2);
        float maxX = posBoundaries.position.x + (posBoundaries.localScale.x / 2);

        float minZ = posBoundaries.position.z - (posBoundaries.localScale.z / 2);
        float maxZ = posBoundaries.position.z + (posBoundaries.localScale.z / 2);

        Vector3 newVec = new Vector3(Random.Range(minX, maxX),
                                     0,
                                     Random.Range(minZ, maxZ));
        return newVec;
    }
}
