using UnityEngine;
using UnityEngine.UI;

public class SavePlayer : MonoBehaviour
{

    public GameObject playerNameTxtbox;

    public GameObject toggleFloods;

    public GameObject toggleDecay;

    public GameObject toggleImmigration;

    public GameObject floodManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerNameTxtbox.active)
        {
            Time.timeScale = 0;
        }
    }

    public void OnSaveNameBtnPress()
    {
        PlayerPrefs.SetString("UserName", ((playerNameTxtbox.GetComponent("InputField") as InputField).text));
        playerNameTxtbox.transform.parent.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    public void OnFloodToggleChange(bool value)
    {
        floodManager.GetComponent<FloodingManager>().enabled = value;
    }

    public void OnDecayToggleChange(bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt("Decay", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Decay", 0);
        }
    }

    public void OnImmigrationToggleChange(bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt("Immigration", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Immigration", 0);
        }
    }
}
