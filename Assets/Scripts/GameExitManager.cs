using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameExitManager : MonoBehaviour
{
    public void OnExitButtonClicked()
    {
        if (NetworkServer.active && NetworkClient.isConnected) // Si c'est l'hôte
        {
            Debug.Log("Stopping server and returning to StarterMenu...");
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected) // Si c'est un client
        {
            Debug.Log("Stopping client and returning to StarterMenu...");
            NetworkManager.singleton.StopClient();
        }
        else
        {
            Debug.LogWarning("No active server or client. Returning to StarterMenu...");
        }

        // Charger la scène StarterMenu
        SceneManager.LoadScene("StarterMenu");
    }

}
