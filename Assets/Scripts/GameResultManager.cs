using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.SceneManagement;

public static class GameResultData
{
    public static PlayerRole WinningTeam; // L'équipe gagnante
}



public class GameResultManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText; // Texte affiché sur le Canvas pour le résultat
    [SerializeField] private GameObject resultCanvas;   // Canvas à afficher

    private void Start()
    {
        if (resultCanvas != null)
        {
            resultCanvas.SetActive(false); // Cache le Canvas au début
        }
    }

    public void CheckIfAllComputersHacked()
    {
        // Récupère tous les ordinateurs de la scène
        var computers = FindObjectsOfType<Computer>();

        if (computers.Length == 0)
        {
            Debug.LogWarning("Aucun ordinateur trouvé dans la scène !");
            return;
        }

        // Vérifie si tous les ordinateurs sont hackés
        bool allHacked = true;
        foreach (var computer in computers)
        {
            if (computer.currentStatus != -2) // -2 signifie "hacké"
            {
                allHacked = false;
                break;
            }
        }

        // Détermine le résultat
        if (allHacked)
        {
            // RedTeam gagne
            DisplayResult(PlayerRole.RedTeam);
        }

    }


    // public void CheckGameResult()
    // {
    //     // Récupère tous les ordinateurs de la scène
    //     var computers = FindObjectsOfType<Computer>();

    //     if (computers.Length == 0)
    //     {
    //         Debug.LogWarning("Aucun ordinateur trouvé dans la scène !");
    //         return;
    //     }

    //     // Vérifie si tous les ordinateurs sont hackés
    //     bool allHacked = true;
    //     foreach (var computer in computers)
    //     {
    //         if (computer.currentStatus != -2) // -2 signifie "hacké"
    //         {
    //             allHacked = false;
    //             break;
    //         }
    //     }

    //     // Détermine le résultat
    //     if (allHacked)
    //     {
    //         // RedTeam gagne
    //         DisplayResult(PlayerRole.RedTeam);
    //     }
    //     else
    //     {
    //         // BlueTeam gagne
    //         DisplayResult(PlayerRole.BlueTeam);
    //     }
    // }

    // private void DisplayResult(PlayerRole winningTeam)
    // {
    //     var localPlayer = NetworkClient.localPlayer.GetComponent<ThirdPersonController>();
    //     PlayerRole playerRole = localPlayer.GetRole();

    //     if (resultCanvas != null && resultText != null)
    //     {
    //         resultCanvas.SetActive(true);

    //         if (playerRole == winningTeam)
    //         {
    //             resultText.text = $"Victoire ! \nVotre equipe [{winningTeam}] a gagne la partie ! ";
    //             resultText.color = Color.green;
    //         }
    //         else
    //         {
    //             resultText.text = $"DEFAITE . . . \nL'EQUIPE ADVERSE [{winningTeam}] A GAGNE LA PARTIE . . . ";
    //             resultText.color = Color.red;
    //         }
    //     }

    //     Debug.Log($"L'équipe gagnante est : {winningTeam}");
    // }

    public void CheckGameResult()
    {
        //Debug.Log("is server dans check game result" + isServer);
        //if (!isServer) return; // Seul le serveur peut déterminer le résultat
        Debug.Log("Vérification du résultat du jeu...");
        // Récupère tous les ordinateurs de la scène
        var computers = FindObjectsOfType<Computer>();

        if (computers.Length == 0)
        {
            Debug.LogWarning("Aucun ordinateur trouvé dans la scène !");
            return;
        }

        // Vérifie si tous les ordinateurs sont hackés
        bool allHacked = true;
        foreach (var computer in computers)
        {
            if (computer.currentStatus != -2) // -2 signifie "hacké"
            {
                allHacked = false;
                break;
            }
        }

        // Détermine le résultat
        PlayerRole winningTeam = allHacked ? PlayerRole.RedTeam : PlayerRole.BlueTeam;
        DisplayResult(winningTeam);
    }


    private void DisplayResult(PlayerRole winningTeam)
    {
        // Sauvegarder l'équipe gagnante pour la scène "Results"
        //GameResultData.WinningTeam = winningTeam;
        GameInfo.Instance.WinningTeam = winningTeam;

        Debug.Log("L'équipe gagnante est : " + GameInfo.Instance.WinningTeam);

        // Optionnel : Sauvegarder aussi le rôle du joueur local dans GameInfo
        var localPlayer = NetworkClient.localPlayer.GetComponent<ThirdPersonController>();
        Debug.Log("Le joueur local est de l'équipe : " + localPlayer.GetRole());
        GameInfo.Instance.PlayerRole = localPlayer.GetRole();

        Debug.Log("Le joueur local est de l'équipe : " + GameInfo.Instance.PlayerRole);

        // Charger la scène "Results" pour tous les joueurs
        RpcLoadResultsScene();
    }

    // RPC pour charger la scène "Results" sur tous les clients
    [ClientRpc]
    private void RpcLoadResultsScene()
    {
        Debug.Log("Chargement de la scène des résultats...");

        SceneManager.LoadScene("Results");
    }


}
