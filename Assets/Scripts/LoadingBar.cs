using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class LoadingBar : MonoBehaviour
{

    public GameObject loadingBar;
    public GameObject canvasToShow; // Canvas ou GameObject à activer
    public GameObject currentCanvas; // Canvas ou GameObject activé

    public float time = 5f; // Temps total pour remplir la barre

    public void Start()
    {
        if (loadingBar != null)
        {
            Debug.Log("Start: loadingBar found");
            loadingBar.SetActive(true); // Assure-toi qu'il est activé avant de commencer l'animation
            loadingBar.transform.localScale = new Vector3(0, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z);
        }
        else
        {
            Debug.LogError("Start: loadingBar not found");
        }
    }

    public void AnimateBar()
    {
        // if (!isLocalPlayer) // Assure-toi que l'animation ne se lance que pour le client local
        // {
        //     Debug.Log("AnimateBar: not local player");
        //     return;
        // }
        Debug.Log("AnimateBar");
        LeanTween.scaleX(loadingBar, 1, time).setOnComplete(OnComplete);

        //OnComplete();
    }

    public void OnComplete()
    {
        canvasToShow.SetActive(true);
        currentCanvas.SetActive(false);
        LeanTween.scaleX(loadingBar, 0, 0);

        // if (canvasToShow.name == "malware")
        // {
        //     MalwareManager malwareManager = FindObjectOfType<MalwareManager>();
        //     malwareManager.ActivateMalware();
        // }
        if (currentCanvas.name == "installing")
        {
            AntivirusManager antivirusManager = FindObjectOfType<AntivirusManager>();
            if (antivirusManager == null)
            {
                Debug.LogError("AntivirusManager non trouvé dans la scène au niveau du loading.");
                return;
            }
            Debug.Log("OnComplete antivirusManager found");

            antivirusManager.ActivateAntivirus();
            //antivirusManager.ShowAntivirusStatus();
        }

    }

    public void Deactivate()
    {
        LeanTween.cancel(loadingBar);
        LeanTween.scaleX(loadingBar, 0, 0);
    }

}
