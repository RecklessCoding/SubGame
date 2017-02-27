using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsDeathsHandler : MonoBehaviour {

    private int eatenCount;

    private int starvedCount;

    private int deathsCount;

    private int totalDeathsCount;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        totalDeathsCount = eatenCount + starvedCount + deathsCount;
    }

    internal void AgentWasEaten()
    {
        eatenCount++;
    }

    internal void AgentStaved()
    {
        starvedCount++;
    }

    internal void AgentDied()
    {
        deathsCount++;
    }

    internal int HowManyAgentsWereStarved
    {
        get {
            return starvedCount;
        }
    }

    internal int HowManyAgentsWereEaten
    {
        get
        {
            return eatenCount;
        }
    }

    internal int HowManyAgentsDied
    {
        get
        {
            return deathsCount;
        }
    }
}
