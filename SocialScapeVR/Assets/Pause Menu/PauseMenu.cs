using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject Map;
    public Button ButtonToSelectInPause;
    public Button ButtonToSelectInMap;
    public bool isPaused;
    
        
    void Start()
    {
        pauseMenu.SetActive(false);
        ButtonToSelectInPause.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
       
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
       
    }

    public void LoadMap()
    {
        pauseMenu.SetActive(false);
        Map.SetActive(true);
        ButtonToSelectInMap.Select();
    }

    public void back()
    {
        Map.SetActive(false);
        pauseMenu.SetActive(true);
        ButtonToSelectInPause.Select();

    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
