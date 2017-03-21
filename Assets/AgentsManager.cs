using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    private int agentsCount = 0;

    void Update()
    {
        agentsCount = gameObject.transform.childCount;

        if (agentsCount == 0)
        {
            // game over!
            (transform.GetComponent("TimeDistribution") as TimeDistribution).EndGame();

            (transform.GetComponent("AgentsCountersTxtboxesUpdater") as AgentsCountersTxtboxesUpdater).EndGame();

        }
    }

    internal void ChangeSpeed(float factor)
    {
        Agent[] agents = GetAllAgents();

        foreach (Agent agent in agents)
        {
            agent.ChangeSpeed(factor);
        }
    }

    internal int HowManyAgentsAreALive
    {
        get
        {
            return agentsCount;
        }
    }

    private Agent[] GetAllAgents()
    {
        return gameObject.GetComponentsInChildren<Agent>(); ;
    }
}