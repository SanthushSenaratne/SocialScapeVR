using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    public string scenename;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Player player = FindObjectOfType<Player>();
            player.SavePlayer();
            Debug.Log("Game Saved");
            
            SceneManager.LoadScene(scenename);
        }
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();

        player.LoadPlayerWithoutPos();
        Debug.Log("Player data loaded after scene switch");

        player.SavePlayer();

    }
}
