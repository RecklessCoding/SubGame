using UnityEngine;

public class FloodingManager : MonoBehaviour
{
    private int nextFlood = 120;

    private const int FLOOD_TIMER = 360;

    private int timesInvoked;

    public GameObject river;

    private FloodingAnimation animation;

    void Start()
    {
        animation = river.GetComponent("FloodingAnimation") as FloodingAnimation;
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
            animation.startAnimation();

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
