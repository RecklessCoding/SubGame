using UnityEngine;

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

    public int worksIndex = -1;
    private float nextWorkUpdate = -1;
    public bool isNight = false;
    public bool canBeEaten = false;

    // Use this for initialization
    void Start()
    {
        agentsManager = transform.parent.gameObject;
        agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();

        timeDistribution = gameObject.transform.parent.GetComponent("TimeDistribution") as TimeDistribution;
        dateBorn = timeDistribution.DaysPassed;

        nextStaminaUpdate = timeDistribution.TimeInDay;
        staminaUpdateTime = timeDistribution.TimeInDay / 2;

        SetRandomAge();
        defaultColor = new Color(0, 1, 1, 1);
        GetComponent<SpriteRenderer>().color = defaultColor;

        worksIndex = Random.Range(-2, 3) + 1;

        FindHouse();
    }

    // Update is called once per frame
    void Update()
    {
        if (agentBehaviours.Home == null)
        {
            FindHouse();
        }
        if (!agentBehaviours)
        {
            agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();
        }

        ChangeWorkIndex();
        CheckIfAlive();
        UpdateStamina();
        ActionSelection();
    }

    void Awake()
    {
        if (agentBehaviours == null)
        {
            agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();
        }

    }

    void OnTriggerEnter(Collider collidedObject)
    {
        try
        {
            if ((collidedObject.gameObject != null) && (gameObject != null) && (agentBehaviours != null))
            {
                if (collidedObject.gameObject.Equals(agentBehaviours.Home) && isNight)
                {
                    agentBehaviours.StayHome();
                    canBeEaten = false;
                }

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

        if (!agentBehaviours.isHome)
            canBeEaten = true;
    }

    internal void IsDay()
    {
        isNight = false;
        canBeEaten = false;
    }

    internal bool CanBuildBridge()
    {
        if (!agentBehaviours)
        {
            agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();
        }

        return agentBehaviours.HasRock() && agentBehaviours.IsBuildingBridge;
    }

    internal bool CanBuildHouse()
    {
        if (!agentBehaviours)
        {
            agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();
        }

        return agentBehaviours.HasRock() && agentBehaviours.IsGoingHome;
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
                    CEatFood();
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
                    if (timeDistribution.FoodTime > 0)
                    {
                        nextWorkUpdate = UpdateTime(timeDistribution.FoodTime);
                        break;
                    }
                    else
                    {
                        ChangeWorkIndex();
                        break;
                    }
                case 1:
                    if (timeDistribution.BridgesTime > 0)
                    {
                        nextWorkUpdate = UpdateTime(timeDistribution.BridgesTime);
                        break;
                    }
                    else
                    {
                        ChangeWorkIndex();
                        break;
                    }
                case 2:
                    if (timeDistribution.HousesTime > 0)
                    {
                        nextWorkUpdate = UpdateTime(timeDistribution.HousesTime);
                        break;
                    }
                    else
                    {
                        ChangeWorkIndex();
                        break;
                    }
                case 3:
                    if (timeDistribution.ProcreationTime > 0)
                    {
                        nextWorkUpdate = UpdateTime(timeDistribution.ProcreationTime);
                        break;
                    }
                    else
                    {
                        ChangeWorkIndex();
                        break;
                    }
                case 4:
                    worksIndex = Random.Range(-2, 3) + 1;
                    break;
            }
        }
    }

    private float UpdateTime(float timeIncrease)
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
        canBeEaten = false;
        Debug.Log("Got home");
        agentBehaviours.isHome = true;

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
                agentBehaviours.StayHome();
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
        if (canBeEaten && isNight)
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentWasEaten(timeDistribution.DaysPassed - dateBorn);

            KillItself();
        }
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

        maxLife = Random.Range(timeInDay * 15, timeInDay * 45);
    }

    private void FindHouse()
    {
        GameObject house;
        GameObject[] housesAvailable = GameObject.FindGameObjectsWithTag("HouseNotBuiltAvailable");
        if (housesAvailable.Length > 0)
        {
            house = gameObject.GetComponent<AgentNavigator>().FindNearestObject(housesAvailable).gameObject;
            AllocateHouse(house);
        }

        if (agentBehaviours.Home == null)
        {
            housesAvailable = GameObject.FindGameObjectsWithTag("HouseBuiltAvailable");
            if (housesAvailable.Length > 0)
            {
                house = gameObject.GetComponent<AgentNavigator>().FindNearestObject(housesAvailable).gameObject;
                AllocateHouse(house);
            }
        }

        if (agentBehaviours.Home != null)
            agentBehaviours.Home.GetComponent<HouseScript>().UpdateAgentReproduction(agentBehaviours.IsFull() || agentBehaviours.CanProcreate());
    }

    private bool AllocateHouse(GameObject house)
    {
        HouseScript houseScript = house.GetComponent("HouseScript") as HouseScript;
        if (houseScript.AllocateAgent())
        {
            agentBehaviours.SetHome(house);

            return true;
        }
        return false;
    }
}
