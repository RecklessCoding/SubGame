using UnityEngine;
using System.Collections;

public class FoodScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            StartCoroutine(BeEaten());
        }
    }

    public void Grow()
    {

    }

    IEnumerator BeEaten()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
