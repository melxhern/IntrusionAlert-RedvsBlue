using Assets.Scripts;
using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;

public class BeginUI : MonoBehaviour
{
    public CanvasGroup Group;
    public TMP_Text RoleText;
    public TMP_Text Postit;
    public GameObject Panel;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStarted += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= OnGameStart;
    }

    /// <summary>
    /// Called when the game starts.
    /// Initializes the UI.
    /// </summary>
    private void OnGameStart()
    {
        StartCoroutine(InitializeUI());
    }

    /// <summary>
    /// Coroutine to initialize the UI.
    /// Waits for a short delay to ensure SyncVars are propagated.
    /// </summary>
    private IEnumerator InitializeUI()
    {
        // Attends un court délai pour que les SyncVars soient propagées
        yield return new WaitForSeconds(2);

        //Panel.SetActive(false);
        ThirdPersonController player = ThirdPersonController.Local;
        RoleText.text = $"Vous êtes {player.Role.ToString().ToLower()}";

        if (player.Role == PlayerRole.BlueTeam) Postit.text = "Votre rôle: protéger le réseau interne, \nVotre mission: vérifier et mettre en place les firewalls";

        if (player.Role == PlayerRole.RedTeam) Postit.text = "Votre rôle: entrer dans le réseau interne, \nVotre mission: insérer dans les ordinateurs non protégés les clés USB inféctées.";

        StartCoroutine(DoFade());

        StartCoroutine(WaitAndShowPanel(4f));
    }

    /// <summary>
    /// Coroutine to fade out the UI.
    /// </summary>
    private IEnumerator DoFade()
    {
        yield return new WaitForSeconds(2);

        float time = 0, duration = 2;
        while (time < duration)
        {
            Group.alpha = Mathf.Lerp(1, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Group.alpha = 0;
        Group.blocksRaycasts = false;
        Group.interactable = false;
    }

    /// <summary>
    /// Coroutine to wait and show the panel.
    /// </summary>
    /// <param name="waitTime">The time to wait before showing the panel.</param>
    private IEnumerator WaitAndShowPanel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (Panel != null) Panel.SetActive(true);
    }
}
