using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using StarterAssets;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText; // Texte pour le résultat

    private void Start()
    {
        // Récupérer l'équipe gagnante
        PlayerRole winningTeam = GameInfo.Instance.WinningTeam;
        PlayerRole playerRole = GameInfo.Instance.PlayerRole;

        Debug.Log($"L'équipe gagnante est : {winningTeam}");


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
