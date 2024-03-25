using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int level;
    public int wordCount;
    public int disfluencyCount;
    public float[] position;

    public PlayerData(Player player)
    {
        level = player.level;
        wordCount = player.wordCount;
        disfluencyCount = player.disfluencyCount;
        
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
