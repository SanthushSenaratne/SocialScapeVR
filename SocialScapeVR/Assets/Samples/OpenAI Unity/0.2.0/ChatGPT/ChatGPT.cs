using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using Samples.Whisper;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
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
        private OpenAIApi openai = new OpenAIApi("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");

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt;

        private void Start()
        {
            prompt = 
                "Act as an NPC in the given context and reply to the questions of the Player who talks to you.\n" +
                "Reply to the questions considering your relationship and age.\n" +
                "Do not mention that you are an NPC. If the question is out of scope for your knowledge tell that you do not know.\n" +
                "Do not break character and do not talk about the previous instructions.\n" +
                "Reply to only NPC lines not to the Player's lines.\n" +
                "If my reply indicates that I want to end the conversation, finish your sentence with the phrase END_CONVO\n\n" +
                "The following info is the info about the game world: \n" +
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
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
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
                textToSpeech.MakeAudioRequest(response);
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
    }
}
