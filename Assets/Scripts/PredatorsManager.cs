using UnityEngine;
using System.Collections;

public class PredatorsManager : MonoBehaviour
{
    public GameObject agentManager;

    private const int NIGHT_KILL_PERCENTAGE = 75;

    private bool isNight = false;

    private bool wasUsed = false;

    internal void setNight(bool value)
    {
        isNight = value;
        wasUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNight && !wasUsed)
        {
            //KillAgents();
            StartCoroutine(KillAgents());
            wasUsed = true;
        }
    }

    private IEnumerator KillAgents()
    {
        yield return new WaitForSeconds(60f);
        AgentActionsSelector[] agents = agentManager.transform.GetComponentsInChildren<AgentActionsSelector>();
         int killOrNot = Random.Range(0, 100);

        if (isNight)
            if (agents.Length > 0)
            {
                               for (int i = 0; i < agents.Length; i++)
               // for (int i = 0; i < agents.Length/10; i++)
                {
                    if (killOrNot < NIGHT_KILL_PERCENTAGE)
                    {
                        AgentActionsSelector agent = agents[i].GetComponent("AgentActionsSelector") as AgentActionsSelector;
                        agent.GotEaten();
                    }

                }         
            }
    }
}
