using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class LobbyManager : MonoBehaviour
//{
//    public Button StartGameButton;

//    // Start is called before the first frame update
//    void Start()
//    {
//        if (StartGameButton == null)
//        {
//            StartGameButton = GameObject.Find("StartGameButton")?.GetComponent<Button>();
//            if (StartGameButton == null)
//            {
//                Debug.LogError("StartGameButton introuvable dans la scène !");
//                return;
//            }
//        }

//        if (NetworkServer.active)
//        {
//            Debug.Log("Le serveur est actif.");
//            StartGameButton.gameObject.SetActive(true);
//            StartGameButton.onClick.AddListener(StartGameClicked);
//        }
//        else
//        {
//            Debug.Log("Le serveur n'est pas actif.");
//            StartGameButton.gameObject.SetActive(false);
//        }
//    }

//    private void StartGameClicked()
//    {
//        NetworkManager.singleton.ServerChangeScene("Game");
//    }

//    void CheckIfAllPlayersReady()
//    {
//        // Vérifie l'état de chaque joueur pour savoir s'ils sont tous prêts.
//        bool allReady = true;

//        foreach (var player in playersList)  // Remplace par la liste des joueurs
//        {
//            if (!player.isReady)
//            {
//                allReady = false;
//                break;
//            }
//        }

//        if (allReady)
//        {
//            // Lancer la scène du jeu
//            NetworkManager.singleton.ServerChangeScene("Game");  // Remplace "Game" par le nom de ta scène de jeu
//        }
//    }






//}

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
                return; // Un joueur n'est pas prêt, on ne fait rien
            }
        }

        // Si on arrive ici, tous les joueurs sont prêts
        NetworkManager.singleton.ServerChangeScene("Game");
    }

    private void Start()
    {
        if (StartGameButton == null)
        {
            StartGameButton = GameObject.Find("ReadyButton")?.GetComponent<Button>();
        }

        if (StartGameButton != null)
        {
            StartGameButton.gameObject.SetActive(NetworkClient.active);
        }
    }
}

