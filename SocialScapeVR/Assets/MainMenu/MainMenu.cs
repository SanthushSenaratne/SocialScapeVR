using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public Button buttonToSelectInStartMenu;
    public Button buttonToSelectInOptionsMenu;
    public TMP_Dropdown microphoneDropdown;

    void Start()
    {
        buttonToSelectInStartMenu.Select();

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

    private void SetMicrophone(int index)
    {
        PlayerPrefs.SetInt("user-mic-device-index", index);
    }

    public string GetMicrophoneText()
    {   
        var index = PlayerPrefs.GetInt("user-mic-device-index");
        string microphoneText = microphoneDropdown.options[index].text;
        Debug.Log(microphoneText);
        return microphoneText;
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