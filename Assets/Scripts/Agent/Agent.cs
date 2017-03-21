using UnityEngine;

public class Agent : MonoBehaviour
{
    private GameObject agentsManager;

    private AgentResourcesManager agentResources = new AgentResourcesManager();

    private NavMeshAgentPath agentPathfinding;

    private AgentActionsHandler agentActionsHandler;

    private AgentsDeathsHandler deathsCounters;

    private const int PROCREATE_CHANCE = 5;

    private int dateBorn = 0;

    public float nextStaminaUpdate = 0;

    public float staminaUpdateTime = 0;

    public int maxLife = 0;

    void Start()
    {
        agentPathfinding = gameObject.GetComponent("NavMeshAgentPath") as NavMeshAgentPath;
        agentActionsHandler = new AgentActionsHandler(agentPathfinding, agentResources);

        agentsManager = transform.parent.gameObject;

        ResetToDefaultValues();

        dateBorn = (agentsManager.GetComponent("TimeDistribution") as TimeDistribution).DaysPassed;
    }

    void Update()
    {
        CheckIfAlive();
        UpdateStamina();

        if (agentActionsHandler != null)
        {
            agentActionsHandler.PerformActionSelection();
        }
        else
        {
            agentActionsHandler = new AgentActionsHandler(agentPathfinding, agentResources);
            agentActionsHandler.PerformActionSelection();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if ((other.gameObject != null) && (gameObject != null) && (agentActionsHandler != null))
            {
                if (other.gameObject.tag.Equals("Food") && agentActionsHandler.IsGatheringFood)
                {
                    GatherFood();
                }
                else if (other.gameObject.tag.Equals("Rock") && agentActionsHandler.IsGatheringRock)
                {
                    GatherRock();
                }
                else if (other.gameObject.tag.Equals("BridgeNotAvailable") && agentActionsHandler.IsBuildingBridges)
                {
                    Build();
                }
                else if (other.gameObject.Equals(agentResources.Home) && agentActionsHandler.IsGoingHome)
                {
                    GotHome();
                }
            }
        }
        catch (System.Exception e) // REALLY BAD FIX!
        {
            Debug.LogError(e);
        }
    }

    internal bool CanBuildBridge()
    {
        return agentResources.HasRocks() && agentActionsHandler.IsBuildingBridges;
    }

    internal bool CanBuildHouse()
    {
        return agentResources.HasRocks() && agentActionsHandler.IsGoingHome;
    }

    internal void StarNewWork(int workIndex)
    {
        try
        {
            if ((gameObject != null) && (agentActionsHandler != null))
            {
                agentActionsHandler.StarNewWork(workIndex);
            }
            else
            {
                agentActionsHandler = new AgentActionsHandler(agentPathfinding, agentResources);
                agentActionsHandler.StarNewWork(workIndex);
            }
        }
        catch (System.Exception e) // REALLY BAD FIX!
        {
            Debug.LogError(e);
        }
    }

    private void GatherFood()
    {
        agentResources.IncreaseFood();
        GatherResource();
        agentActionsHandler.Eat();
    }

    private void GatherResource()
    {
        agentPathfinding.StopWalking();

        agentActionsHandler.GatherResource();
    }

    private void GatherRock()
    {
        GatherResource();
        agentResources.IncreaseRocks();
    }

    private void GotHome()
    {
        agentPathfinding.StopWalking();

        if (agentResources.HasHomeNotBuilt())
        {
            Build();
        }
        else
        {
            Reproduce();
        }

        agentActionsHandler.IsGoingHome = false;
    }

    private void Reproduce()
    {
        HouseScript home = agentResources.Home.GetComponent("HouseScript") as HouseScript;
        if (home.CanReproduce())
        {
            int dieRoll = UnityEngine.Random.Range(1, 100);

            if (dieRoll < PROCREATE_CHANCE)
            {
                AgentsCreator agentsCreator = transform.parent.gameObject.GetComponent("AgentsCreator") as AgentsCreator;
                agentsCreator.BornAgent(transform.position);
            }
        }
    }

    internal void Build()
    {
        agentPathfinding.StopWalking();
        if (agentActionsHandler != null)
        {
            agentActionsHandler.Build();
        }
    }

    internal void GotEaten()
    {
        AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
        deathsHandler.AgentWasEaten((agentsManager.GetComponent("TimeDistribution") as TimeDistribution).DaysPassed - dateBorn);

        KillItself();
    }

    private void UpdateStamina()
    {
        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + staminaUpdateTime;             // Change the next update (current second+1)
            Tired();
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

        if (agentResources.Stamina == 2)
        {
            //TODO
        }
    }

    private void CheckIfAlive()
    {
        if (Time.time >= maxLife)
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentDied((agentsManager.GetComponent("TimeDistribution") as TimeDistribution).DaysPassed - dateBorn);

            KillItself();
        }
    }

    private void KillItself()
    {
        if (agentResources.HasHome())
        {
            HouseScript home = agentResources.Home.GetComponent("HouseScript") as HouseScript;
            home.RemoveAgent();
        }

        Destroy(gameObject);
    }

    internal void ChangeSpeed(float factor)
    {
        if (factor != 1)
        {
            staminaUpdateTime = (agentsManager.GetComponent("TimeDistribution") as TimeDistribution).timeInDay;

            nextStaminaUpdate = ((nextStaminaUpdate - Mathf.FloorToInt(Time.time)) / factor) + Mathf.FloorToInt(Time.time);

            maxLife = Mathf.FloorToInt(staminaUpdateTime) * 30;
        }
        else
        {
            ResetToDefaultValues();
        }

        agentPathfinding.ChangeSpeed(factor);
    }

    private void ResetToDefaultValues()
    {
        TimeDistribution td = agentsManager.GetComponent("TimeDistribution") as TimeDistribution;
        maxLife = Mathf.FloorToInt(td.TimeInDay) * 15;
        staminaUpdateTime = Mathf.FloorToInt(td.TimeInDay);
        nextStaminaUpdate = ((nextStaminaUpdate - Mathf.FloorToInt(Time.time))) + Mathf.FloorToInt(Time.time) + Mathf.FloorToInt(staminaUpdateTime);
    }
}