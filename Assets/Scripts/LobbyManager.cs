using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public Button StartGameButton;
    private static List<PlayerReady> playersList = new List<PlayerReady>();

    public static void RegisterPlayer(PlayerReady player)
    {
        if (player == null)
        {
            Debug.LogError("Le joueur que l'on essaie d'enregistrer est null !");
            return;
        }
        if (!playersList.Contains(player))
        {
            playersList.Add(player);
        }
    }

    public static void UnregisterPlayer(PlayerReady player)
    {
        if (playersList.Contains(player))
        {
            playersList.Remove(player);
        }
    }

    public static void CheckIfAllPlayersReady()
    {
        if (playersList.Count == 0) return;

        foreach (var player in playersList)
        {
            if (!player.isReady)
            {
                return; // Un joueur n'est pas pret, on ne fait rien
            }
        }

        // Si on arrive ici, tous les joueurs sont prets
        NetworkManager.singleton.ServerChangeScene("Game");
    }

    private void Start()
    {
        if (StartGameButton == null)
        {
            StartGameButton = GameObject.Find("ReadyButton")?.GetComponent<Button>();
        }
        else {
            StartGameButton.gameObject.SetActive(NetworkClient.active);
        }
    }
}

