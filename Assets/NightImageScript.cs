using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightImageScript : MonoBehaviour {


    public float fadeSpeed = 0.5f;

    public float alpha = 0.3f;

    // Use this for initialization
    void Start () {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        
        GUI.depth = 1000;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
