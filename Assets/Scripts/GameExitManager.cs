
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameExitManager : MonoBehaviour
{
    /// <summary>
    /// Called when the exit button is clicked.
    /// Stops the server or client and returns to the StarterMenu scene.
    /// </summary>
    public void OnExitButtonClicked()
    {
        if (NetworkServer.active && NetworkClient.isConnected) // Si c'est l'hôte
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected) // Si c'est un client
        {
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
