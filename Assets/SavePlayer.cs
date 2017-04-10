using UnityEngine;
using UnityEngine.UI;

public class SavePlayer : MonoBehaviour {

    public GameObject playerNameTxtbox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
     
    public void OnSaveNameBtnPress()
    {
        PlayerPrefs.SetString("UserName", ((playerNameTxtbox.GetComponent("InputField") as InputField).text));
        playerNameTxtbox.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
