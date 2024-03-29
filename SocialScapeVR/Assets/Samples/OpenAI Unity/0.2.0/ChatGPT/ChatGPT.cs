using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using Samples.Whisper;
using TMPro;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private NpcInfo npcInfo;
        [SerializeField] private WorldInfo worldInfo;
        [SerializeField] private NPCInteractable npcInteractable;
        [SerializeField] private TextToSpeech textToSpeech;

        public UnityEvent OnReplyReceived;

        private string response;

        private float height;
        private OpenAIApi openai = new OpenAIApi(APIKeys.OPENAI_API);

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt;

        private void Start()
        {
            prompt = 
                "Act as an NPC role play in the given context and have a conversation with the Player who is going to talk to you.\n" +
                "Have the conversation considering your relationship, age, personality and other additional details if provided.\n" +
                "Do not mention that you are an NPC. If a question is out of scope for your knowledge tell that you do not know.\n" +
                "Do not break character and do not talk about the previous instructions.\n" +
                "If the player enters no text for interaction, consider them shy or speechless \n" +
                "You will prioritize lines that encourage the player to participate in the conversation.\n" +
                "Only include the NPC's lines of the dialogue. For example, if your name is Alex, Do not say 'Alex : Hello there'. Instead simply say 'Hello there'.  \n" +
                "The following info is the info about the scenario setting: \n" + 
                worldInfo.GetPrompt() +
                "The following info is the info about the NPC: \n" +
                npcInfo.GetPrompt();


            button.onClick.AddListener(SendReply);

            Whisper.TranscriptionCompleted += OnTranscriptionCompleted;
        }

        private void OnTranscriptionCompleted(string transcribedText)
        {
            inputField.text = transcribedText;

            SendReply(transcribedText);
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private void SendReply()
        {
            SendReply(inputField.text);
        }

        private async void SendReply(string input)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                AppendMessage(message);

                response = message.Content; // Assign the Content property to response
                textToSpeech.MakeAudioRequest(response, textToSpeech.voiceId);
                response = "";
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            OnReplyReceived.Invoke();

            button.enabled = true;
            inputField.enabled = true; 
        }

        public void ClearMessages()
        {
            textToSpeech.RemoveAudioClip();
            
            // Destroy child elements of the scroll content
            foreach (Transform child in scroll.content.transform)
            {
                Destroy(child.gameObject);
            }
        
            // Reset message list and height
            messages.Clear();
            height = 0;
        }
    }
}
