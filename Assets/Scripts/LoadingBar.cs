using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadingBar : MonoBehaviour
{

    public GameObject loadingBar;
    public GameObject canvasToShow; // Canvas ou GameObject à activer
    public GameObject currentCanvas; // Canvas ou GameObject activé

    public float time = 5f; // Temps total pour remplir la barre

    private Vector3 originalScale; // Échelle originale de la barre
    private float elapsedTime = 0f; // Temps écoulé depuis le début
    private bool isLoadingComplete = false;


    // Start is called before the first frame update
    void Start()
    {

        ResetLoadingBar(); // Réinitialise dès le départ
        //enabled = true; // Réactiver le script de mise à jour

    }

    // Réinitialise la barre de chargement
    public void ResetLoadingBar()
    {
        elapsedTime = 0f;
        originalScale = loadingBar.transform.localScale;
        isLoadingComplete = false;
        loadingBar.transform.localScale = new Vector3(0, originalScale.y, originalScale.z);
        Debug.Log("Barre de chargement réinitialisée.");
    }

    void Update()
    {
        if (!isLoadingComplete)
        {
            // Augmenter le temps écoulé
            elapsedTime += Time.deltaTime;

            // Calculer le facteur de progression
            float progress = Mathf.Clamp01(elapsedTime / time);
            loadingBar.transform.localScale = new Vector3(progress * originalScale.x, originalScale.y, originalScale.z);

            // Si le chargement est terminé, activer le Canvas
            if (elapsedTime >= time)
            {
                isLoadingComplete = true;
                canvasToShow.SetActive(true);
                currentCanvas.SetActive(false);
            }
        }
    }


    public void Deactivate()
    {
        ResetLoadingBar();
        this.enabled = false;
    }
}
