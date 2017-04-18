using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public GameObject agentsManager;

    private int[] availableSpeed = { -2, 1, 2, 4, 8, 16, 32 };

    // Use this for initialization
    void Start()
    {
    }

    public void OnTimeSliderChange(float value)
    {
        int newSpeed = availableSpeed[(int) value];

        if (newSpeed < 0)
        {
            ChangeSpeed(Math.Abs(1 / ((float) newSpeed)));
        }
        else
        {
            ChangeSpeed(newSpeed);
        }
    }
     

    private void ChangeSpeed(float factor)
    {
        Time.timeScale = factor;
        //TimeDistribution timeDistribution = agentsManager.GetComponent("TimeDistribution") as TimeDistribution;
        //timeDistribution.ChangeDayNightCycle(factor);

        //AgentsManager agentMngr = agentsManager.GetComponent("AgentsManager") as AgentsManager;
        //agentMngr.ChangeSpeed(factor);
    }
}