using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour
{

    public int agentsAllocatedToHouse = 0;

    private const int MAX_AGENTS = 7;

    private int rocksNeeded = 11;

    private int stage = 0;

    private int timeToDetarorate = 480;

    private const int DETARORATION_TIMER = 240;

    private bool isDecayOn = true;

    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        rocksNeeded = gameObject.transform.childCount;


        if (PlayerPrefs.GetInt("Decay") == 1)
        {
            isDecayOn = true;
        }
        else
        {
            isDecayOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDecayOn)
        {
            CheckForDeterorate();
        }

        if ((agentsAllocatedToHouse == 0))
        {
            DetorateHouse();
        }

        if (stage < 0)
        {
            stage = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            Build(other.gameObject);
        }
    }

    public bool AllocateAgent()
    {
        if (agentsAllocatedToHouse < MAX_AGENTS)
        {
            agentsAllocatedToHouse++;
            UpdateHouseStatus();

            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveAgent()
    {
        --agentsAllocatedToHouse;
        UpdateHouseStatus();
    }

    public bool CanReproduce()
    {
        return (agentsAllocatedToHouse > 2);
    }

    private void UpdateHouseStatus()
    {
        if (agentsAllocatedToHouse == MAX_AGENTS && gameObject.CompareTag("HouseBuiltFull"))
        {
            gameObject.tag = "HouseBuiltFull";
        }
        else if (agentsAllocatedToHouse == MAX_AGENTS && gameObject.CompareTag("HouseNotBuiltFull"))
        {
            gameObject.tag = "HouseNotBuiltFull";
        }
        else if (agentsAllocatedToHouse < MAX_AGENTS && gameObject.CompareTag("HouseNotBuiltAvailable"))
        {
            gameObject.tag = "HouseNotBuiltAvailable";
        }
        else if (agentsAllocatedToHouse < MAX_AGENTS && gameObject.CompareTag("HouseBuiltAvailable"))
        {
            gameObject.tag = "HouseBuiltAvailable";
        }
    }

    public void Destroy()
    {
        if (agentsAllocatedToHouse < MAX_AGENTS)
        {
            gameObject.tag = "HouseNotBuiltAvailable";
        }
        else
        {
            gameObject.tag = "HouseNotBuiltFull";
        }
    }

    private void Build(GameObject agentGO)
    {
        Agent agent = agentGO.GetComponent("Agent") as Agent;

        if (agent.CanBuildHouse() && (stage < (rocksNeeded)))
        {
            transform.GetChild(stage).gameObject.SetActive(true);
            stage = stage + 1;
            if (stage >= rocksNeeded)
            {
                stage = rocksNeeded - 1;
            }
        }

        if (stage == (rocksNeeded - 1))
        {
            if (agentsAllocatedToHouse == MAX_AGENTS)
            {
                gameObject.tag = "HouseBuiltFull";
            }
            else
            {
                gameObject.tag = "HouseBuiltAvailable";
            }
        }
    }

    private void CheckForDeterorate()
    {
        if ((Time.time >= timeToDetarorate) && (stage > 0))
        {
            if (Random.Range(0, 100) < 50)
            {
                DetorateHouse();
            }
            timeToDetarorate = Mathf.FloorToInt(Time.time) + DETARORATION_TIMER;
        }
    }

    private void DetorateHouse()
    {
        timeToDetarorate = Mathf.FloorToInt(Time.time) + DETARORATION_TIMER;

        transform.GetChild(stage).gameObject.SetActive(false);
        stage = stage - 1;
        if (stage > 0)
        {
            transform.GetChild(stage).gameObject.SetActive(true);
        }

        Destroy();
    }
}
