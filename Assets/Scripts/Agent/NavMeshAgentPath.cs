using UnityEngine;

public class NavMeshAgentPath : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agentNavMesh;

    private Animator anim;

    private float AGENT_SPEED;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().updateRotation = false;
        agentNavMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();

        AGENT_SPEED = agentNavMesh.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (agentNavMesh.velocity.x == 0 && agentNavMesh.velocity.z == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", agentNavMesh.velocity.x);
            anim.SetFloat("input_y", agentNavMesh.velocity.z);
        }
    }

    internal void StopWalking()
    {
        anim.SetBool("isWalking", false);
        agentNavMesh.speed = 0;
    }

    internal Transform GoToFood()
    {
        agentNavMesh.speed = 5;

        bool isBottomForestAvailable = false;
        if (GameObject.FindGameObjectsWithTag("BridgeAvailable").Length > 0)
        {
            isBottomForestAvailable = true;
        }

        GameObject[] foodAvailable = GameObject.FindGameObjectsWithTag("Food");
        Transform target = null;
        target = FindNearestObject(foodAvailable);

        if (target != null)
        {
            if (target.transform.position.z < 0 && !isBottomForestAvailable)
            {
                return null;
            }
        }

        GoToTarget(target);

        return target;
    }

    internal Transform GoToUnfinishedBridge()
    {
        GameObject[] bridges = GameObject.FindGameObjectsWithTag("BridgeNotAvailable");

        return GoToTarget((FindNearestObject(bridges)));
    }

    internal void GoHome(GameObject home)
    {
        if (home.transform)
        {
            Vector3 location = new Vector3(home.transform.position.x + 0.7f, 
                home.transform.position.y, home.transform.position.z - 0.7f);
  
            agentNavMesh.speed = AGENT_SPEED;
            Debug.DrawLine(transform.position, location, Color.green);
            agentNavMesh.SetDestination(location);
        }  
    }

    internal void GoToRocks()
    {
        GoToTarget((FindNearestObject(GameObject.FindGameObjectsWithTag("Rock"))));
    }

    internal Transform FindNearestObject(GameObject[] targetObjects)
    {
        float minDistance = Mathf.Infinity; // initiate distance at max value possible
        Transform target = null; // use this to detect the no enemy case

        foreach (GameObject nearestObject in targetObjects)
        {
            float targetDistance = Vector2.Distance(new Vector2(nearestObject.transform.position.x,
                nearestObject.transform.position.z),
                new Vector2(transform.position.x, transform.position.z));

            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                target = nearestObject.transform;
            }
        }

        return target;
    }

    internal void IncreaseSpeed()
    {

    }


    internal void DecreaseSpeed()
    {

    }

    private Transform GoToTarget(Transform target)
    {
        if (target)
        {
            agentNavMesh.speed = AGENT_SPEED;
            Debug.DrawLine(transform.position, target.position, Color.green);
            agentNavMesh.SetDestination(target.position);
        }

        return target;
    }
}
