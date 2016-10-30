using UnityEngine;
using System.Collections;

public class HouseScript : MonoBehaviour {

            
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private bool isAgentOnThisHouse(Vector3 pos, float radius = 1f)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        foreach (Collider collider in colliders)
        {
            GameObject go = collider.gameObject;

            if (go.CompareTag(gameObject.tag))
            {
                return true;
            }
        }

        return false;
    }
}
