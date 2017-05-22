using UnityEngine;
using UnityEngine.AI;

public class AgentNavigator : MonoBehaviour
{
    private NavMeshAgent agentNavMesh;

    private Animator anim;

    private float speed;

    private const float DEFAULT_SPEED = 0.25f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().updateRotation = false;
        agentNavMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();

        SetDefeault();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
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

    internal void ChangeSpeed(float factor)
    {
        if (factor != 1)
        {
            speed = DEFAULT_SPEED * factor;
            SetSpeed(speed);
        }
        else
        {
            SetDefeault();
        }
    }

    private void SetDefeault()
    {
        speed = DEFAULT_SPEED;
        SetSpeed(speed);
    }

    internal Transform GoToFood()
    {
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

            Debug.DrawLine(transform.position, location, Color.green);
            if (agentNavMesh.isOnNavMesh)
            {
                SetSpeed(speed);
                agentNavMesh.SetDestination(location);
            }
        }
    }

    internal void StopWalking()
    {
        anim.SetBool("isWalking", false);
        SetSpeed(0);
    }

    private void SetSpeed(float spd)
    {
        agentNavMesh.speed = spd;
        agentNavMesh.acceleration = 0.5f;
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


    private Transform GoToTarget(Transform target)
    {
        if (target)
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
            if (agentNavMesh.isOnNavMesh)
            {
                SetSpeed(speed);
                agentNavMesh.SetDestination(target.position);
            }
        }

        return target;
    }
}
