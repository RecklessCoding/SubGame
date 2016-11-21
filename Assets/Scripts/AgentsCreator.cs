using UnityEngine;
using System.Collections;

public class AgentsCreator : MonoBehaviour {

    public GameObject agent;

	// Use this for initialization
	void Start () {

        Transform posBoundaries = GameObject.Find("HousesManager").transform;
        Vector3 pos;

        for (int i = 0; i < 200; i++)
        {
            pos = GetRandomPos(posBoundaries);

            Instantiate(agent, pos, Quaternion.Euler(90, 0, 0));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
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
