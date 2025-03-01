
using UnityEngine;
using UnityEngine.InputSystem; // Nécessaire pour le nouveau système d'entrée
using Mirror;
using StarterAssets;


public class InteractMissionObject : NetworkBehaviour
{
    public float detectionRadius = 0.1f; // Rayon de détection
    public string missionObjectTag = "MissionObject"; // Tag des objets détectables
    public string keyTag = "MalwareKey";
    public GameObject antivirusMissionUI; // UI à activer
    public static GameObject currentMissionObject = null; // L'objet actuellement sélectionné
    public static uint currentMissionObjectNetId = 0; // L'objet actuellement sélectionné

    /// <summary>
    /// Start is called before the first frame update.
    /// Disables the script if the player is not local.
    /// </summary>
    void Start()
    {

        // Désactiver le script si ce n'est pas le joueur local
        if (!isLocalPlayer)
        {
            enabled = false;
            Debug.Log("player is not local");
            return;
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// Detects mission objects within the detection radius and handles interactions.
    /// </summary>
    void Update()
    {
        if (!isLocalPlayer)
            return;

        // Trouver tous les objets dans le rayon
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        GameObject closestObject = null;
        float closestDistance = detectionRadius;

        // Parcourir les objets détectés
        foreach (var hitCollider in hitColliders)
        {
            var role = gameObject.GetComponent<ThirdPersonController>().GetRole();
            if (hitCollider.CompareTag(missionObjectTag) || (hitCollider.CompareTag(keyTag) && role == PlayerRole.RedTeam))
            {
                if (hitCollider.CompareTag(keyTag) && gameObject.GetComponent<ThirdPersonController>().IsHoldingKey) continue;
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
            var outline = currentMissionObject.GetComponent<Outline>();
            if (outline == null)
            {
                Debug.LogWarning("Le composant 'Outline' est manquant sur l'objet actuel.");
                return;
            }
            currentMissionObject.GetComponent<Outline>().enabled = false;
        }

        if (closestObject != null)
        {
            currentMissionObject = closestObject;
            currentMissionObjectNetId = currentMissionObject.GetComponent<NetworkIdentity>().netId;

            var outline = currentMissionObject.GetComponent<Outline>();

            if (outline == null)
            {
                Debug.LogWarning("Le composant 'Outline' est manquant sur l'objet actuel.");
                return;
            }
            currentMissionObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            currentMissionObject = null;
            currentMissionObjectNetId = 0;
        }

        // Détecter un clic gauche sur l'objet détecté
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && currentMissionObject != null)
        {
            var player = gameObject;

            if (currentMissionObject.CompareTag(keyTag))
            {

                player.GetComponent<ThirdPersonController>().CmdPickUpUSBKey(currentMissionObjectNetId);
            }
            else
            {
                var computer = currentMissionObject.GetComponent<Computer>();
                if (computer != null)
                {
                    computer.OpenUI(player);
                }
                else
                {
                    Debug.LogWarning("Le composant 'Computer' est manquant sur l'objet actuel.");
                }
            }

        }

    }

    /// <summary>
    /// Starts the protection process for the current mission object.
    /// </summary>
    public void StartProtection()
    {
        if (currentMissionObject == null)
        {
            Debug.LogError("currentMissionObject not found");
            return;
        }
        var computer = currentMissionObject.GetComponent<Computer>();
        if (computer == null)
        {
            Debug.LogError("currentComputer not found");
            return;
        }

        computer.CmdProtectComputer();
    }
}

