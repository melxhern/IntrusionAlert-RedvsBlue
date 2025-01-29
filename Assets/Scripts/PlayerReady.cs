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
            readyButton.GetComponentInChildren<TMP_Text>().text = newState ? "Pr�t ?" : "Annuler";
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
//    public bool isReady = false;        // L'�tat de "Ready" du joueur, synchronis� sur le serveur

//    public void Start()
//    {
//        if (readyButton == null)
//        {
//            Debug.Log("Le bouton Ready n'a pas �t� trouv� !");
//        }

//        if (readyText == null)
//        {
//            Debug.Log("Le texte du bouton Ready n'a pas �t� trouv� !");
//        }
//    }

//    // Appel� lorsque le joueur local commence
//    public override void OnStartLocalPlayer()
//    {
//        base.OnStartLocalPlayer();

//        // Cherche le bouton Ready et le texte associ�s uniquement pour le joueur local
//        readyButton = GameObject.Find("ReadyButton")?.GetComponent<Button>();
//        readyText = readyButton.GetComponentInChildren<TextMeshProUGUI>();

//        if (readyButton == null)
//        {
//            Debug.LogError("Le bouton Ready n'a pas �t� trouv� !");
//            return;
//        }

//        if (readyText == null)
//        {
//            Debug.LogError("Le texte du bouton Ready n'a pas �t� trouv� !");
//            return;
//        }

//        if (readyButton != null)
//        {
//            readyButton.gameObject.SetActive(true);  // Affiche le bouton pour le joueur local
//            readyButton.onClick.AddListener(OnReadyButtonClicked);
//            UpdateReadyButtonText();  // Met � jour le texte initial
//        }
//    }

//    // Cette m�thode est appel�e lorsque le joueur clique sur le bouton Ready
//    public void OnReadyButtonClicked()
//    {
//        isReady = !isReady;
//        Debug.Log(isReady);
//        Debug.Log("avant la fonction");
//        CmdSetReady(isReady);  // Si le joueur n'est pas pr�t, il devient pr�t
//        Debug.Log("Apres la fonction");

//    }

//    // Commande envoy�e au serveur pour changer l'�tat "Ready"

//    [Command]
//    public void CmdSetReady(bool readyState)
//    {
//        Debug.Log("CmdSetReady appel� : �tat pr�t = " + readyState);

//        isReady = readyState;
//        UpdateReadyButtonText();
//        LobbyManager.CheckIfAllPlayersReady();
//    }

//    // Cette m�thode met � jour le texte du bouton en fonction de l'�tat du joueur
//    private void UpdateReadyButtonText()
//    {
//        if (readyText != null)
//        {
//            if (isReady)
//            {
//                readyText.text = "Cancel";  // Si le joueur est pr�t, le texte devient "Cancel"
//            }
//            else
//            {
//                readyText.text = "Ready";  // Si le joueur n'est pas pr�t, le texte reste "Ready"
//            }
//        }
//    }

//    // Appel� lorsque le joueur se d�connecte
//    public override void OnStopClient()
//    {
//        base.OnStopClient();

//        // Cache le bouton si le joueur se d�connecte
//        if (readyButton != null)
//        {
//            readyButton.gameObject.SetActive(false);
//        }
//    }
//}



