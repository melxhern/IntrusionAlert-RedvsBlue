using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Pour manipuler les UI

public class PlayerReady : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReadyStateChanged))]
    public bool isReady = false;

    private Button readyButton;

    /// <summary>
    /// Initialize the player and set up the ready button if the player is local.
    /// </summary>
    private void Start()
    {
        if (isLocalPlayer)
        {
            readyButton = GameObject.Find("ReadyButton")?.GetComponent<Button>();
            if (readyButton != null)
            {
                readyButton.onClick.AddListener(CmdToggleReady);
            }
        }

        if (isServer)
        {
            LobbyManager.RegisterPlayer(this);
        }
    }

    /// <summary>
    /// Command to toggle the player's ready status and check if all players are ready.
    /// </summary>
    [Command]
    private void CmdToggleReady()
    {
        isReady = !isReady;
        LobbyManager.CheckIfAllPlayersReady();
    }

    /// <summary>
    /// Callback when the ready state changes.
    /// Updates the ready button text if the player is local.
    /// </summary>  
    private void OnReadyStateChanged(bool oldState, bool newState)
    {
        if (isLocalPlayer && readyButton != null)
        {
            readyButton.GetComponentInChildren<TMP_Text>().text = newState ? "Pr�t ?" : "Annuler";
        }
    }

    /// <summary>
    /// Unregister the player from the lobby manager when the player is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (isServer)
        {
            LobbyManager.UnregisterPlayer(this);
        }
    }
}
