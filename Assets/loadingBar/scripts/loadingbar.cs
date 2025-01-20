using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour
{

    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;

    public GameObject canvasToShow; // Canvas ou GameObject à activer
    public GameObject currentCanvas; // Canvas ou GameObject activé


    // Use this for initialization
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }

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

        Debug.Log("current status " + currentComputer.currentStatus);

        if (currentComputer.currentStatus == 1)
        {
            if (currentComputer.currentStatus != 2)
            {
                currentComputer.CmdComputerProtected();
                Debug.Log("Computer protected");
            }
        }
        else if (currentComputer.currentStatus == -1)
        {
            currentComputer.CmdComputerPirated();
            Debug.Log("Computer pirated");
        }

    }

    public void Deactivate()
    {
        imageComp.fillAmount = 0.0f;
    }

}
