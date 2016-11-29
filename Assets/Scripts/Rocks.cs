using UnityEngine;
using System.Collections;

public class Rocks : MonoBehaviour
{

    public GameObject rockTemplate;

    private int restockTime = 1;

    public const double MAX_ROCKS = 100;

    private GameObject[] rocksAvailable;

    // Use this for initialization
    void Start()
    {
        RefreshStock();
    }

    // Update is called once per frame
    void Update()
    {
        //    UpdateRocksList();
        //  RefreshStock();
    }


    private void UpdateRocksList()
    {
        rocksAvailable = GameObject.FindGameObjectsWithTag("Rock");
    }

    private void RefreshStock()
    {
        int i = 0;
        while (i < MAX_ROCKS)
        {
            SpawnRocks();
            i++;
        }
    }

    private void SpawnRocks()
    {
        Vector3 pos = GetRandomPos();
        GameObject rock = Instantiate(rockTemplate, pos, Quaternion.Euler(90, 0, 0)) as GameObject;
        rock.transform.SetParent(gameObject.transform);
    }


    private bool isPosOK(Vector3 pos, float radius = 0.25f)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        foreach (Collider collider in colliders)
        {
            GameObject go = collider.gameObject;

            if (go.transform.CompareTag("Water") || go.transform.CompareTag("Rock"))
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 GetRandomPos()
    {
        float minX = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2) + 0.25f;
        float maxX = gameObject.transform.position.x + (gameObject.transform.localScale.x / 2) + 0.25f;

        float minZ = gameObject.transform.position.z - (gameObject.transform.localScale.z / 2) + 0.75f;
        float maxZ = gameObject.transform.position.z + (gameObject.transform.localScale.z / 2) - 0.75f;

        bool foundValidPos = false;

        Vector3 newVec = new Vector3(Random.Range(minX, maxX),
                               0,
                               Random.Range(minZ, maxZ));

        int maxRetries = 0;

        while (!foundValidPos && maxRetries < 10)
        {
            foundValidPos = isPosOK(newVec);
            if (foundValidPos)
                break;

            newVec = new Vector3(Random.Range(minX, maxX),
                    0,
                    Random.Range(minZ, maxZ));

            maxRetries++;
        }

        return newVec;
    }
}
