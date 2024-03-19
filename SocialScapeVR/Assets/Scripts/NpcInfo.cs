using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Relationship
{
    Classmate,
    Teacher,
    Friend,
    Enemy,
    Family,
    Stranger
}

public enum Age
{
    Nine,
    Thirteen,
    Thirty,
    SixtyFive,
}

public enum Personality
{
    Serious,
    Humorous,
    Sarcastic,
    Shy,
    Outgoing,
    Friendly,
    Rude,
    Kind,
    Mean,
    Caring
}

public class NpcInfo : MonoBehaviour
{
    [SerializeField] private string npcName = "";
    [SerializeField] private Relationship npcRelationship;
    [SerializeField] private Age npcAge;
    [SerializeField] private Personality npcPersonality;
    [TextArea(3, 10)]
    [SerializeField] private string additionalDetails = "";

    public string GetPrompt()
    {
        string prompt = $"**NPC Information:**\n";
        prompt += $"* NPC Name: {npcName}\n";
        prompt += $"* NPC Relationship: {npcRelationship.ToString()}\n";
        prompt += $"* NPC Age: {npcAge.ToString()}\n";
        prompt += $"* NPC Personality: {npcPersonality.ToString()}\n";

        if (!string.IsNullOrEmpty(additionalDetails))
        {
            prompt += $"\n**Additional Details:**\n{additionalDetails}";
        }

        return prompt;
    }
}
