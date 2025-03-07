﻿
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using Mirror;

public class LoadingBar : MonoBehaviour
{
    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;

    public GameObject canvasToShow; // Canvas à activer
    public GameObject currentCanvas; // Canvas actuellement activé


    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {

        if (imageComp.fillAmount != 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
        }
        else
        {
            imageComp.fillAmount = 0.0f;
            OnComplete();
        }
    }

    /// <summary>
    /// Called when the loading bar is complete.
    /// </summary>
    public void OnComplete()
    {
        canvasToShow.SetActive(true);
        currentCanvas.SetActive(false);

        var currentMissionObject = InteractMissionObject.currentMissionObject;
        if (currentMissionObject == null)
        {
            Debug.LogError("currentMissionObject not found");
            return;
        }

        var currentComputer = currentMissionObject.GetComponent<Computer>();
        if (currentComputer == null)
        {
            Debug.LogError("currentComputer not found");
            return;
        }

        if (currentComputer.currentStatus == -1)
        {
            currentComputer.CmdComputerPirated();
            var player = NetworkClient.localPlayer?.gameObject.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                player.CmdUpdateIsHoldingKey(false); // Met à jour correctement la valeur
            }
            else
            {
                Debug.LogError("ThirdPersonController introuvable sur l'objet du joueur.");
            }
        }
        else if (currentComputer.currentStatus != 2)
        {
            currentComputer.CmdComputerProtected();
        }
    }

    /// <summary>
    /// Deactivate the loading bar.
    /// </summary>
    public void Deactivate()
    {
        imageComp.fillAmount = 0.0f;
    }

}
