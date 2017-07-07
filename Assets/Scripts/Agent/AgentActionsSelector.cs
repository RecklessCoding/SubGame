using UnityEngine;

public class AgentActionsSelector : MonoBehaviour
{
    public float nextStaminaUpdate = 0;

    public float staminaUpdateTime = 0;

    private int dateBorn = 0;

    public float maxLife = 0;

    private GameObject agentsManager;

    private Color defaultColor;

    private TimeDistribution timeDistribution;

    private AgentBehaviourLibrary agentBehaviours;

    public int worksIndex = -1;
    private float nextWorkUpdate = -1;
    public bool isNight = false;
    public bool canBeEaten = false;

    private int botNumber = 0;
    private static int numberOfBots = 0;

    // Use this for initialization
    void Start()
    {
        numberOfBots = numberOfBots + 1;
        botNumber = numberOfBots;

        agentsManager = transform.parent.gameObject;
        agentBehaviours = gameObject.GetComponent<AgentBehaviourLibrary>();

        timeDistribution = gameObject.transform.parent.GetComponent("TimeDistribution") as TimeDistribution;
        dateBorn = timeDistribution.DaysPassed;

        nextStaminaUpdate = timeDistribution.TimeInDay;
        staminaUpdateTime = timeDistribution.TimeInDay / 1.5f;

        SetRandomAge();
        defaultColor = new Color(0, 1, 1, 1);
        GetComponent<SpriteRenderer>().color = defaultColor;

        worksIndex = Random.Range(-2, 3) + 1;

        FindHouse();

        agentBehaviours.SetBotNumber(botNumber);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);

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

        if (timeDistribution = null)
            timeDistribution = gameObject.transform.parent.GetComponent("TimeDistribution") as TimeDistribution;
    }

    void OnTriggerEnter(Collider collidedObject)
    {
        try
        {

            if ((collidedObject.gameObject != null) && (gameObject != null) && (agentBehaviours != null))
            {
                if (collidedObject.gameObject.Equals(agentBehaviours.Home) && isNight)
                {
                    if (!agentBehaviours.HasHomeNotBuilt())
                        canBeEaten = false;
                    else
                        canBeEaten = true;
                    agentBehaviours.StayHome();
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
                    ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-ReadyToBuild", "DE");
                    ABOD3_Bridge.GetInstance().AletForElement(botNumber, "BuildBridge", "A");
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


    void OnTriggerStay(Collider other)
    {
        if (agentBehaviours.home != null && !agentBehaviours.isHome)
        {
            if (other.gameObject.Equals(agentBehaviours.Home) && isNight)
            {
                if (!agentBehaviours.HasHomeNotBuilt())
                    canBeEaten = false;
                else
                    canBeEaten = true;
                agentBehaviours.isHome = true;
                agentBehaviours.IsGoingHome = false;
                agentBehaviours.StayHome();
            }
            else if (other.gameObject.Equals(agentBehaviours.Home) && agentBehaviours.IsGoingHome)
            {
                if (!agentBehaviours.HasHomeNotBuilt())
                    canBeEaten = false;
                else
                    canBeEaten = true;
                CGotHome();
            }
        }
    }

    public void MakeSelected()
    {
        ABOD3_Bridge.GetInstance().ChangeSelectedBot(botNumber);
        gameObject.GetComponent<SpriteOutline>().enabled = true;
    }

    public void MakeUnselected()
    {
        ABOD3_Bridge.GetInstance().ChangeSelectedBot(0);
        gameObject.GetComponent<SpriteOutline>().enabled = false;
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
        if (isNight)
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-Survive", "D");
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-IsNight", "DE");
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "RunHome", "A");

            agentBehaviours.GoToHome();
        }
        else if (agentBehaviours.IsStarving())
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-Survive", "D");
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-EatFood", "DE");

            CEatFood(true);
        }
        else
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-DailyLife", "D");
            switch (worksIndex)
            {
                case 0:
                    ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-EatFood", "D");
                    CEatFood(false);
                    break;
                case 1:
                    ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-BuildBridges", "D");
                    CBuildBridge();
                    break;
                case 2:
                    ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-BuildHome", "D");
                    CHomeBuilding();
                    break;
                case 3:
                    ABOD3_Bridge.GetInstance().AletForElement(botNumber, "D-Procreate", "D");
                    CTryToProcreate();
                    break;
            }
        }
    }

    private void CTryToProcreate()
    {
        if (agentBehaviours.HasHomeNotBuilt())
        {
            agentBehaviours.GoToHome();
        }
        else
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-HasHome", "DE");
            agentBehaviours.GoToProcreate();
            agentBehaviours.GoToHome();
        }
    }

    public void ForceWorkChange()
    {
        nextWorkUpdate = -1;
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
                    if (timeDistribution.ProcreationTime >= 1)
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
                    worksIndex = Random.Range(-2, 2) + 1;
                    ChangeWorkIndex();
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
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-DoesNotHaveHome", "DE");

            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "C-BuildHome", "C");
            if (agentBehaviours.HasRock())
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-HasRock", "CE");
                agentBehaviours.GoToHome();
            }
            else
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-HasNoRocks", "CE");
                agentBehaviours.GoToRock();
            }
        }
        else
        {
            ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-HasHome", "DE");
            agentBehaviours.GoToHome();
        }
    }

    private void CGotHome()
    {
        agentBehaviours.IsGoingHome = false;
        canBeEaten = false;
        agentBehaviours.isHome = true;

        if (agentBehaviours.HasHomeNotBuilt())
        {
            if (agentBehaviours.HasRock())
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-ReadyToBuild", "CE");
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "BuildHouse", "A");
                agentBehaviours.BuildHouse();
            }
        }
        else
        {
            if (agentBehaviours.IsGoingToProcreate)
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-IsHome", "DE");
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "C-Procreate", "C");
                agentBehaviours.Procreate();
                agentBehaviours.StayHome();
            }
            else
            {
                agentBehaviours.StayHome();
            }
        }
    }

    private void CEatFood(bool isEmergency)
    {

        if (agentBehaviours.HasFood())
        {
            if (isEmergency)
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-HasFood", "CE");
            }
            else
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-HasFood", "DE");
            }
            agentBehaviours.EatFood();
        }
        else
        {
            if (isEmergency)
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "CE-GoGetFood", "CE");
            }
            else
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-GoGetFood", "DE");
            }
            agentBehaviours.GoToFood();
        }
    }

    private void CBuildBridge()
    {
        if (!isNight)
            if (agentBehaviours.HasRock())
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-HasRocks", "DE");
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "GoNearestBridgeSite", "A");
                agentBehaviours.GoToBridge();
            }
            else
            {
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "DE-HasNoRocks", "DE");
                ABOD3_Bridge.GetInstance().AletForElement(botNumber, "GoGetRock", "A");
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
        else
        {
            agentBehaviours.BecomeUnableToProcreate();
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

    internal void GotDrowned()
    {

        AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
        deathsHandler.AgentWasDrowned(timeDistribution.DaysPassed - dateBorn);

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

        maxLife = Random.Range(timeInDay * 3, timeInDay * 45) + Time.time;
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
