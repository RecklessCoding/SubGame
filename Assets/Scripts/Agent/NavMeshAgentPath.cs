using UnityEngine;
using System.Collections.Generic;

public class NavMeshAgentPath : MonoBehaviour {

    public GameObject[] foodAvailable;

    public GameObject[] rocksAvailable;

    NavMeshAgent agent;
    Animator anim;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        transform.GetComponent<NavMeshAgent>().updateRotation = false;

        foodAvailable = GameObject.FindGameObjectsWithTag("Food");
        rocksAvailable = GameObject.FindGameObjectsWithTag("Rock");
    }
	
	// Update is called once per frame
	void Update () {
        if (agent.velocity.x == 0 && agent.velocity.z == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", agent.velocity.x);
            anim.SetFloat("input_y", agent.velocity.z);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        anim.SetBool("isWalking", false);
    }

    public void GoToFood()
    {
        foodAvailable = GameObject.FindGameObjectsWithTag("Food");
        float minDistance = Mathf.Infinity;
        Transform target = null;

        bool isRightForestAvailable = false;

        if (GameObject.FindGameObjectsWithTag("BridgeAvailable").Length > 0)
        {
            isRightForestAvailable = true;
        }

        foreach (GameObject nearestFood in foodAvailable)
        {
            float foodDistance = Vector3.Distance(nearestFood.transform.position, transform.position);

            if (nearestFood.transform.position.x > 0)
            {
                if (!isRightForestAvailable)
                {
                    foodDistance = 99999999f;
                }
            } 

            if (foodDistance < minDistance)
            {
                target = nearestFood.transform;
            }
        }

        if (target)
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
            agent.SetDestination(target.position);
        }
    }

    public void GoToRocks()
    {
    rocksAvailable = GameObject.FindGameObjectsWithTag("Food");


        float minDistance = Mathf.Infinity; // initiate distance at max value possible
        Transform target = null; // use this to detect the no enemy case

        foreach (GameObject nearestRock in rocksAvailable)
        {
            float roctDistance = Vector3.Distance(nearestRock.transform.position, transform.position);

            if (roctDistance < minDistance)
            { 
                minDistance = roctDistance;    
                target = nearestRock.transform;              
            }
        }
        if (target)
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
            agent.SetDestination(target.position);
        }
    }
}
