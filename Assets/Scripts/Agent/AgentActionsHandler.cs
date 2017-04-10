using UnityEngine;
using System.Collections;
using System;

public class AgentActionsHandler
{
    private NavMeshAgentPath agentPathfinding;

    private AgentResourcesManager agentResources;

    private int nextAvailability = 0;

    private const int RESOURCE_DELAY = 2;

    private const int BUILD_DELAY = 5;

    private bool isBusy = false;

    private bool isBuildingBridges = false;

    private bool isBuildingHouses = false;

    private bool isGatheringFood = false;

    private bool isGatheringRock = false;

    private bool isGoingHome = false;

    private int workIndex = 0;

    internal AgentActionsHandler(NavMeshAgentPath agentPathfinding, AgentResourcesManager agentResources)
    {
        this.agentPathfinding = agentPathfinding;
        this.agentResources = agentResources;
    }

    internal bool IsBuildingBridges
    {
        get
        {
            return isBuildingBridges;
        }
        set
        {
            isBuildingBridges = value;
        }
    }

    internal bool IsBuildingHouses
    {
        get
        {
            return isBuildingHouses;
        }
        set
        {
            isBuildingHouses = value;
        }
    }

    internal bool IsGatheringFood
    {
        get
        {
            return isGatheringFood;
        }
    }

    internal bool IsGatheringRock
    {
        get
        {
            return isGatheringRock;
        }
    }

    internal bool IsGoingHome
    {
        get
        {
            return isGoingHome;
        }
        set
        {
            isGoingHome = value;
        }
    }

    public object Go { get; internal set; }

    internal void Build()
    {
        isBusy = true;
        agentResources.DecreaseRocks();
        isBuildingBridges = false;
        isBuildingHouses = false;

        nextAvailability = Mathf.FloorToInt(Time.time) + BUILD_DELAY;
    }

    internal void PerformActionSelection()
    {
        UpdateAvailability();
        PickAction();
    }

    internal void StarNewWork(int workIndex)
    {
        this.workIndex = workIndex;
        PerformActionSelection();
    }

    internal void GatherResource()
    {
        isBusy = true;
        nextAvailability = Mathf.FloorToInt(Time.time) + RESOURCE_DELAY;
        isGatheringFood = false;
        isGatheringRock = false;
    }

    private void UpdateAvailability()
    {
        if (Time.time >= nextAvailability)
        {
            isBusy = false;
        }
    }

    private void PickAction()
    {
        if (!isBusy)
        {
            switch (workIndex)
            {
                case 0:
                    GoGatherFood();
                    break;
                case 1:
                    GoBuildBridge();
                    break;
                case 2:
                    GoHome();
                    break;
                case 3:
                        GoHome();
                    break;
            }
        }
    }

    internal void Eat()
    {
        agentResources.DecreaseFood();
        agentResources.IncreaseStamina();
    }

    internal void GoGatherFood()
    {
        isGatheringFood = true;
        agentPathfinding.GoToFood();
    }

    private void GoBuildBridge()
    {
        if (agentResources.HasRocks())
        {
            GoToUnfinishedBridge();
        }
        else
        {
            GoToRocks();
        }
    }


    private void GoToUnfinishedBridge()
    {
        isBuildingBridges = true;
        if (agentPathfinding.GoToUnfinishedBridge() == null)
        {
            //TODO:
        }
    }

    private void GoHome()
    {
        if (agentResources.HasHomeBuilt())
        {
            GoToHome();
        }
        else if (agentResources.HasHomeNotBuilt())
        {
            GoBuildHome();
        }
        else
        {
            FindHouse();
        }
    }

    private void GoToRocks()
    {
        isGatheringRock = true;
        agentPathfinding.GoToRocks();
    }

    private void GoBuildHome()
    {
        if (agentResources.HasRocks())
        {
            GoToHome();
        }
        else
        {
            GoToRocks();
        }
    }

    private void GoToHome()
    {
        isGoingHome = true;
        agentPathfinding.GoHome(agentResources.Home);
    }

    private void FindHouse()
    {
        GameObject house;
        GameObject[] housesAvailable = GameObject.FindGameObjectsWithTag("HouseNotBuiltAvailable");
        if (housesAvailable.Length > 0)
        {
            house = agentPathfinding.FindNearestObject(housesAvailable).gameObject;
            AllocateHouse(house);
        }

        if (!agentResources.HasHome())
        {
            housesAvailable = GameObject.FindGameObjectsWithTag("HouseBuiltAvailable");
            if (housesAvailable.Length > 0)
            {
                house = agentPathfinding.FindNearestObject(housesAvailable).gameObject;
                AllocateHouse(house);
            }
        }
    }


    private bool AllocateHouse(GameObject house)
    {
        HouseScript houseScript = house.GetComponent("HouseScript") as HouseScript;
        if (houseScript.AllocateAgent())
        {
            agentResources.SetHome(house);

            return true;
        }
        return false;
    }
}