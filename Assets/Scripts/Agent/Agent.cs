using UnityEngine;
using System.Collections;
using System;

public class Agent : MonoBehaviour
{
    AgentResourcesManager agentResources = new AgentResourcesManager();

    NavMeshAgentPath agentPathfinding;

    AgentActionsHandler agentActionsHandler;

    private bool isAlive = true;
    private int nextStaminaUpdate = 20;
    private const int STAMINA_UPDATE_TIME = 20;

    private const int PROCREATE_CHANGE = 10;

    void Start()
    {
        agentPathfinding = gameObject.GetComponent("NavMeshAgentPath") as NavMeshAgentPath;
        agentActionsHandler = new AgentActionsHandler(agentPathfinding, agentResources);
    }

    void Update()
    {
        CheckIfAlive();
        UpdateStamina();

        if (agentActionsHandler != null)
            agentActionsHandler.PerformActionSelection();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
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


    public bool CanBuildBridge()
    {
        return agentResources.HasRocks() && agentActionsHandler.IsBuildingBridges;
    }

    public bool CanBuildHouse()
    {
        return agentResources.HasRocks() && agentActionsHandler.IsGoingHome;
    }

    public void StarNewWork(int workIndex)
    {
        agentActionsHandler.StarNewWork(workIndex);
    }

    private void CheckIfAlive()
    {
        if (!isAlive)
        {
            KillItself();
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
        } else
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
            int dieRool = UnityEngine.Random.Range(1, 100);

            if (dieRool < PROCREATE_CHANGE)
            {
                AgentsCreator agentsCreator = transform.parent.gameObject.GetComponent("AgentsCreator") as AgentsCreator;
                agentsCreator.SpawnAgent(transform.position);
            }
        }
    }

    public void KillItself()
    {
        if (agentResources.HasHome())
        {
            HouseScript home = agentResources.Home.GetComponent("HouseScript") as HouseScript;
            home.RemoveAgent();
        }

        Destroy(gameObject, 1f);
    }

    private void UpdateStamina()
    {
        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + STAMINA_UPDATE_TIME;             // Change the next update (current second+1)
            Tired();
        }
    }

    public void Build()
    {
        agentPathfinding.StopWalking();
        agentActionsHandler.Build();
    }

    private void Tired()
    {
        agentResources.DecreaseStamina();
        if (agentResources.Stamina == 0)
        {
            isAlive = false;
            KillItself();
        }
    }
}
