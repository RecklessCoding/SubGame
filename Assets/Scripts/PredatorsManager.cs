using UnityEngine;
using System.Collections;

public class PredatorsManager : MonoBehaviour
{
    private const int NUMBER_OF_PREDATORS = 20;

    private const int DAY_KILL_PERCENTAGE = 20;

    private const int NIGHT_KILL_PERCENTAGE = 60;

    private int killTime = 30;

    private int timeToKill = 30;

    void Start()
    {

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
                if (killOrNot < DAY_KILL_PERCENTAGE)
                {
                    if (agents.Length > 0)
                    {
                        Agent agent = agents[Random.Range(0, agents.Length)].GetComponent("Agent") as Agent;
                        agent.GotEaten();
                    }
                }
            }
        }
    }
}
