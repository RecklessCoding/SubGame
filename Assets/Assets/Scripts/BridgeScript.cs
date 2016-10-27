using UnityEngine;
using System.Collections;

public class BridgeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            StartCoroutine(Build());
        }
    }

    IEnumerator Build()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}