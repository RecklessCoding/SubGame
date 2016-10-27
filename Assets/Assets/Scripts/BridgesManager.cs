using UnityEngine;
using System.Collections;

public class BridgesManager : MonoBehaviour {

    public GameObject bridgeTemplate;

    // Use this for initialization
    void Start () {
        BuildBridge();
    }

    // Update is called once per frame
    void Update () {
    }

    public void BuildBridge()
    {
        StartCoroutine(Build());
    }


    IEnumerator Build()
    {
        yield return new WaitForSeconds(2);

        Vector3 pos = GetRandomPos();

        GameObject bridge = Instantiate(bridgeTemplate, pos, Quaternion.Euler(90, 0, 0)) as GameObject;
        foreach (MeshRenderer mesh in bridge.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
    }

    private bool isPosOK(Vector3 pos, float radius = 0.25f)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        foreach (Collider collider in colliders)
        {
            GameObject go = collider.gameObject;

            if (go.transform.CompareTag("Rock"))
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 GetRandomPos()
    {
        float minX = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2);

        float minZ = gameObject.transform.position.z - (gameObject.transform.localScale.z / 2) + 0.75f;
        float maxZ = gameObject.transform.position.z + (gameObject.transform.localScale.z / 2) - 0.75f;

        bool foundValidPos = false;


        Vector3 newVec = new Vector3(minX,
                               0,
                               Random.Range(minZ, maxZ));

        int maxRetries = 0;

        while (!foundValidPos && maxRetries < 10)
        {
            foundValidPos = isPosOK(newVec);
            if (foundValidPos)
                break;

            newVec = new Vector3(minX,
                    0,
                    Random.Range(minZ, maxZ));

            maxRetries++;
        }

        return newVec;
    }
}
