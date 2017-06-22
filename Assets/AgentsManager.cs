using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    private int agentsCount = 0;

    private GameObject agent;

    void Update()
    {
        agentsCount = gameObject.transform.childCount;

        if (agentsCount == 0)
        {            // game over!

            (transform.GetComponent("AgentsCountersTxtboxesUpdater") as AgentsCountersTxtboxesUpdater).EndGame();

            (transform.GetComponent("TimeDistribution") as TimeDistribution).EndGame();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit something: " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Agent")
                {
                    if (agent != null)
                    {
                        agent.GetComponent<AgentActionsSelector>().MakeUnselected();
                    }
                    agent = hitInfo.transform.gameObject;
                    agent.GetComponent<AgentActionsSelector>().MakeSelected();                    
                }
            }
        }
    }

    internal void ChangeSpeed(float factor)
    {
        Agent[] agents = GetAllAgents();

        foreach (Agent agent in agents)
        {
            agent.ChangeSpeed(factor);
        }
    }

    internal int HowManyAgentsAreALive
    {
        get
        {
            return agentsCount;
        }
    }

    private Agent[] GetAllAgents()
    {
        return gameObject.GetComponentsInChildren<Agent>(); ;
    }
}