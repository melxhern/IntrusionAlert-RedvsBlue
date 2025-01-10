using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Nécessaire pour le nouveau système d'entrée
using Mirror;
using System.Linq;


public class InteractMissionObject : NetworkBehaviour
{
    public float detectionRadius = 0.1f; // Rayon de détection
    public string missionObjectTag = "MissionObject"; // Tag des objets détectables
    public GameObject antivirusMissionUI; // UI à activer
    public static GameObject currentMissionObject = null; // L'objet actuellement sélectionné

    private AntivirusManager antivirusManager;




    void Start()
    {

        // Désactiver le script si ce n'est pas le joueur local
        if (!isLocalPlayer)
        {
            enabled = false;
            Debug.Log("player is not local");
            return;
        }

        Debug.Log("InteractMissionObject script activé !");

        antivirusManager = FindObjectOfType<AntivirusManager>();
        if (antivirusManager == null)
        {
            Debug.LogError("AntivirusManager introuvable dans la scène !");
        }

        antivirusMissionUI = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(obj => obj.name == "missionAntivirus"); // permet de trouver l'objet même si désactivé
        if (antivirusMissionUI == null)
        {
            Debug.LogError("L'UI AntivirusMissionUI est introuvable !");
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Trouver tous les objets dans le rayon
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        //Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, detectionRadius);


        GameObject closestObject = null;
        float closestDistance = detectionRadius;

        // Parcourir les objets détectés
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(missionObjectTag))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = hitCollider.gameObject;
                }
            }
        }

        // Mettre à jour l'outline
        if (currentMissionObject != null && currentMissionObject != closestObject)
        {
            currentMissionObject.GetComponent<Outline>().enabled = false;
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
            // Activer le canvas principal
            antivirusMissionUI.SetActive(true);
        }
    }


}
