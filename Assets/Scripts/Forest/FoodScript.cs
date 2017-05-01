using UnityEngine;
using System.Collections;

public class FoodScript : MonoBehaviour {

    void Start()
    {
       // StartCoroutine(Grow());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            StartCoroutine(BeEaten());
        }
    }

    IEnumerator Grow()
    {
        transform.tag = "FoodNotReady";
        yield return new WaitForSeconds(500);
        transform.tag = "Food";

    }

    IEnumerator BeEaten()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
