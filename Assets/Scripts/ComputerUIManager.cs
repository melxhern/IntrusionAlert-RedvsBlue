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

    public TMP_Text protectedLogUIElement;

    // Start is called before the first frame update

    private void ClearAllUI()
    {
        computerUI.SetActive(false); 
        startUI.SetActive(false);
        protectedUI.SetActive(false);
        backgroundUI.SetActive(false);
    }


    public void StartUI()
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        startUI.SetActive(true);
    }

    public void HackStartUI(bool isProtected)
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        hackingUI.SetActive(true);

        
        var usbUI = computerUI.transform.Find("usbInserted").gameObject;
        var loadingScript = usbUI.GetComponent<LoadingBar>();

        loadingScript.currentCanvas = usbUI; // Canvas actuel
        loadingScript.canvasToShow = isProtected ? protectedUI : hackedUI; // Canvas final
        loadingScript.enabled = true; // Active la logique de chargement
        loadingScript.AnimateBar(); // Démarre l'animation de la barre de chargement
    }

    public void HackedUI()
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        hackedUI.SetActive(true);
    }

    public void ProtectedUI(GameObject computer)
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
        protectedUI.SetActive(true);

        protectedLogUI.GetComponent<DynamicTextLoader>().openForComputer(computer);
    }

    public void NoInteractionUI()
    {
        ClearAllUI();
        computerUI.SetActive(true);
        backgroundUI.SetActive(true);
    }
}
