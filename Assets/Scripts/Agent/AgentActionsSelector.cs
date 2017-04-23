﻿using UnityEngine;

public class AgentActionsSelector : MonoBehaviour
{
    public float nextStaminaUpdate = 0;

    public float staminaUpdateTime = 0;

    private int dateBorn = 0;

    private int maxLife = 0;

    private GameObject agentsManager;

    private Color defaultColor;

    private TimeDistribution timeDistribution;

    private AgentBehaviourLibrary agentBehaviours;

    private int worksIndex = 0;
    private float nextWorkUpdate = -1;
    private bool isNight = false;

    // Use this for initialization
    void Start()
    {
        agentsManager = transform.parent.gameObject;
        agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();

        timeDistribution = gameObject.GetComponent("TimeDistribution") as TimeDistribution;
        dateBorn = timeDistribution.DaysPassed;
        staminaUpdateTime = timeDistribution.TimeInDay;

        SetRandomAge();
        defaultColor = new Color(0, 1, 1, 1);
        GetComponent<SpriteRenderer>().color = defaultColor;

        worksIndex = Random.Range(-1, 2) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agentBehaviours)
        {
            agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();
        }

        ChangeWorkIndex();
        CheckIfAlive();
        UpdateStamina();
        ActionSelection();
    }

    void OnTriggerEnter(Collider collidedObject)
    {
        try
        {
            if ((collidedObject.gameObject != null) && (gameObject != null) && (agentBehaviours != null))
            {
                if (collidedObject.gameObject.tag.Equals("Food") && agentBehaviours.IsGatheringFood)
                {
                    agentBehaviours.GatherFood();
                }
                else if (collidedObject.gameObject.tag.Equals("Rock") && agentBehaviours.IsGatheringRock)
                {
                    agentBehaviours.GatherRock();
                }
                else if (collidedObject.gameObject.tag.Equals("BridgeNotAvailable") && agentBehaviours.IsBuildingBridge)
                {
                    agentBehaviours.BuildBridge();
                }
                else if (collidedObject.gameObject.Equals(agentBehaviours.Home) && agentBehaviours.IsGoingHome)
                {
                    CGotHome();
                }
            }
        }
        catch (System.Exception e) // REALLY BAD FIX!
        {
            Debug.LogError(e);
        }
    }

    internal void IsNight()
    {
        nextWorkUpdate = nextWorkUpdate + timeDistribution.NightLength;
        isNight = true;
    }

    internal void IsDay()
    {
        isNight = false;
    }

    private void ActionSelection()
    {
        if (agentBehaviours.IsStarving())
        {
            agentBehaviours.GoToFood();
        }

        if (isNight)
        {
            agentBehaviours.GoToHome();
        }
        else
        {
            switch (worksIndex)
            {
                case 0:
                    agentBehaviours.GoToFood();
                    break;
                case 1:
                    agentBehaviours.GoToBridge();
                    break;
                case 2:
                    CHomeBuilding();
                    break;
                case 3:
                    CTryToProcreate();
                    break;
            }
        }
    }

    private void CTryToProcreate()
    {
        if (agentBehaviours.HasHomeNotBuilt())
        {
            CHomeBuilding();
        }
        else
        {
            agentBehaviours.GoToProcreate();
            agentBehaviours.GoToHome();
        }
    }

    private void ChangeWorkIndex()
    {
        if (Time.time >= nextWorkUpdate) // Time to pick a new work
        {
            worksIndex++;

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
                case 4:
                    worksIndex = 0;
                    break;
            }
        }
    }

    private float UpdateTime(float timeIncrease, float nextWorkUpdate)
    {
        if (timeIncrease == 0)
        {
            return 0;
        }
        else
        {
            int timeIncreaseAsInt = Mathf.FloorToInt(timeIncrease);
            nextWorkUpdate = Mathf.FloorToInt(Time.time) + timeIncreaseAsInt;

            return nextWorkUpdate;
        }
    }

    private void CHomeBuilding()
    {
        if (agentBehaviours.HasHomeNotBuilt())
        {
            if (agentBehaviours.HasRock())
            {
                agentBehaviours.GoToHome();
            }
            else
            {
                agentBehaviours.GoToRock();
            }
        }
        else
        {
            agentBehaviours.GoToHome();
        }
    }

    private void CGotHome()
    {
        agentBehaviours.IsGoingHome = false;

        if (agentBehaviours.HasHomeNotBuilt())
        {
            if (agentBehaviours.HasRock())
            {
                agentBehaviours.BuildHouse();
            }
        }
        else
        {
            if (agentBehaviours.IsGoingToProcreate)
            {
                agentBehaviours.Procreate();
            }
            else
            {
                agentBehaviours.StayHome();
            }
        }
    }

    private void CEatFood()
    {
        if (agentBehaviours.HasFood())
        {
            agentBehaviours.EatFood();
        }
        else
        {
            agentBehaviours.GoToFood();
        }
    }

    private void CBuildBridge()
    {
        if (agentBehaviours.HasRock())
        {
            agentBehaviours.GoToBridge();
        }
        else
        {
            agentBehaviours.GoToRock();
        }
    }

    private void CBuildHouse()
    {
        if (agentBehaviours.HasHouse())
        {
            if (agentBehaviours.HasRock())
            {
                agentBehaviours.GoToBridge();
            }
            else
            {
                agentBehaviours.GoToRock();
            }
        }
    }

    private void UpdateStamina()
    {
        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + staminaUpdateTime;             // Change the next update (current second+1)
            agentBehaviours.GetHungrier();
        }

        if (agentBehaviours.IsFull())
        {
            agentBehaviours.BecomeFull();
        }
        else if (agentBehaviours.HasStaminaToProcreate())
        {
            agentBehaviours.BecomeAbleToProcreate();
        }
        else if (!agentBehaviours.HasStaminaToProcreate())
        {
            agentBehaviours.BecomeUnableToProcreate();
        }
        else if (agentBehaviours.IsHungry())
        {
            agentBehaviours.BecomeHungry();
        }
        else if (agentBehaviours.IsStarving())
        {
            agentBehaviours.BecomeStarving();
        }
        else if (agentBehaviours.IsDead())
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentStaved(timeDistribution.DaysPassed - dateBorn);

            KillItself();
        }
    }

    private void CheckIfAlive()
    {
        if (Time.time >= maxLife)
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentDied(timeDistribution.DaysPassed - dateBorn);

            KillItself();
        }
    }

    internal void GotEaten()
    {
        AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
        deathsHandler.AgentWasEaten(timeDistribution.DaysPassed - dateBorn);

        KillItself();
    }

    private void KillItself()
    {
        if (agentBehaviours.Home)
        {
            HouseScript home = agentBehaviours.Home.GetComponent("HouseScript") as HouseScript;
            home.RemoveAgent();
        }

        Destroy(gameObject);
    }

    private void SetRandomAge()
    {
        int timeInDay = Mathf.FloorToInt(timeDistribution.TimeInDay);

        maxLife = Random.Range(timeInDay, timeInDay * 15);
    }
}
