using Amazon;
using System.IO;
using UnityEngine;
using Amazon.Polly;
using Amazon.Runtime;
using Amazon.Polly.Model;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Networking;
public enum VoiceIdEnum
    {
        Ivy,
        Justin,
        Kevin,
        Ruth
    }

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public class Credentials
    {
        public const string ACCESS_KEY = "AKIA6GBMF6JQSEMGTRHJ";
        public const string SECRET_KEY = "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";
    }

    [SerializeField] public VoiceIdEnum voiceId;
    
    public async void MakeAudioRequest(string message, VoiceIdEnum voiceId)
    {

        var credentials = new BasicAWSCredentials(Credentials.ACCESS_KEY, Credentials.SECRET_KEY);
        var client = new AmazonPollyClient(credentials, RegionEndpoint.USEast1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = message,
            Engine = Engine.Neural,
            VoiceId = (VoiceId)voiceId.ToString(),
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);

        WriteIntoFile(response.AudioStream);

        string audioPath;
        
        #if UNITY_ANDROID && !UNITY_EDITOR
            audioPath = $"jar:file://{Application.persistentDataPath}/audio.mp3";
        #elif (UNITY_IOS || UNITY_OSX) && !UNITY_EDITOR
            audioPath = $"file://{Application.persistentDataPath}/audio.mp3";
        #else
            audioPath = $"{Application.persistentDataPath}/audio.mp3";
        #endif
        
        using (var www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
