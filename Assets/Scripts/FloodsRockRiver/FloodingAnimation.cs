using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodingAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.SetActive(true);

        transform.GetChild(1).gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startAnimation()
    {
        StartCoroutine(animateFlood());
    }

    private IEnumerator animateFlood()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
     //   yield return new WaitForSeconds(2f);   //Wait
        //transform.GetChild(2).gameObject.SetActive(true);
       // transform.GetChild(0).gameObject.SetActive(false);
      //  transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);   //Wait
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
     //   transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);   //Wait
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
      //  transform.GetChild(2).gameObject.SetActive(false);
    }
}
