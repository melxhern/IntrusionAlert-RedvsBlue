using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AntivirusManager : MonoBehaviour
{
    public GameObject antivirusMissionUI; // UI 
    public TMP_Text canvasText; // Référence au texte principal pour afficher les informations
    public TMP_Text selectedComputerText; // Texte affichant l'ordinateur sélectionné
    public TMP_Text antivirusLogText; // Texte affichant l'historique
    public float antivirusDuration = 120f; // Durée de l'antivirus en secondes

    public Dictionary<GameObject, float> activeAntivirus = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, float> antivirusActivationLog = new Dictionary<GameObject, float>();

    /// <summary>
    /// Active l'antivirus sur l'ordinateur sélectionné.
    /// </summary>
    public void ActivateAntivirus()
    {
        // Affiche l'ordinateur sélectionné si n'a pas la clé USB
        GameObject selectedComputer = InteractUSBKey.currentMissionObject != null
            ? InteractUSBKey.currentMissionObject
            : InteractMissionObject.currentMissionObject;


        if (selectedComputer == null)
        {
            canvasText.text = "Aucun ordinateur sélectionné.";
            return;
        }

        // Active l'antivirus pour l'ordinateur sélectionné
        activeAntivirus[selectedComputer] = Time.time + antivirusDuration;

        // Enregistre dans l'historique
        if (!antivirusActivationLog.ContainsKey(selectedComputer))
        {
            antivirusActivationLog[selectedComputer] = Time.time;
        }

        UpdateUI(); // Met à jour l'interface utilisateur
    }

    /// <summary>
    /// Affiche et met à jour en temps réel l'état de l'ordinateur actuellement sélectionné.
    /// </summary>
    public void ShowAntivirusStatus()
    {
        // Arrête toute coroutine existante pour éviter des conflits.
        StopAllCoroutines();

        // Lancer une coroutine pour mettre à jour l'état régulièrement.
        StartCoroutine(UpdateAntivirusStatus());
    }

    /// <summary>
    /// Coroutine qui met à jour en continu le statut de l'antivirus.
    /// </summary>
    private IEnumerator UpdateAntivirusStatus()
    {
        while (true)
        {
            // Affiche l'ordinateur sélectionné si n'a pas la clé USB
            GameObject selectedComputer = InteractUSBKey.currentMissionObject != null
                ? InteractUSBKey.currentMissionObject
                : InteractMissionObject.currentMissionObject;

            MalwareManager malwareManager = FindObjectOfType<MalwareManager>();

            if (selectedComputer == null)
            {
                canvasText.text = "Aucun ordinateur sélectionné.";
            }
            else if (activeAntivirus.ContainsKey(selectedComputer))
            {
                float timeRemaining = activeAntivirus[selectedComputer] - Time.time;

                if (timeRemaining > 0)
                {
                    canvasText.text = $"{selectedComputer.name} est protégé. Temps restant : {Mathf.CeilToInt(timeRemaining)}s.";
                    // Rendre le bouton invisible
                    //Transform image = antivirusMissionUI.transform.Find("start/test");
                    //image.gameObject.SetActive(true);
                    Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                    button.gameObject.SetActive(false);
                }
                else
                {
                    // Retirer l'ordinateur de la liste active lorsqu'il n'est plus protégé.
                    activeAntivirus.Remove(selectedComputer);
                    canvasText.text = $"{selectedComputer.name} était protégé, mais l'antivirus a expiré.";
                    //Transform image = antivirusMissionUI.transform.Find("start/test");
                    //image.gameObject.SetActive(false);
                    // Rendre le bouton visible
                    Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                    button.gameObject.SetActive(true);
                }
            }

            else if (antivirusActivationLog.ContainsKey(selectedComputer))
            {
                canvasText.text = $"{selectedComputer.name} était protégé, mais l'antivirus a expiré.";
                Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                button.gameObject.SetActive(true);
            }

            else if (malwareManager.IsInfected(selectedComputer))
            {
                GameObject malware = antivirusMissionUI.transform.Find("malware").gameObject;
                malware.gameObject.SetActive(true);
            }

            else
            {
                canvasText.text = $"{selectedComputer.name} n'a jamais été protégé.";
                Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                button.gameObject.SetActive(true);
            }

            // Attendre 1 seconde avant de mettre à jour à nouveau.
            yield return new WaitForSeconds(1f);
        }
    }


    /// <summary>
    /// Met à jour les textes de l'interface utilisateur.
    /// </summary>
    private void UpdateUI()
    {
        // Met à jour le texte de l'historique
        antivirusLogText.text = "Historique des activations :\n";

        foreach (var log in antivirusActivationLog)
        {
            antivirusLogText.text += $"{log.Key.name} activé à {FormatTime(log.Value)}\n";
        }

        // Affiche l'ordinateur sélectionné si n'a pas la clé USB
        GameObject selectedComputer = InteractUSBKey.currentMissionObject != null
            ? InteractUSBKey.currentMissionObject
            : InteractMissionObject.currentMissionObject;

        if (selectedComputer != null)
        {
            selectedComputerText.text = $"Ordinateur sélectionné : {selectedComputer.name}";
        }
        else
        {
            selectedComputerText.text = "Aucun ordinateur sélectionné.";
        }
    }

    /// <summary>
    /// Formate le temps en minutes:secondes.
    /// </summary>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public bool IsComputerProtected(GameObject computer)
    {
        return activeAntivirus.ContainsKey(computer) && activeAntivirus[computer] > Time.time;
    }



}
