using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerUIManager : MonoBehaviour
{
    public GameObject computerUI;

    public GameObject startUI;
    public GameObject protectedUI;
    public GameObject protectedLogUI;
    public GameObject backgroundUI;
    public GameObject hackingUI;
    public GameObject hackedUI;
    public GameObject notHackedUI;

    //public TMP_Text protectedLogUIElement;


    /// <summary>
    /// Clears all UI elements.
    /// </summary>
    private void ClearAllUI()
    {
        computerUI.SetActive(false);
        startUI.SetActive(false);
        protectedUI.SetActive(false);
        backgroundUI.SetActive(false);
    }

    /// <summary>
    /// Activates the start UI.
    /// </summary>
    public void StartUI()
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        startUI.SetActive(true);
    }

    /// <summary>
    /// Activates the hacking UI.
    /// </summary>
    /// <param name="isProtected">Indicates if the computer is protected.</param>
    public void HackStartUI(bool isProtected)
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        hackingUI.SetActive(true);

        var usbUI = computerUI.transform.Find("usbInserted").gameObject;
        var usbLoadUI = computerUI.transform.Find("usbInserted/loading/vica").gameObject;
        var loadingScript = usbLoadUI.GetComponent<LoadingBar>();

        loadingScript.currentCanvas = usbUI; // Canvas actuel
        loadingScript.canvasToShow = isProtected ? notHackedUI : hackedUI; // Canvas final
        loadingScript.enabled = true; // Active la logique de chargement
    }

    /// <summary>
    /// Activates the hacked UI.
    /// </summary>
    public void HackedUI()
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        hackedUI.SetActive(true);
    }

    /// <summary>
    /// Activates the protected UI and starts the antivirus timer.
    /// </summary>
    /// <param name="computer">The computer GameObject.</param>
    /// <param name="endOfProtectionTime">The time when the protection ends.</param>
    public void ProtectedUI(GameObject computer, double endOfProtectionTime)
    {
        // Active l'UI d'abord
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        protectedUI.SetActive(true);

        Canvas.ForceUpdateCanvases();

        // Calcul du temps restant
        float timeRemaining = Mathf.Max(0, (float)(endOfProtectionTime - Mirror.NetworkTime.time));

        // Mettre à jour l'UI avec un timer si nécessaire
        var antivirusTimer = protectedUI.GetComponent<AntivirusTimer>();
        if (antivirusTimer != null)
        {
            antivirusTimer.StartAntivirusTimer((float)Mirror.NetworkTime.time, (float)endOfProtectionTime);
        }
        else
        {
            Debug.LogError("AntivirusTimer script is missing on the protectedUI GameObject.");
        }

    }

    /// <summary>
    /// Activates the no interaction UI.
    /// </summary>
    public void NoInteractionUI()
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
    }
}
