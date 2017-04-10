﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInvoker : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject helpButton;
    public GameObject resumeButtom;

    public GameObject goBackButtom;
    public GameObject resumeButtom2;
    public GameObject helpText;

    public bool isPaused = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc");
            if (isPaused)
            {
                ResumeGame();
                HideMenu();
            }
            else
            {      
                PauseGame();
                ShowMenu();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    private void ShowMenu()
    {
        restartButton.SetActive(true);
        helpButton.SetActive(true);
        resumeButtom.SetActive(true);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    private void HideMenu()
    {
        restartButton.SetActive(false);
        helpButton.SetActive(false);
        resumeButtom.SetActive(false);
        goBackButtom.SetActive(false);
        resumeButtom2.SetActive(false);

        helpText.SetActive(false);
    }

    public void OnRestartButtonClick()
    {
        Application.LoadLevel(Application.loadedLevel);
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
    }

    public void OnHelpGoBackButtonClick()
    {
        resumeButtom2.SetActive(false);
        goBackButtom.SetActive(false);
        helpText.SetActive(false);
        ShowMenu();
    }
}