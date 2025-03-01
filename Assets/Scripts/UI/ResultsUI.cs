
using UnityEngine;
using TMPro;
using StarterAssets;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText; // Texte pour le résultat

    /// <summary>
    /// Start is called before the first frame update.
    /// Displays the game result based on the player's role and the winning team.
    /// </summary>
    private void Start()
    {
        // Récupérer l'équipe gagnante
        PlayerRole winningTeam = GameInfo.Instance.WinningTeam;
        PlayerRole playerRole = GameInfo.Instance.PlayerRole;

        // Afficher le message de victoire ou de défaite
        if (playerRole == winningTeam)
        {
            resultText.text = $"Victoire ! \nVotre equipe [{winningTeam}] a gagne la partie !";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = $"Defaite . . . \nL'equipe adverse [{winningTeam}] a gagne la partie.";
            resultText.color = Color.red;
        }
    }
}
