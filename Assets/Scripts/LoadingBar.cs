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
        loadingBar.transform.localScale = new Vector3(0, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z);

    }

    public void AnimateBar()
    {
        LeanTween.scaleX(loadingBar, 1, time).setOnComplete(OnComplete);
        Debug.Log("Barre de chargement animée.");
    }

    public void OnComplete()
    {
        canvasToShow.SetActive(true);
        currentCanvas.SetActive(false);
        LeanTween.scaleX(loadingBar, 0, 0);
    }

    public void Deactivate()
    {
        LeanTween.cancel(loadingBar);
        LeanTween.scaleX(loadingBar, 0, 0);
    }

}
