using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentResourcesManager agentResources = new AgentResourcesManager();

    private NavMeshAgentPath agentPathfinding;

    private AgentActionsHandler agentActionsHandler;

    private AgentsDeathsHandler deathsCounters;

    private int nextStaminaUpdate;

    private const int STAMINA_UPDATE_TIME = 10;

    private const int PROCREATE_CHANCE = 50;

    private int bornTime;

    private const int MAX_LIFE = 480;

    void Start()
    {
        nextStaminaUpdate = STAMINA_UPDATE_TIME;
        bornTime = Mathf.FloorToInt(Time.time);

        agentPathfinding = gameObject.GetComponent("NavMeshAgentPath") as NavMeshAgentPath;
        agentActionsHandler = new AgentActionsHandler(agentPathfinding, agentResources);
    }

    void Update()
    {
        CheckIfAlive();
        UpdateStamina();

        if (agentActionsHandler != null)
        {
            agentActionsHandler.PerformActionSelection();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && gameObject != null)
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
        agentActionsHandler.StarNewWork(workIndex);
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
        agentActionsHandler.Build();
    }

    internal void GotEaten()
    {
        AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
        deathsHandler.AgentWasEaten();

        KillItself();
    }

    private void UpdateStamina()
    {
        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + STAMINA_UPDATE_TIME;             // Change the next update (current second+1)
            Tired();
        }
    }

    private void Tired()
    {
        agentResources.DecreaseStamina();
        if (agentResources.Stamina == 0)
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentStaved();

            KillItself();
        }
    }

    private void CheckIfAlive()
    {
        if (Time.time >= MAX_LIFE)
        {
            AgentsDeathsHandler deathsHandler = transform.parent.gameObject.GetComponent("AgentsDeathsHandler") as AgentsDeathsHandler;
            deathsHandler.AgentDied();

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

        Destroy(gameObject, 1f);
    }
}