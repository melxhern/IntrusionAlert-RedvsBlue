using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Pour manipuler les UI

//public class PlayerReady : NetworkBehaviour
//{
//    public Button readyButton;  // Le bouton dans l'UI
//    public Text readyButtonText; // Le texte du bouton
//    public bool isReady = false;  // L'état de "prêt" du joueur

//    void Start()
//    {
//        // On désactive le bouton au début, et on l'active après que tout soit configuré.
//        if (readyButton != null)
//        {
//            readyButton.onClick.AddListener(OnReadyButtonClicked);  // Lier le clic au changement d'état
//            UpdateButtonText();  // Mettre à jour le texte du bouton initialement
//        }
//    }

//    public void OnReadyButtonClicked()
//    {
//        // Inverse l'état "Ready" du joueur
//        isReady = !isReady;

//        // Mise à jour du texte du bouton
//        UpdateButtonText();

//        // Informer le serveur de l'état prêt
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
//        // Cette fonction sera appelée sur le serveur pour mettre à jour l'état prêt du joueur.
//        // Tu peux ici envoyer l'état "prêt" du joueur au serveur et le synchroniser pour tous les autres joueurs.
//        RpcUpdateReadyState(ready);
//    }

//    [ClientRpc]
//    void RpcUpdateReadyState(bool ready)
//    {
//        // Cette fonction est appelée sur tous les clients pour mettre à jour l'état du joueur
//        // Cela peut être utilisé pour mettre à jour l'interface de tous les joueurs
//        isReady = ready;
//        UpdateButtonText();
//    }


//}

public class PlayerReady : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReadyStateChanged))]
    public bool isReady = false;

    private Button readyButton;

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

    [Command]
    private void CmdToggleReady()
    {
        isReady = !isReady;
        LobbyManager.CheckIfAllPlayersReady();
    }

    private void OnReadyStateChanged(bool oldState, bool newState)
    {
        if (isLocalPlayer && readyButton != null)
        {
            readyButton.GetComponentInChildren<TMP_Text>().text = newState ? "Prêt ?" : "Annuler";
        }
    }

    private void OnDestroy()
    {
        if (isServer)
        {
            LobbyManager.UnregisterPlayer(this);
        }
    }
}


//public class PlayerReady : NetworkBehaviour
//{
//    public Button readyButton;          // Le bouton Ready local du joueur
//    public TextMeshProUGUI readyText;   // Le texte du bouton, si tu utilises TextMeshPro

//    [SyncVar]
//    public bool isReady = false;        // L'état de "Ready" du joueur, synchronisé sur le serveur

//    public void Start()
//    {
//        if (readyButton == null)
//        {
//            Debug.Log("Le bouton Ready n'a pas été trouvé !");
//        }

//        if (readyText == null)
//        {
//            Debug.Log("Le texte du bouton Ready n'a pas été trouvé !");
//        }
//    }

//    // Appelé lorsque le joueur local commence
//    public override void OnStartLocalPlayer()
//    {
//        base.OnStartLocalPlayer();

//        // Cherche le bouton Ready et le texte associés uniquement pour le joueur local
//        readyButton = GameObject.Find("ReadyButton")?.GetComponent<Button>();
//        readyText = readyButton.GetComponentInChildren<TextMeshProUGUI>();

//        if (readyButton == null)
//        {
//            Debug.LogError("Le bouton Ready n'a pas été trouvé !");
//            return;
//        }

//        if (readyText == null)
//        {
//            Debug.LogError("Le texte du bouton Ready n'a pas été trouvé !");
//            return;
//        }

//        if (readyButton != null)
//        {
//            readyButton.gameObject.SetActive(true);  // Affiche le bouton pour le joueur local
//            readyButton.onClick.AddListener(OnReadyButtonClicked);
//            UpdateReadyButtonText();  // Met à jour le texte initial
//        }
//    }

//    // Cette méthode est appelée lorsque le joueur clique sur le bouton Ready
//    public void OnReadyButtonClicked()
//    {
//        isReady = !isReady;
//        Debug.Log(isReady);
//        Debug.Log("avant la fonction");
//        CmdSetReady(isReady);  // Si le joueur n'est pas prêt, il devient prêt
//        Debug.Log("Apres la fonction");

//    }

//    // Commande envoyée au serveur pour changer l'état "Ready"

//    [Command]
//    public void CmdSetReady(bool readyState)
//    {
//        Debug.Log("CmdSetReady appelé : état prêt = " + readyState);

//        isReady = readyState;
//        UpdateReadyButtonText();
//        LobbyManager.CheckIfAllPlayersReady();
//    }

//    // Cette méthode met à jour le texte du bouton en fonction de l'état du joueur
//    private void UpdateReadyButtonText()
//    {
//        if (readyText != null)
//        {
//            if (isReady)
//            {
//                readyText.text = "Cancel";  // Si le joueur est prêt, le texte devient "Cancel"
//            }
//            else
//            {
//                readyText.text = "Ready";  // Si le joueur n'est pas prêt, le texte reste "Ready"
//            }
//        }
//    }

//    // Appelé lorsque le joueur se déconnecte
//    public override void OnStopClient()
//    {
//        base.OnStopClient();

//        // Cache le bouton si le joueur se déconnecte
//        if (readyButton != null)
//        {
//            readyButton.gameObject.SetActive(false);
//        }
//    }
//}



