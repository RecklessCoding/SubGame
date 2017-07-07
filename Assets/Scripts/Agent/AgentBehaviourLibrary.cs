using System;
using UnityEngine;

public class AgentBehaviourLibrary : MonoBehaviour
{
    /** Memory of state */
    public int staminaLevel = 15;

    private bool canProcreate = true;

    private bool hasFood = false;

    public bool hasRock = false;

    public GameObject home;

    private AgentNavigator agentNavigator;

    public int PROCREATE_CHANCE = 5;

    public bool isHome;

    private bool isNight = false;
    private bool isGatheringFood = false;
    private bool isGatheringRock = false;
    private bool isGoingHome = false;
    private bool isBuildingBridge = false;
    private bool isBuildingHouses = false;
    private bool isGoingToProcreate = false;

    private int botNumber = 0;

    // Use this for initialization
    void Start()
    {
        agentNavigator = gameObject.GetComponent<AgentNavigator>();
        isHome = false;
        staminaLevel = UnityEngine.Random.Range(5, 22);
        if (staminaLevel > 15)
            staminaLevel = 15;
    }

    void Awake()
    {
        if (agentNavigator == null)
        {
            agentNavigator = gameObject.GetComponent<AgentNavigator>();
        }
    }
    /** ---- Status Updaters ---- */

    internal void SetBotNumber(int botNumber)
    {
        this.botNumber = botNumber;
    }

    internal void GetHungrier()
    {
        staminaLevel = staminaLevel - 1;
    }

    internal void BecomeFull()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0, 1);

        if (home != null)
            home.GetComponent<HouseScript>().UpdateAgentReproduction(true);
    }

    internal void BecomeAbleToProcreate()
    {
        canProcreate = true;
        GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
    }

    internal void BecomeUnableToProcreate()
    {
        canProcreate = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 0.92f, 0.016f, 1);
    }

    internal void BecomeHungry()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0.75f, 0.01f, 1);

        if (home != null)
            home.GetComponent<HouseScript>().UpdateAgentReproduction(false);
    }

    internal void BecomeStarving()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);

        if (home != null)
            home.GetComponent<HouseScript>().UpdateAgentReproduction(false);
    }

    internal void SetHome(GameObject house)
    {
        home = house;
    }

    /** -----------ACTIONS----------- */
    internal void GoToFood()
    {
        ABOD3_Bridge.GetInstance().AletForElement(botNumber, "GoToNearestFood", "A");

        isHome = false;
        isGatheringFood = true;
        agentNavigator.GoToFood();
    }

    internal void GatherFood()
    {
        GatherResource();
        hasFood = true;
        isGatheringFood = false;
    }

    internal void EatFood()
    {
        ABOD3_Bridge.GetInstance().AletForElement(botNumber, "EatFood", "A");

        staminaLevel = staminaLevel + 2;

        if (staminaLevel > 15)
            staminaLevel = 15;

        hasFood = false;
    }

    internal void GoToForest()
    {
        throw new NotImplementedException();
    }

    internal void GoToHome()
    {
        ABOD3_Bridge.GetInstance().AletForElement(botNumber, "GoHome", "A");

        isGoingHome = true;

        if (home != null)
            agentNavigator.GoHome(home);
    }

    internal void StayHome()
    {
        isHome = true;

        agentNavigator.StopWalking();
        isGoingToProcreate = false;
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
        ABOD3_Bridge.GetInstance().AletForElement(botNumber, "C-Procreate", "C");

        isGoingToProcreate = false;
        isHome = true;
        if (home != null)
            home.GetComponent<HouseScript>().UpdateAgentReproduction(canProcreate);

        if ((home.GetComponent<HouseScript>()).CanReproduce())
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-HasEnoughEnergy", "CE");
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "AttemptProcreation", "A");

            int dieRoll = UnityEngine.Random.Range(0, 100);
            staminaLevel--;
            if (dieRoll < PROCREATE_CHANCE)
            {
                staminaLevel--;

                AgentsCreator agentsCreator = transform.parent.gameObject.GetComponent("AgentsCreator") as AgentsCreator;
                agentsCreator.BornAgent(transform.position);
            }
        }
        else
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-NotEnoughEnergy", "CE");
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "StayHome", "A");
        }


        if (home != null)
            home.GetComponent<HouseScript>().UpdateAgentReproduction(false);
    }

    internal void GoToBridge()
    {
        isHome = false;
        isBuildingBridge = true;

        if (agentNavigator.GoToUnfinishedBridge() == null)
        {
            isBuildingBridge = false;

            agentNavigator.GoToBridges();
        }
    }

    internal void GoToRock()
    {
        ABOD3_Bridge.GetInstance().AletForElement(botNumber, "GoGetRock", "A");

        isHome = false;
        isGatheringRock = true;

        agentNavigator.GoToRocks();
    }

    internal void GatherRock()
    {
        isHome = false;

        hasRock = true;
        GatherResource();
    }

    internal void BuildBridge()
    {
        isHome = false;
        hasRock = false;

        agentNavigator.StopWalking();
        IsBuildingBridge = false;
    }

    internal void BuildHouse()
    {
        isHome = true;
        hasRock = false;
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
        if (7 <= staminaLevel)
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
        if (0 < staminaLevel && staminaLevel <= 2)
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
        if (home != null)
        {
            return (home.CompareTag("HouseNotBuiltAvailable") || home.CompareTag("HouseNotBuiltFull"));
        }
        else
        {
            return false;
        }
    }
}
