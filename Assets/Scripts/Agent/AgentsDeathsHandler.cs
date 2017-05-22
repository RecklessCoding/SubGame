using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsDeathsHandler : MonoBehaviour
{
    private int drownedCount = 0;

    private int eatenCount = 0;

    private int starvedCount = 0;

    private int deathsCount = 0;
        
    private int totalDeathsCount = 0;

    private float averageDaysAgentsLived = 0;

    private float totalDaysAgentsLived = 0;

    private AgentsManager agentsManager;

    // Use this for initialization
    void Start()
    {
        agentsManager = gameObject.GetComponent("AgentsManager") as AgentsManager;
    }

    // Update is called once per frame
    void Update()
    {
        totalDeathsCount = eatenCount + starvedCount + deathsCount;
        averageDaysAgentsLived = totalDaysAgentsLived / (totalDeathsCount);
    }

    internal void AgentWasEaten(float daysAgentLived)
    {
        totalDaysAgentsLived = totalDaysAgentsLived + daysAgentLived;
        eatenCount = eatenCount + 1;
    }


    internal void AgentWasDrowned(float daysAgentLived)
    {
        totalDaysAgentsLived = totalDaysAgentsLived + daysAgentLived;
        drownedCount = drownedCount + 1;
    }


    internal void AgentStaved(float daysAgentLived)
    {
        totalDaysAgentsLived = totalDaysAgentsLived + daysAgentLived;
        starvedCount = starvedCount + 1;
    }

    internal void AgentDied(float daysAgentLived)
    {
        totalDaysAgentsLived = totalDaysAgentsLived + daysAgentLived;
        deathsCount = deathsCount + 1;
    }

    internal int HowManyAgentsWereStarved
    {
        get
        {
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

    internal float HowManyDaysAgentsLived
    {
        get
        {
            return averageDaysAgentsLived;
        }
    }
}
