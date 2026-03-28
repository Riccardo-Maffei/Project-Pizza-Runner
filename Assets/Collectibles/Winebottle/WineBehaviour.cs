using UnityEngine;
using utils;

public class WineBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. set magic variable in GameData TRUE 
            GameData.ReversedCommands.SetValue(true);

            // 2. Feedback
            Debug.Log("<color=red>WINE COLLECTED! Controls Inverted!</color>");

            // 3. Delete bottle in the scene
            Destroy(gameObject);
        }
    }
}