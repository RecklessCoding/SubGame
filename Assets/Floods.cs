using UnityEngine;
using System.Collections;
using System;

public class Floods : MonoBehaviour
{
    private int nextFlood = 60;

    private const int FLOOD_TIMER = 360;

    private int timesInvoked;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FloodRiver();
    }

    private void FloodRiver()
    {
        if ((Time.time >= nextFlood))
        {
            nextFlood = Mathf.FloorToInt(Time.time) + FLOOD_TIMER - (timesInvoked * 5);

            for (int i = 0; i < transform.childCount; i++)
            {
                BridgeScript bridgeScript = transform.GetChild(i).GetComponent("BridgeScript") as BridgeScript;
                if (bridgeScript != null)
                {
                    bridgeScript.Destroy();
                }
            }
            timesInvoked++;
        }
    }
}
