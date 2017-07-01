using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour
{

    public GameObject scoreBoard;

    public GameObject agentsManager;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            agentsManager.GetComponent<AgentsCountersTxtboxesUpdater>().UpdateScoreManager();

            scoreBoard.SetActive(!scoreBoard.activeSelf);
        }
    }
}
