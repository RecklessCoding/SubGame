using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuInvoker : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject helpButton;
    public GameObject resumeButtom;
    public GameObject exitButton;

    public GameObject goBackButtom;
    public GameObject resumeButtom2;
    public GameObject helpText;

    public GameObject background;

    public bool isPaused = false;

    private float lastTimescale = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Application.targetFrameRate = 30;

                ResumeGame();
                HideMenu();
            }
            else
            {
                Application.targetFrameRate = 45;

                PauseGame();
                ShowMenu();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        lastTimescale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void ShowMenu()
    {
        restartButton.SetActive(true);
        helpButton.SetActive(true);
        resumeButtom.SetActive(true);
        exitButton.SetActive(true);
        background.SetActive(true);

    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = lastTimescale;
    }

    private void HideMenu()
    {
        restartButton.SetActive(false);
        helpButton.SetActive(false);
        resumeButtom.SetActive(false);
        goBackButtom.SetActive(false);
        resumeButtom2.SetActive(false);
        exitButton.SetActive(false);
        helpText.SetActive(false);
        background.SetActive(false);
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }


    public void OnResumeButtonClick()
    {
        ResumeGame();
        HideMenu();
    }

    public void OnHelpButtonClick()
    {
        HideMenu();

        resumeButtom2.SetActive(true);
        goBackButtom.SetActive(true);
        helpText.SetActive(true);
        background.SetActive(true);

    }

    public void OnHelpGoBackButtonClick()
    {
        resumeButtom2.SetActive(false);
        goBackButtom.SetActive(false);
        helpText.SetActive(false);
        ShowMenu();
    }
}