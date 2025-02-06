using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using StarterAssets;

public class GameInfo : MonoBehaviour
{
    public static GameInfo Instance { get; private set; }

    public PlayerRole WinningTeam; // L'équipe gagnante
    public PlayerRole PlayerRole; // Le rôle du joueur local (RedTeam, BlueTeam, etc.)

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Ensures that there is only one instance of GameInfo.
    /// </summary>
    private void Awake()
    {
        // Assure-toi qu'il n'y a qu'une seule instance de GameInfo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si une instance existe déjà, détruis celle-ci
        }
        else
        {
            Instance = this; // Assigne l'instance
            DontDestroyOnLoad(gameObject); // Garde l'objet persistant à travers les scènes
        }
    }

}
