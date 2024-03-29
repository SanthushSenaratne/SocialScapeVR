using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int level;
    public int wordCount;
    public int disfluencyCount;
    public int fluencyRate;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        wordCount = data.wordCount;
        disfluencyCount = data.disfluencyCount;

        if (wordCount == 0)
        {
            fluencyRate = 0;
        }
        else
        {
            fluencyRate = 100 - (disfluencyCount * 100 / wordCount);
        }

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;
    }

    public void LoadPlayerWithoutPos()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        wordCount = data.wordCount;
        disfluencyCount = data.disfluencyCount;

        if (wordCount == 0)
        {
            fluencyRate = 0;
        }
        else
        {
            fluencyRate = 100 - (disfluencyCount * 100 / wordCount);
        }
    }

    public void ResetPlayer()
    {
        level = 0;
        wordCount = 0;
        disfluencyCount = 0;
        fluencyRate = 0;
    }

    public void CalculateLevel()
    {
        if (wordCount < 100)
        {
            level = 0;
        }
        else
        {
            // Extract the level by removing the last 2 digits and converting to integer
            string levelString = wordCount.ToString().Substring(0, wordCount.ToString().Length - 2);
            level = int.Parse(levelString);
        }
    }
}
