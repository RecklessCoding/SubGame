using UnityEngine;
using System.Collections;

public class PredatorsManager : MonoBehaviour
{
    private const int NUMBER_OF_PREDATORS = 10;

    private const int DAY_KILL_PERCENTAGE = 10;

    private const int NIGHT_KILL_PERCENTAGE = 50;

    private int killTime = 150;

    private int timeToKill = 75;

    private bool isNight = false;

    void Start()
    {

    }

    internal void setNight(bool value)
    {
        isNight = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeToKill)
        {
            timeToKill = Mathf.FloorToInt(Time.time) + killTime;
            GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");

            for (int i = 0; i < NUMBER_OF_PREDATORS; i++)
            {
                int killOrNot = Random.Range(0, 100);

                if (!isNight)
                    if (killOrNot < DAY_KILL_PERCENTAGE)
                    {
                        if (agents.Length > 0)
                        {
                            AgentActionsSelector agent = agents[Random.Range(0, agents.Length)].GetComponent("AgentActionsSelector") as AgentActionsSelector;
                            agent.GotEaten();
                        }
                    }
            }
        }

        if (isNight)
        {
            timeToKill = Mathf.FloorToInt(Time.time) + killTime;
            GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
            int killOrNot = Random.Range(0, 100);

            if (killOrNot < NIGHT_KILL_PERCENTAGE)
            {
                if (agents.Length > 0)
                {
                    AgentActionsSelector agent = agents[Random.Range(0, agents.Length)].GetComponent("AgentActionsSelector") as AgentActionsSelector;
                    agent.GotEaten();
                }
            }
        }
    }
}
