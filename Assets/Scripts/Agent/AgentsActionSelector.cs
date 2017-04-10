using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class AgentsActionSelector : MonoBehaviour
{
    private bool isNight = false;

    private GameObject[] agents;

    public GameObject[] agentsGroup1;
    private int nextWorkUpdateGroup1 = 0;
    private int worksIndex1 = 0;

    public GameObject[] agentsGroup2;
    private int nextWorkUpdateGroup2 = 0;
    private int worksIndex2 = 1;

    public GameObject[] agentsGroup3;
    private int nextWorkUpdateGroup3 = 0;
    private int worksIndex3 = 1;

    public GameObject[] agentsGroup4;
    private int nextWorkUpdateGroup4 = 0;
    private int worksIndex4 = 2;

    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("Agent");
        ReStrategise();

        AlertAgents(agentsGroup1, worksIndex1);
        AlertAgents(agentsGroup2, worksIndex2);
        AlertAgents(agentsGroup3, worksIndex3);
        AlertAgents(agentsGroup4, worksIndex4);
    }

    void Update()
    {
        getAgents();

        nextWorkUpdateGroup1 = UpdateWorkList(agentsGroup1, nextWorkUpdateGroup1, ref worksIndex1);
        nextWorkUpdateGroup2 = UpdateWorkList(agentsGroup2, nextWorkUpdateGroup2, ref worksIndex2);
        nextWorkUpdateGroup3 = UpdateWorkList(agentsGroup3, nextWorkUpdateGroup3, ref worksIndex3);
        nextWorkUpdateGroup4 = UpdateWorkList(agentsGroup4, nextWorkUpdateGroup4, ref worksIndex4);
    }

    internal void ReStrategise()
    {
        getAgents();

        nextWorkUpdateGroup1 = UpdateWorkList(agentsGroup1, -1, ref worksIndex1);
        nextWorkUpdateGroup2 = UpdateWorkList(agentsGroup2, -1, ref worksIndex2);
        nextWorkUpdateGroup3 = UpdateWorkList(agentsGroup3, -1, ref worksIndex3);
        nextWorkUpdateGroup4 = UpdateWorkList(agentsGroup4, -1, ref worksIndex4);
    }


    internal void IsNight()
    {
        isNight = true;

        worksIndex1 = 3;
        worksIndex2 = 3;
        worksIndex3 = 3;
        worksIndex4 = 3;

        nextWorkUpdateGroup1 = UpdateWorkList(agentsGroup1, -1, ref worksIndex1);
        nextWorkUpdateGroup2 = UpdateWorkList(agentsGroup2, -1, ref worksIndex2);
        nextWorkUpdateGroup3 = UpdateWorkList(agentsGroup3, -1, ref worksIndex3);
        nextWorkUpdateGroup4 = UpdateWorkList(agentsGroup4, -1, ref worksIndex4);

    }

    internal void IsDay()
    {
        isNight = false;

        ReStrategise();
    }

    private void getAgents()
    {
        GameObject[] tempAgents = GameObject.FindGameObjectsWithTag("Agent");
        if (tempAgents.Length > 0)
        {
            agents = agents.Concat(tempAgents.Except(agents)).ToArray();

            SplitMidPoint(agents, out agentsGroup1, out agentsGroup2);
            SplitMidPoint(agentsGroup1, out agentsGroup1, out agentsGroup3);
            SplitMidPoint(agentsGroup2, out agentsGroup2, out agentsGroup4);
        }
    }

    internal void ChangeSpeed()
    {
        ReStrategise();
        ReStrategise();
        ReStrategise();

    }

    private int UpdateWorkList(GameObject[] agentsArray, int nextWorkUpdate, ref int worksIndex)
    {
        if (Time.time >= nextWorkUpdate) // Time to pick a new work
        {
            TimeDistribution timeDistribution = gameObject.GetComponent("TimeDistribution") as TimeDistribution;

            worksIndex++;
            if (worksIndex >= 3)
            {
                if (!isNight) // Completed all possible work orders
                {
                    worksIndex = 0;
                }
                else
                {
                    worksIndex = 3;
                }
            }

            switch (worksIndex)
            {
                case 0:
                    nextWorkUpdate = UpdateTime(timeDistribution.FoodTime, nextWorkUpdate);
                    break;
                case 1:
                    nextWorkUpdate = UpdateTime(timeDistribution.BridgesTime, nextWorkUpdate);
                    break;
                case 2:
                    nextWorkUpdate = UpdateTime(timeDistribution.HousesTime, nextWorkUpdate);
                    break;
                case 3:
                    nextWorkUpdate = UpdateTime(timeDistribution.RestHousesTime, nextWorkUpdate);
                    break;
            }
            AlertAgents(agentsArray, worksIndex);

            return nextWorkUpdate;
        }

        return nextWorkUpdate;
    }

    private void CheckIfNight()
    {
        if (isNight)
        {
            worksIndex1 = 3;
            worksIndex2 = 3;
            worksIndex3 = 3;
            worksIndex4 = 3;

            nextWorkUpdateGroup1 = UpdateWorkList(agentsGroup1, -1, ref worksIndex1);
            nextWorkUpdateGroup2 = UpdateWorkList(agentsGroup2, -1, ref worksIndex2);
            nextWorkUpdateGroup3 = UpdateWorkList(agentsGroup3, -1, ref worksIndex3);
            nextWorkUpdateGroup4 = UpdateWorkList(agentsGroup4, -1, ref worksIndex4);
        }
    }

    private void AlertAgents(GameObject[] agentsArray, int worksIndex)
    {
        foreach (GameObject agent in agentsArray)
        {
            if (agent != null)
            {
                Agent agentScript = agent.GetComponent("Agent") as Agent;
                agentScript.StarNewWork(worksIndex);
            }
        }
    }

    private int UpdateTime(float timeIncrease, int nextWorkUpdate)
    {
        int timeIncreaseAsInt = Mathf.FloorToInt(timeIncrease);

        if (timeIncrease == 0)
        {
            return 0;
        }
        else
        {
            nextWorkUpdate = Mathf.FloorToInt(Time.time) + timeIncreaseAsInt;
            return nextWorkUpdate;
        }
    }

    private void Split<T>(T[] array, int index, out T[] first, out T[] second)
    {
        first = array.Take(index).ToArray();
        second = array.Skip(index).ToArray();
    }

    private void SplitMidPoint<T>(T[] array, out T[] first, out T[] second)
    {
        Split(array, array.Length / 2, out first, out second);
    }
}
