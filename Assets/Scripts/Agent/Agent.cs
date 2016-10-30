using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    AgentResourcesManager agentResources;

    private int RESOURCE_DELAY = 2;

    private bool isAlive = true;
    private int nextStaminaUpdate = 1;

    // Use this for initialization
    void Start () {
        agentResources = new AgentResourcesManager();
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + 1;             // Change the next update (current second+1)
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
            GatherFood();
        }
        else if (other.gameObject.tag == "Rock")
        {
            GatherRock();
        }
    }

    private IEnumerator GatherFood()
    {
        yield return new WaitForSeconds(RESOURCE_DELAY);

        agentResources.IncreaseFood();
    }

    private IEnumerator GatherRock()
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
