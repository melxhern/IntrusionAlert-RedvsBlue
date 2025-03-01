
using UnityEngine;
using TMPro;
using Mirror;
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


    /// <summary>
    /// Start is called before the first frame update.
    /// Hides the result canvas at the beginning.
    /// </summary>
    private void Start()
    {
        if (resultCanvas != null)
        {
            resultCanvas.SetActive(false); // Cache le Canvas au début
        }
    }

    /// <summary>
    /// Checks if all computers in the scene are hacked.
    /// </summary>
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


    /// <summary>
    /// Checks the game result based on the status of all computers.
    /// </summary>
    public void CheckGameResult()
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
        PlayerRole winningTeam = allHacked ? PlayerRole.RedTeam : PlayerRole.BlueTeam;
        DisplayResult(winningTeam);
    }

    /// <summary>
    /// Displays the result of the game.
    /// </summary>
    /// <param name="winningTeam">The team that won the game.</param>
    private void DisplayResult(PlayerRole winningTeam)
    {
        // Sauvegarder l'équipe gagnante pour la scène "Results"
        GameInfo.Instance.WinningTeam = winningTeam;

        // Optionnel : Sauvegarder aussi le rôle du joueur local dans GameInfo
        var localPlayer = NetworkClient.localPlayer.GetComponent<ThirdPersonController>();
        GameInfo.Instance.PlayerRole = localPlayer.GetRole();

        // Charger la scène "Results" pour tous les joueurs
        RpcLoadResultsScene();
    }

    /// <summary>
    /// RPC to load the "Results" scene on all clients.
    /// </summary>
    [ClientRpc]
    private void RpcLoadResultsScene()
    {
        SceneManager.LoadScene("Results");
    }


}
