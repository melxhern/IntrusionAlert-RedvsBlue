using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Nécessaire pour le nouveau système d'entrée


public class InteractUSBKey : MonoBehaviour
{

    public float detectionRadius = 1f; // Rayon de détection
    public string keyTag = "MalwareKey"; // Tag des objets détectables
    public string antivirusTag = "MissionObject"; // Tag des objets détectables
    public static GameObject currentMissionObject = null; // L'objet actuellement sélectionné
    public GameObject playerRightHand;
    public GameObject antivirusUI; // UI à activer

    private GameObject heldObject = null; // L'objet actuellement tenu dans la main

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        GameObject closestObject = null;
        float closestDistance = detectionRadius;

        // Parcourir les objets détectés
        foreach (var hitCollider in hitColliders)
        {

            GameObject detectedObject = hitCollider.gameObject;

            // Ignore l'objet actuellement tenu dans la main
            if (detectedObject == heldObject)
                continue;

            if (detectedObject.CompareTag(keyTag) || detectedObject.CompareTag(antivirusTag))
            {
                float distance = Vector3.Distance(transform.position, detectedObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = detectedObject;
                    Debug.Log("hit collider");
                }
            }
        }

        // Mettre à jour l'outline
        if (currentMissionObject != null && currentMissionObject != closestObject)
        {
            currentMissionObject.GetComponent<Outline>().enabled = false;
            Debug.Log("enabled");
        }

        if (closestObject != null)
        {
            currentMissionObject = closestObject;
            currentMissionObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            currentMissionObject = null;
        }

        // Détecter un clic gauche sur l'objet détecté
        if (Mouse.current.leftButton.wasPressedThisFrame && currentMissionObject != null)
        {

            // Si un objet "MalwareKey" est détecté et cliqué
            if (currentMissionObject != null && currentMissionObject.CompareTag(keyTag))
            {
                PickUpUSBKey();
            }

            // Si un objet "AntivirusMissionObject" est détecté et cliqué
            if (currentMissionObject != null && currentMissionObject.CompareTag(antivirusTag))
            {
                Debug.Log("Antivirus object detected");
                InteractWithAntivirusObject();
            }


        }
    }

    void PickUpUSBKey()
    {
        heldObject = currentMissionObject; // Assigne l'objet tenu
        heldObject.transform.SetParent(playerRightHand.transform);
        heldObject.transform.localPosition = Vector3.zero;
    }

    void InteractWithAntivirusObject()
    {
        // Vérifie si le joueur tient une clé USB dans sa main
        if (heldObject != null && heldObject.CompareTag(keyTag))
        {
            antivirusUI.gameObject.SetActive(true); // Active l'interface antivirus
            Debug.Log("Antivirus UI activated");
        }


    }
}
