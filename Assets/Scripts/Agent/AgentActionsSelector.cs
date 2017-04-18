using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionSelector : MonoBehaviour
{

    public float nextStaminaUpdate = 0;

    public float staminaUpdateTime = 0;

    private AgentResourcesManager agentResources = new AgentResourcesManager();

    private AgentBehaviourLibrary agentBehaviours = new AgentBehaviourLibrary();

    private bool canProcreate = true;

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);

    }

    // Update is called once per frame
    void Update()
    {

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

            return nextWorkUpdate;
        }

        return nextWorkUpdate;
    }


    private void UpdateStamina()
    {
        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + staminaUpdateTime;             // Change the next update (current second+1)
            GetHungrier();
        }

        if (agentResources.Stamina == 15)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
        }

        if (agentResources.Stamina <= 10)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
            canProcreate = false;
        }

        if (agentResources.Stamina <= 4)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
        }

        if (agentResources.Stamina <= 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);

            agentBehaviours.GoGatherFood();
        }
    }

    private void Tired()
    {
        agentResources.DecreaseStamina();
        if (agentResources.Stamina == 0)
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentStaved((agentsManager.GetComponent("TimeDistribution") as TimeDistribution).DaysPassed - dateBorn);

            KillItself();
        }
    }
}
