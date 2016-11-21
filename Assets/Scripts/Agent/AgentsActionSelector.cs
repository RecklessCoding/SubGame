using UnityEngine;
using System.Collections;
using System;

public class AgentsActionSelector : MonoBehaviour
{

    private int nextWorkUpdate = 0;

    /** Food, Bridges, House */
    private int worksIndex = 1;

    private bool isNight = false;
    // Update is called once per frame
    void Update()
    {
        UpdateWorkList();
    }

    public void ReStrategise()
    {
        nextWorkUpdate = -1;

        UpdateWorkList();
    }

    private void UpdateWorkList()
    {
        if (Time.time >= nextWorkUpdate) // Time to pick a new work
        {
            TimeDistribution timeDistribution = gameObject.GetComponent("TimeDistribution") as TimeDistribution;

            switch (worksIndex)
            {
                case 0:
                    int foodTime = Mathf.FloorToInt(timeDistribution.FoodTime);
                    UpdateTime(foodTime);
                    GoGatherFood();
                    break;
                case 1:
                    int bridgesTime = Mathf.FloorToInt(timeDistribution.BridgesTime);
                    UpdateTime(bridgesTime);
                    GoBuildBridge();
                    break;
                case 2:
                    int housesTime = Mathf.FloorToInt(timeDistribution.HousesTime);
                    UpdateTime(housesTime);
                    GoToHouse();
                    break;
                case 3:
                    int restHouseTime = Mathf.FloorToInt(timeDistribution.RestHousesTime);
                    UpdateTime(restHouseTime);
                    GoToHouse();
                    break;
            }

            worksIndex++;
            if (worksIndex >= 3) // Completed all possible work orders
            {
                worksIndex = 0;
            }
        }
    }

    private void CheckIfNight() {
        if (isNight)
        {
            nextWorkUpdate = -1;
            worksIndex = 3;
        } 
    }


    private void GoGatherFood()
    {
        AlertAgents();
    }


    private void GoBuildBridge()
    {
        AlertAgents();
    }


    private void GoToHouse()
    {
        AlertAgents();
    }

    private void AlertAgents()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");

        foreach (GameObject agent in agents)
        {
            Agent agentScript = agent.GetComponent("Agent") as Agent;
            agentScript.StarNewWork(worksIndex);
        }
    }

    private void UpdateTime(int timeIncrease)
    {
        if (timeIncrease == 0)
        {
            return;
        }
        else
        {
            nextWorkUpdate = Mathf.FloorToInt(Time.time) + timeIncrease;
            return;
        }
    }
}
