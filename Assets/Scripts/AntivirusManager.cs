using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using System;

[Serializable]
public struct AntivirusData
{
    public uint computerNetId; // Utilisation de NetworkIdentity NetId
    public float expirationTime;
}

public class AntivirusManager : NetworkBehaviour
{
    public GameObject antivirusMissionUI; // UI 
    public TMP_Text canvasText; // Référence au texte principal pour afficher les informations
    public TMP_Text selectedComputerText; // Texte affichant l'ordinateur sélectionné
    public TMP_Text antivirusLogText; // Texte affichant l'historique
    public float antivirusDuration = 120f; // Durée de l'antivirus en secondes

    // Données synchronisées pour tous les clients
    public readonly SyncList<AntivirusData> syncedAntivirusData = new SyncList<AntivirusData>();

    // Dictionnaires pour le joueur local
    private Dictionary<uint, float> activeAntivirus = new Dictionary<uint, float>();

    /// <summary>
    /// Active l'antivirus sur l'ordinateur sélectionné.
    /// </summary>
    public void ActivateAntivirus()
    {
        GameObject selectedComputer = InteractMissionObject.currentMissionObject;

        if (selectedComputer == null)
        {
            canvasText.text = "Aucun ordinateur sélectionné.";
            return;
        }

        NetworkIdentity computerIdentity = selectedComputer.GetComponent<NetworkIdentity>();
        if (computerIdentity != null)
        {
            var playerRelay = NetworkClient.localPlayer.GetComponent<PlayerNetworkRelay>();
            if (playerRelay != null)
            {
                playerRelay.CmdActivateAntivirus(computerIdentity.netId); // Utilisation de netId
            }
        }
        else
        {
            Debug.LogError("L'ordinateur sélectionné n'a pas de NetworkIdentity.");
        }
    }

    /// <summary>
    /// Server-side method to activate antivirus on a specified computer.
    /// </summary>
    /// <param name="computerNetId">The network ID of the computer to activate the antivirus on.</param>
    [Server]
    public void ServerActivateAntivirus(uint computerNetId)
    {

        float expirationTime = Time.time + antivirusDuration;

        try
        {
            AntivirusData newAntivirusData = new AntivirusData
            {
                computerNetId = computerNetId,
                expirationTime = expirationTime
            };
            syncedAntivirusData.Add(newAntivirusData);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
        }
    }

    /// <summary>
    /// Client-side method to update the antivirus status.
    /// </summary>
    /// <param name="computerNetId">The network ID of the computer.</param>
    /// <param name="expirationTime">The expiration time of the antivirus.</param>
    [ClientRpc]
    private void RpcUpdateAntivirusStatus(uint computerNetId, float expirationTime)
    {
        if (!activeAntivirus.ContainsKey(computerNetId))
        {
            activeAntivirus[computerNetId] = expirationTime;
        }
        UpdateUI();
    }

    /// <summary>
    /// Vérifie si un ordinateur est protégé.
    /// </summary>
    /// <param name="computer">The computer GameObject.</param>
    /// <returns>True if the computer is protected, otherwise false.</returns>
    private bool IsComputerProtected(GameObject computer)
    {
        NetworkIdentity identity = computer.GetComponent<NetworkIdentity>();
        if (identity == null) return false;

        foreach (var data in syncedAntivirusData)
        {
            if (data.computerNetId == identity.netId && data.expirationTime > Time.time)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Affiche et met à jour en temps réel l'état de l'ordinateur actuellement sélectionné.
    /// </summary>
    public void ShowAntivirusStatus()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateAntivirusStatus());
    }

    /// <summary>
    /// Coroutine qui met à jour en continu le statut de l'antivirus.
    /// </summary>
    private IEnumerator UpdateAntivirusStatus()
    {
        while (true)
        {
            GameObject selectedComputer = InteractMissionObject.currentMissionObject;

            if (selectedComputer == null)
            {
                canvasText.text = "Aucun ordinateur sélectionné.";
            }
            else
            {
                NetworkIdentity identity = selectedComputer.GetComponent<NetworkIdentity>();
                if (identity != null && IsComputerProtected(selectedComputer))
                {
                    float timeRemaining = activeAntivirus[identity.netId] - Time.time;

                    if (timeRemaining > 0)
                    {
                        canvasText.text = $"{selectedComputer.name} est protégé. Temps restant : {Mathf.CeilToInt(timeRemaining)}s.";
                        Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                        if (button != null) button.gameObject.SetActive(false);
                    }
                    else
                    {
                        activeAntivirus.Remove(identity.netId);
                        canvasText.text = $"{selectedComputer.name} était protégé, mais l'antivirus a expiré.";
                        Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                        if (button != null) button.gameObject.SetActive(true);
                    }
                }
                else
                {
                    canvasText.text = $"{selectedComputer.name} n'a jamais été protégé.";
                    Transform button = antivirusMissionUI.transform.Find("start/ButtonAntivirus");
                    if (button != null) button.gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Met à jour les textes de l'interface utilisateur.
    /// </summary>
    private void UpdateUI()
    {
        antivirusLogText.text = "Historique des activations :\n";

        foreach (var data in syncedAntivirusData)
        {
            antivirusLogText.text += $"Ordinateur ID {data.computerNetId} activé jusqu'à {FormatTime(data.expirationTime)}\n";
        }

        GameObject selectedComputer = InteractMissionObject.currentMissionObject;
        if (selectedComputer != null)
        {
            NetworkIdentity identity = selectedComputer.GetComponent<NetworkIdentity>();
            selectedComputerText.text = identity != null
                ? $"Ordinateur sélectionné : ID {identity.netId}"
                : "Aucun ordinateur sélectionné.";
        }
        else
        {
            selectedComputerText.text = "Aucun ordinateur sélectionné.";
        }
    }

    /// <summary>
    /// Formate le temps en minutes:secondes.
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds to format.</param>
    /// <returns>A formatted time string.</returns>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
