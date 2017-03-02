using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public GameObject agentsManager;

    private TimeDistribution timeDistribution;

    private float dayLength;

    private float nightLength;

    private float agentsSpeed;

    // Use this for initialization
    void Start () {

        timeDistribution = agentsManager.GetComponent("TimeDistribution") as TimeDistribution;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    internal void OnTimeSliderChange(int factor)
    {
         
    }


    internal void ChangeSpeed(int factor)
    {
        dayLength = dayLength / factor;
        nightLength = nightLength / factor;


        agentsSpeed = agentsSpeed * factor;

        timeDistribution.ChangeDayNightCycle(factor);
    }
}