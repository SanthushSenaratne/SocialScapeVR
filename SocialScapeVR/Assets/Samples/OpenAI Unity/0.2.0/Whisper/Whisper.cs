using OpenAI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Xml.XPath;

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private TMP_Dropdown microphoneDropdown; 
        [SerializeField] private TMP_Text message; 
        [SerializeField] private TMP_Text startText;
        [SerializeField] private TMP_Text stopText;
        PauseMenu pauseMenu;

        private readonly string fileName = "output.wav";
        private readonly int duration = 10;
        private int wordCount = 0;
        private int disfluencyCount = 0;
        private int totalRate = 0;
        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi(APIKeys.OPENAI_API);

        public delegate void OnTranscriptionCompleted(string text);
        public event OnTranscriptionCompleted TranscriptionCompleted;

        private void Start()
        {
            GameObject pauseManager = GameObject.Find("PauseManager");
            pauseMenu = pauseManager.GetComponent<PauseMenu>();

            #if UNITY_WEBGL && !UNITY_EDITOR
            microphoneDropdown.options.Add(new TMP_Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            foreach (var device in Microphone.devices)
            {
                microphoneDropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
            recordButton.onClick.AddListener(ToggleRecording);
            microphoneDropdown.onValueChanged.AddListener(ChangeMicrophone);

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            microphoneDropdown.SetValueWithoutNotify(index);
            #endif

            stopText.enabled = false; 
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        private async void ToggleRecording()
        {
            if (isRecording)
            {
                startText.enabled = true;
                stopText.enabled = false;

                isRecording = false;

                #if !UNITY_WEBGL
                Microphone.End(null);
                #endif

                byte[] data = SaveWav.Save(fileName, clip);

                var req = new CreateAudioTranscriptionsRequest
                {
                    FileData = new FileData() { Data = data, Name = "audio.wav" },
                    Model = "whisper-1",
                    Language = "en"
                };
                var res = await openai.CreateAudioTranscription(req);
                

                progressBar.fillAmount = 0;
                message.text = res.Text;

                string text = SpeechAnalysis.PreprocessText(res.Text);

                wordCount = SpeechAnalysis.CountWords(text);

                disfluencyCount = SpeechAnalysis.CountKeywordOccurrences(text, SpeechAnalysis.disfluencyIndicators);

                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.wordCount += wordCount;
                    player.disfluencyCount += disfluencyCount;
                    player.fluencyRate = 100 - (player.disfluencyCount * 100 / player.wordCount);

                    player.CalculateLevel();
                    player.CalculateXp();
                    player.SavePlayer();
                }
                else
                {
                    Debug.LogWarning("Player object not found to save disfluency rate.");
                }

                if (TranscriptionCompleted != null)
                {
                    TranscriptionCompleted(res.Text);
                }

                progressBar.fillAmount = 0;
            }
            else
            {
                startText.enabled = false;
                stopText.enabled = true;

                isRecording = true;

                #if !UNITY_WEBGL
                clip = Microphone.Start(pauseMenu.GetMicrophoneText(), false, duration, 44100);
                #endif
            }
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;

                if (time >= duration)
                {
                time = 0;
                isRecording = false;
                ToggleRecording();
                }
            }

            if (Input.GetButtonDown("Interact"))
            {
                ToggleRecording();
            }
        }

        public void ResetSpeechDisfluency()
        {
            totalRate = 0;
        }
    }
}
