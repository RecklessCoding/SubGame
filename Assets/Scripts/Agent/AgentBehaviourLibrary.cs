using System;
using UnityEngine;

public class AgentBehaviourLibrary : MonoBehaviour
{
    /** Memory of state */
    private int staminaLevel = 15;

    private bool canProcreate = true;

    private bool hasFood = false;

    private bool hasRock = false;

    private GameObject home;

    private AgentNavigator agentNavigator;

    private const int PROCREATE_CHANCE = 5;

    private bool isNight = false;
    private bool isGatheringFood;
    private bool isGatheringRock;
    private bool isGoingHome;
    private bool isBuildingBridge;
    private bool isBuildingHouses;
    private bool isGoingToProcreate;

    // Use this for initialization
    void Start()
    {

    }


    /** ---- Status Updaters ---- */

    internal void GetHungrier()
    {
        staminaLevel = staminaLevel - 1;
    }

    internal void BecomeFull()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
    }

    internal void BecomeAbleToProcreate()
    {
        canProcreate = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
    }

    internal void BecomeUnableToProcreate()
    {
        canProcreate = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
    }

    internal void BecomeHungry()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
    }

    internal void BecomeStarving()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
    }

    internal void SetHome(GameObject house)
    {
        home = house;
    }

    /** -----------ACTIONS----------- */
    internal void GoToFood()
    {
        agentNavigator.GoToFood();
    }

    internal void GatherFood()
    {
        GatherResource();
        hasFood = true;
    }

    internal void EatFood()
    {
        if (hasFood)
        {
            staminaLevel++;
            hasFood = false;
        }
    }

    internal void GoToForest()
    {
        throw new NotImplementedException();
    }

    internal void GoToHome()
    {
        isGoingHome = true;

        if (home != null)
            agentNavigator.GoHome(home);
    }

    internal void StayHome()
    {
        agentNavigator.StopWalking();
    }

    internal void GoToProcreate()
    {
        if (canProcreate)
        {
            isGoingToProcreate = true;
        }
    }

    internal void Procreate()
    {
        if ((home.GetComponent<HouseScript>()).CanReproduce())
        {
            int dieRoll = UnityEngine.Random.Range(1, 100);

            if (dieRoll < PROCREATE_CHANCE)
            {
                AgentsCreator agentsCreator = transform.parent.gameObject.GetComponent("AgentsCreator") as AgentsCreator;
                agentsCreator.BornAgent(transform.position);
            }
        }
    }

    internal void GoToBridge()
    {
        agentNavigator.GoToUnfinishedBridge();
        isBuildingBridge = true;
    }

    internal void GoToRock()
    {
        agentNavigator.GoToRocks();
    }

    internal void GatherRock()
    {
        GatherResource();
        hasRock = true;
    }

    internal void BuildBridge()
    {
        agentNavigator.StopWalking();
        IsBuildingBridge = false;
    }

    internal void BuildHouse()
    {
        agentNavigator.StopWalking();
        IsBuildingHouses = false;
    }

    private void GatherResource()
    {
        agentNavigator.StopWalking();
    }

    /** -----------SENSES----------- */

    internal bool HasRock()
    {
        return hasRock;
    }

    internal bool HasFood()
    {
        return hasFood;
    }

    internal bool CanProcreate()
    {
        return canProcreate;
    }

    internal bool IsFull()
    {
        if (staminaLevel == 15)
            return true;
        else
            return false;
    }

    internal bool HasStaminaToProcreate()
    {
        if (10 <= staminaLevel)
            return true;
        else
            return false;
    }

    internal bool IsHungry()
    {
        if ((2 < staminaLevel) && (staminaLevel <= 4))
            return true;
        else
            return false;
    }

    internal bool IsStarving()
    {
        if (staminaLevel <= 2)
            return true;
        else
            return false;
    }

    internal bool IsDead()
    {
        return (staminaLevel <= 0);
    }


    internal bool HasHouse()
    {
        return true;
    }


    internal bool IsNight()
    {
        return isNight;
    }


    internal bool IsBuildingBridge
    {
        get
        {
            return isBuildingBridge;
        }
        set
        {
            isBuildingBridge = value;
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

    internal bool IsGoingToProcreate
    {
        get
        {
            return isGoingToProcreate;
        }

        set
        {
            isGoingToProcreate = value;
        }
    }

    internal GameObject Home
    {
        get
        {
            return home;
        }
    }

    internal bool HasHomeNotBuilt()
    {
        if (home)
        {
            return home.CompareTag("HouseNotBuiltAvailable") || home.CompareTag("HouseNotBuiltFull");
        }
        else
        {
            return false;
        }
    }
}
