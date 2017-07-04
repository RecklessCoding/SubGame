using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject scoreBoard;

    public GameObject agentsManager;

    private bool gameStarted = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Time.timeScale > 0)
            {
                gameStarted = true;
            }
            if (gameStarted)
                scoreBoard.SetActive(!scoreBoard.activeSelf);
        }
    }
}
