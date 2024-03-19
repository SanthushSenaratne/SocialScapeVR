using OpenAI;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private TMP_Dropdown microphoneDropdown; 
        [SerializeField] private TMP_Text message; 

        private readonly string fileName = "output.wav";
        private readonly int duration = 10;

        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi("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");

        public delegate void OnTranscriptionCompleted(string text);
        public static event OnTranscriptionCompleted TranscriptionCompleted;


        private void Start()
        {
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
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        private async void ToggleRecording()
        {
            if (isRecording)
            {
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

                if (TranscriptionCompleted != null)
                {
                TranscriptionCompleted(res.Text);
                }

                progressBar.fillAmount = 0;
            }
            else
            {
                isRecording = true;

                var index = PlayerPrefs.GetInt("user-mic-device-index");

                #if !UNITY_WEBGL
                clip = Microphone.Start(microphoneDropdown.options[index].text, false, duration, 44100);
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
        }
    }
}
