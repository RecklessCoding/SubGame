using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    AgentResourcesManager agentResources;

    NavMeshAgentPath agentPathfinding;

    private int RESOURCE_DELAY = 1;

    private bool isAlive = true;
    private int nextStaminaUpdate = 5;

    // Use this for initialization
    void Start () {
        agentResources = new AgentResourcesManager();
        agentPathfinding = gameObject.GetComponent("NavMeshAgentPath") as NavMeshAgentPath;
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + 10;             // Change the next update (current second+1)
            Tired();
        }

        if (!isAlive)
        {
            KillItself();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            StartCoroutine(GatherFood());
        }
        else if (other.gameObject.tag == "Rock")
        {
            StartCoroutine(GatherRock());
        }
    }

    IEnumerator GatherFood()
    {
        yield return new WaitForSeconds(RESOURCE_DELAY);

        Debug.Log("Collide!");

        agentResources.IncreaseFood();
    }

    IEnumerator GatherRock()
    {
        yield return new WaitForSeconds(RESOURCE_DELAY);

        agentResources.IncreaseRocks();
    }

    public void Eat()
    {
       // food -= foodDecreaser;
        //stamina += 100;
    }

    public void Build()
    {
        // food -= foodDecreaser;
        //stamina += 100;
    }

    private void Tired()
    {
        agentResources.DecreaseStamina();
        Debug.Log(agentResources.Food);

        if (agentResources.Stamina < 100 && agentResources.Food < 5)
        {
            agentPathfinding.GoToFood();
        }

        if (agentResources.Stamina == 0)
        {
            isAlive = false;
        }
    }

    public void KillItself()
    {
        Destroy(gameObject, 10f);
    }

}
