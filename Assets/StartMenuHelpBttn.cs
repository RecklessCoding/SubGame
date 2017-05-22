using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuHelpBttn : MonoBehaviour {

    public GameObject gameStart;

    public GameObject helpText;

    public GameObject backBttn;

    public GameObject background;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnHelpButtonClick()
    {
        for (int i = 0; i < gameStart.transform.childCount; i++)
        {
            gameStart.transform.GetChild(i).gameObject.SetActive(false) ;
        }
        background.SetActive(true);
        helpText.SetActive(true);
        backBttn.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        background.SetActive(false);

        for (int i = 0; i < gameStart.transform.childCount; i++)
        {
            gameStart.transform.GetChild(i).gameObject.SetActive(true);
        }
        helpText.SetActive(false);
        backBttn.SetActive(false);

    }
}
