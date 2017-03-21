using UnityEngine;
using System.Collections;
using System;

public class BridgeScript : MonoBehaviour
{
    public int rocksNeeded = 24;

    public int stage = 0;

    private int timeToDetarorate = 240;

    private const int DETARORATION_TIMER = 120;

    // Use this for initialization
    void Start()
    {
        Destroy();

        rocksNeeded = 24;
    }

    // Update is called once per frame
    void Update()
    {
        Deterorate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {

            Build(other.gameObject);
        }
    }

    public void Destroy()
    {
        gameObject.tag = "BridgeNotAvailable";

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        stage = 0;

        EnableNavMeshObstacle();
    }

    private IEnumerator BuildWithWait(GameObject go)
    {
        yield return new WaitForSeconds(2);
        Build(go);
    }

    private void Build(GameObject agentGO)
    {
        Agent agent = agentGO.GetComponent("Agent") as Agent;

        if (agent.CanBuildBridge() && (stage < (rocksNeeded)))
        {
            transform.GetChild(stage).gameObject.SetActive(true);

            stage = stage + 1;
            if (stage >= rocksNeeded)
            {
                stage = rocksNeeded-1;
            }
        }

        if (stage == (rocksNeeded-1))
        {
            gameObject.tag = "BridgeAvailable";
            UnityEngine.AI.NavMeshObstacle navOb = gameObject.GetComponent(typeof(UnityEngine.AI.NavMeshObstacle)) as UnityEngine.AI.NavMeshObstacle;
            if (navOb != null)
            {
                navOb.enabled = false;
            }
        }
    }

    private void Deterorate()
    {
        if ((Time.time >= timeToDetarorate) && (stage > 0))
        {
            timeToDetarorate = Mathf.FloorToInt(Time.time) + DETARORATION_TIMER;

            transform.GetChild(stage).gameObject.SetActive(false);
            stage = stage - 1;
            transform.GetChild(stage).gameObject.SetActive(true);

            Destroy();
        }
    }

    private void EnableNavMeshObstacle()
    {
        UnityEngine.AI.NavMeshObstacle navOb = gameObject.GetComponent(typeof(UnityEngine.AI.NavMeshObstacle)) as UnityEngine.AI.NavMeshObstacle;
        if (navOb != null)
        {
            navOb.enabled = true;
        }
    }
}