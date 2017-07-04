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
        (playerNameTxtbox.GetComponent("InputField") as InputField).text = PlayerPrefs.GetString("DisplayName");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNameTxtbox.active)
        {
            Time.timeScale = 0;
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }


    public void OnSaveNameBtnPress()
    {
        int total = 0;
        total = PlayerPrefs.GetInt("saved_total"); //set the total variable to the previously saved value 
        PlayerPrefs.SetString("UserName"+total, ((playerNameTxtbox.GetComponent("InputField") as InputField).text));

        PlayerPrefs.SetString("DisplayName", ((playerNameTxtbox.GetComponent("InputField") as InputField).text)); //set the new total value
        PlayerPrefs.SetString("UserName", ((playerNameTxtbox.GetComponent("InputField") as InputField).text) + total); //set the new total value

        total += 1;
        PlayerPrefs.SetInt("saved_total", total); //set the new total value
        PlayerPrefs.Save();

        playerNameTxtbox.transform.parent.gameObject.SetActive(false);

        Time.timeScale = 1;

        if(toggleDecay.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("Decay", 1);
        }
        if (toggleImmigration.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("Immigration", 1);
        }
        if (toggleFloods.GetComponent<Toggle>().isOn)
        {
            floodManager.GetComponent<FloodingManager>().enabled = true;
        }

        LogfileWriter.GetInstance().Createfile();
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
