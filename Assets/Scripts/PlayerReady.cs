using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Pour manipuler les UI

//public class PlayerReady : NetworkBehaviour
//{
//    public Button readyButton;  // Le bouton dans l'UI
//    public Text readyButtonText; // Le texte du bouton
//    public bool isReady = false;  // L'�tat de "pr�t" du joueur

//    void Start()
//    {
//        // On d�sactive le bouton au d�but, et on l'active apr�s que tout soit configur�.
//        if (readyButton != null)
//        {
//            readyButton.onClick.AddListener(OnReadyButtonClicked);  // Lier le clic au changement d'�tat
//            UpdateButtonText();  // Mettre � jour le texte du bouton initialement
//        }
//    }

//    public void OnReadyButtonClicked()
//    {
//        // Inverse l'�tat "Ready" du joueur
//        isReady = !isReady;

//        // Mise � jour du texte du bouton
//        UpdateButtonText();

//        // Informer le serveur de l'�tat pr�t
//        CmdSetReadyState(isReady);
//    }

//    void UpdateButtonText()
//    {
//        if (readyButtonText != null)
//        {
//            readyButtonText.text = isReady ? "Cancel Ready" : "Ready";
//        }
//    }

//    [Command]
//    void CmdSetReadyState(bool ready)
//    {
//        // Cette fonction sera appel�e sur le serveur pour mettre � jour l'�tat pr�t du joueur.
//        // Tu peux ici envoyer l'�tat "pr�t" du joueur au serveur et le synchroniser pour tous les autres joueurs.
//        RpcUpdateReadyState(ready);
//    }

//    [ClientRpc]
//    void RpcUpdateReadyState(bool ready)
//    {
//        // Cette fonction est appel�e sur tous les clients pour mettre � jour l'�tat du joueur
//        // Cela peut �tre utilis� pour mettre � jour l'interface de tous les joueurs
//        isReady = ready;
//        UpdateButtonText();
//    }


//}

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



