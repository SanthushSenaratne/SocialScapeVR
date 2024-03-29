using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject Map;
    public Button ButtonToSelectInPause;
    public Button ButtonToSelectInMap;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public bool isPaused; 
        
    void Start()
    {
        pauseMenu.SetActive(false);
        ButtonToSelectInPause.Select();
    }

    void Update()
    {
        if(Input.GetButtonDown("Escape")) 
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        Player player = FindObjectOfType<Player>();
        levelText.text = "Level: " + player.level;
        scoreText.text = "Score: " + player.fluencyRate + "%";  
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

    public void SaveGame()
    {
        Player player = FindObjectOfType<Player>();
        player.SavePlayer();
        Debug.Log("Game Saved");
        Debug.Log("Level: " + player.level);
        Debug.Log("Word count: " + player.wordCount);
        Debug.Log("Disfluency count: " + player.disfluencyCount);
        Debug.Log("Fluency rate: " + player.fluencyRate);
    }

    public void LoadGame()
    {
        Player player = FindObjectOfType<Player>();
        player.LoadPlayer();
        Debug.Log("Game Loaded");
        Debug.Log("Level: " + player.level);
        Debug.Log("Word count: " + player.wordCount);
        Debug.Log("Disfluency count: " + player.disfluencyCount);
        Debug.Log("Fluency rate: " + player.fluencyRate);
    }

    public void Back()
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
