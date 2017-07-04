using DigitalRuby.RainMaker;
using UnityEngine;

public class FloodingManager : MonoBehaviour
{
    public GameObject rainManager;

    public GameObject lighting;

    public GameObject agentManager;

    private int nextFlood = 1500;

    private const int FLOOD_TIMER = 1500;

    private int timesInvoked;

    public GameObject river;

    private FloodingAnimation animation;

    private RainScript2D rain;

    private bool floodBeforeWritten = false;
    private bool floodAfterWritten = false;

    void Start()
    {
        animation = river.GetComponent("FloodingAnimation") as FloodingAnimation;

        rain = rainManager.GetComponent<RainScript2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FloodRiver();
    }

    private void FloodRiver()
    {
        if ((Time.time >= nextFlood - 120))
        {
            rain.RainIntensity = 1;
        }

        if ((Time.time >= nextFlood - 60))
        {
            rain.RainIntensity = 2;
        }

        if ((Time.time >= nextFlood - 30))
        {
            rain.RainIntensity = 3;
            if (!floodBeforeWritten)
            {
                agentManager.GetComponent<AgentsCountersTxtboxesUpdater>().WriteLogBeforeFlood();
                floodBeforeWritten = true;
            }
        }

        if ((Time.time >= nextFlood - 15))
        {
            rain.RainIntensity = 5;
        }

        if ((Time.time >= nextFlood - 5))
        {
            rain.RainIntensity = 7;
        }


        if ((Time.time >= nextFlood))
        {
            nextFlood = Mathf.FloorToInt(Time.time) + FLOOD_TIMER - (timesInvoked * 5);
            animation.startAnimation();

            for (int i = 0; i < agentManager.transform.GetChildCount(); i++)
            {
                if(isAgentOnRiver(agentManager.transform.GetChild(i).transform.position))
                {
                    agentManager.transform.GetChild(i).GetComponent<AgentActionsSelector>().GotDrowned();
                }
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                BridgeScript bridgeScript = transform.GetChild(i).GetComponent("BridgeScript") as BridgeScript;
                if (bridgeScript != null)
                {
                    bridgeScript.Destroy();
                }
            }
            timesInvoked++;

            if (timesInvoked == 5)
            {
                nextFlood = Mathf.FloorToInt(Time.time) + FLOOD_TIMER;
            }


            if ((Time.time + 30 >= (nextFlood - FLOOD_TIMER)))
            {
                if (!floodAfterWritten)
                {
                    agentManager.GetComponent<AgentsCountersTxtboxesUpdater>().WriteLogAfterFlood();
                    floodAfterWritten = true;
                }
            }


            rain.RainIntensity = 0;
        }
    }

    private bool isAgentOnRiver(Vector3 pos, float radius = 0.1f)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        Debug.DrawLine(pos, pos + new Vector3(radius,radius, radius));

        foreach (Collider collider in colliders)
        {
            GameObject go = collider.gameObject;

            if (go.GetInstanceID().Equals(river.GetInstanceID()))
            {
                return true;
            }
        }

        return false;
    }
}
