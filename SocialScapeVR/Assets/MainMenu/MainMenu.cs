using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public Button buttonToSelectInStartMenu;
    public Button buttonToSelectInOptionsMenu;

    void Start()
    {
        buttonToSelectInStartMenu.Select();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadOptions()
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        buttonToSelectInOptionsMenu.Select();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }  

    public void Back()
    {
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        buttonToSelectInStartMenu.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}