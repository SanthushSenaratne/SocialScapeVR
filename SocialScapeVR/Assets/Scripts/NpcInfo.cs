using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Relationship
{
    Classmate
}

public enum Age
{
    Nine
}

public class NpcInfo : MonoBehaviour
{
    [SerializeField] private string npcName = "";
    [SerializeField] private Relationship npcRelationship;
    [SerializeField] private Age npcAge;

    public string GetPrompt()
    {
        return $"NPC Name: {npcName}\n" +
               $"NPC Relationship: {npcRelationship.ToString()}\n" +
               $"NPC Age: {npcAge.ToString()}\n";
    }
}
