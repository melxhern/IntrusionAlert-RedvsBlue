
using UnityEngine;

public class ComputerUIManager : MonoBehaviour
{
    public GameObject computerUI; // main UI for the computer
    public GameObject startUI; // first UI when blue team connected
    public GameObject protectedUI; // blue team successfully added the antivirus
    public GameObject protectedLogUI; // log of the antivirus
    public GameObject backgroundUI; // background of the computer
    public GameObject hackingUI; // when the USB is inserted
    public GameObject hackedUI; // when malware is installed
    public GameObject notHackedUI; // if the computer was protected during the attack --> hacking failed

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
