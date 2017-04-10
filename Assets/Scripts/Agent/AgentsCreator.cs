using UnityEngine;

public class AgentsCreator : MonoBehaviour
{
    public GameObject agent;

    private const int INITIAL_POPULATION = 200;

    private int infantsCount = 0;

    void Start()
    {
        Transform posBoundaries = GameObject.Find("HousesManager").transform;
        Vector3 pos;

        for (int i = 0; i < INITIAL_POPULATION; i++)
        {
            pos = GetRandomPos(posBoundaries);

            SpawnAgent(pos);
        }
    }

    void Update()
    {

    }

    internal void MigratePopulation(int numberOfImmigrants)
    {
        Transform posBoundaries = GameObject.Find("HousesManager").transform;
        Vector3 pos;

        for (int i = 0; i < numberOfImmigrants; i++)
        {
            pos = GetRandomPos(posBoundaries);

            SpawnAgent(pos);
        }
    }

    internal int HowManyAgentsWereBorned
    {
        get
        {
            return infantsCount;
        }
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
