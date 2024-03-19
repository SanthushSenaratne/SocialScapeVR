using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class SpeechAnalysis : MonoBehaviour
{
  public static readonly string[] anxietyIndicators = {
    "um", "like", "i dont know", "you know what I mean",
    "i guess", "well", "uh", "right", "ummm",
    "uh-huh", "ahh", "you know",
    "i think", "maybe", "kind of", "sort of",
    "to be honest", "you see", "the thing is", "im not sure"
  };

  public static int AnalyzeSpeech(string text)
  {
    text = PreprocessText(text);
    Debug.Log("Preprocessed text: " + text);
    int anxietyScore = CountKeywordOccurrences(text, anxietyIndicators);
    return anxietyScore;
  }

  // Function for text preprocessing 
  private static string PreprocessText(string text)
  {
    // Convert to lowercase for case-insensitive matching
    text = text.ToLower();
    // Remove punctuation
    text = Regex.Replace(text, "[^a-zA-Z0-9\\s]", "");

    return text;
  }

  // Function to count keyword occurrences
  private static int CountKeywordOccurrences(string text, string[] keywords)
    {
    int count = 0;
    foreach (string keyphrase in keywords)
    {
        string pattern = @"\b" + keyphrase.Replace(" ", @"\s+") + @"\b"; 
        count += Regex.Matches(text, pattern).Count;
    }
    return count;
    }
}


