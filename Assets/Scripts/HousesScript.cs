using UnityEngine;

public class HousesScript : MonoBehaviour
{
    public GameObject houseTemplate;

    private int numberOfHouses;

    public GameObject BuildNewHouse()
    {
        Debug.Log("Building!");
        Vector3 pos = GetRandomPos();

        return Instantiate(houseTemplate, pos, Quaternion.Euler(90, 0, 0)) as GameObject;
    }

    private bool isPosOK(Vector3 pos, float radius = 20f)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);
            
        foreach (Collider collider in colliders)
        {
            GameObject go = collider.gameObject;

            if (go.transform.CompareTag("HouseNotBuiltAvailable") ||
                go.transform.CompareTag("HouseNotBuiltFull") ||
                go.transform.CompareTag("HouseBuiltAvailable") || 
                go.transform.CompareTag("HouseBuiltFull"))
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 GetRandomPos()
    {
        float minX = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2);
        float maxX = gameObject.transform.position.x + (gameObject.transform.localScale.x / 2);

        float minZ = gameObject.transform.position.z - (gameObject.transform.localScale.z / 2);
        float maxZ = gameObject.transform.position.z + (gameObject.transform.localScale.z / 2);

        Vector3 newVec = new Vector3(Random.Range(minX, maxX),
                                     0,
                                     Random.Range(minZ, maxZ));

        int maxRetries = 0; 

        while (maxRetries < 10)
        {
            if (isPosOK(newVec))
            {
                break;
            }

            newVec = new Vector3(minX,
                    0,
                    Random.Range(minZ, maxZ));

            maxRetries++;
        }

        return newVec;
    }
}
