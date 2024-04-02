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
    public GameObject OptionsMenu;
    public Button ButtonToSelectInPause;
    public Button ButtonToSelectInMap;
    public Button ButtonToSelectInOptions;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI ratingText;
    public Slider xpSlider;
    public Slider ratingSlider;
    public TMP_Dropdown microphoneDropdown;
    public bool isPaused; 
        
    void Start()
    {
        pauseMenu.SetActive(false);
        ButtonToSelectInPause.Select();
        
        microphoneDropdown.onValueChanged.AddListener(SetMicrophone);

            #if UNITY_WEBGL && !UNITY_EDITOR
            microphoneDropdown.options.Add(new TMP_Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            foreach (var device in Microphone.devices)
            {
                microphoneDropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
            microphoneDropdown.onValueChanged.AddListener(SetMicrophone);

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            microphoneDropdown.SetValueWithoutNotify(index);
            #endif
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

        if(isPaused)
        {
            if (Input.GetButtonDown("Back"))
            {
                ResumeGame();
            }
        }
        
        Player player = FindObjectOfType<Player>();
        levelText.text = "Level: " + player.level;
        xpText.text = player.xp + "/100";
        xpSlider.value = (float)player.xp / 100;

        ratingText.text = player.fluencyRate + "%";  
        ratingSlider.value = (float)player.fluencyRate / 100; 
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;   

        Player player = FindObjectOfType<Player>();
        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        playerInteract.StopInteract();
        playerInteract.enabled = false;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Player player = FindObjectOfType<Player>();
        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        playerInteract.enabled = true;
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

    public void LoadOptions()
    {
        pauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        ButtonToSelectInOptions.Select();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }  

    private void SetMicrophone(int index)
    {
        PlayerPrefs.SetInt("user-mic-device-index", index);
    }

    public string GetMicrophoneText()
    {   var index = PlayerPrefs.GetInt("user-mic-device-index");
        string microphoneText = microphoneDropdown.options[index].text;
        Debug.Log(microphoneText);
        return microphoneText;
    }

    public void Back()
    {   if(OptionsMenu.activeSelf)
        {
            OptionsMenu.SetActive(false);
        }
        else if(Map.activeSelf)
        {
            Map.SetActive(false);
        }
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
