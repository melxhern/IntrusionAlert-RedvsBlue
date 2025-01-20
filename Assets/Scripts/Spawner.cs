using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spawner : MonoBehaviour
{
    //public GameObject gameManagerPrefab; // Assigné dans l'Inspector
    public GameObject usbKeyPrefab; // Prefab de la clé USB à assigner dans l'Inspector
    public int usbKeysPerTable = 1; // Nombre de clés USB à spawner par table

    void Start()
    {
        // S'assurer que ce script ne fonctionne que sur le serveur
        if (!NetworkServer.active)
        {
            Debug.LogWarning("Spawner: This script should only be executed on the server.");
            return;
        }

        //SpawnGameManager();
        SpawnUSBKeysOnTables();
    }

    /// <summary>
    /// Spawne le GameManager côté serveur si le prefab est assigné.
    /// </summary>
    // void SpawnGameManager()
    // {
    //     if (gameManagerPrefab != null)
    //     {
    //         GameObject gameManagerInstance = Instantiate(gameManagerPrefab);

    //         // Spawner sur le réseau
    //         NetworkServer.Spawn(gameManagerInstance);

    //         Debug.Log("GameManager spawned on the server!");
    //     }
    //     else
    //     {
    //         Debug.LogError("Spawner: GameManager prefab is not assigned in the Inspector!");
    //     }
    // }

    /// <summary>
    /// Spawne des clés USB aléatoirement sur les tables de la scène.
    /// </summary>
    void SpawnUSBKeysOnTables()
    {
        if (usbKeyPrefab == null)
        {
            Debug.LogError("Spawner: USB Key prefab is not assigned in the Inspector!");
            return;
        }

        // Recherche toutes les tables dans la scène
        GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");

        if (tables.Length == 0)
        {
            Debug.LogWarning("Spawner: No tables found in the scene!");
            return;
        }

        Debug.Log($"Spawner: Found {tables.Length} tables in the scene.");

        foreach (GameObject table in tables)
        {
            for (int i = 0; i < usbKeysPerTable; i++)
            {
                // Calcule une position aléatoire sur ou autour de la table
                Vector3 spawnPosition = GetRandomPointOnTable(table);

                // Instancie et spawne la clé USB
                GameObject usbKeyInstance = Instantiate(usbKeyPrefab, spawnPosition, Quaternion.identity);
                NetworkServer.Spawn(usbKeyInstance);

                Debug.Log($"Spawner: Spawned USB Key at {spawnPosition} on table {table.name}.");
            }
        }
    }

    /// <summary>
    /// Retourne un point aléatoire sur une table.
    /// </summary>
    Vector3 GetRandomPointOnTable(GameObject table)
    {
        // On suppose que la table a un Collider pour définir sa zone
        Collider tableCollider = table.GetComponent<Collider>();

        if (tableCollider != null)
        {
            Bounds bounds = tableCollider.bounds;

            // Calcule une position aléatoire à l'intérieur des limites (bounds) de la table
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            // Limite la hauteur maximale à 0.7285
            float y = Mathf.Min(bounds.max.y, 0.7285f);

            return new Vector3(x, y, z);
        }
        else
        {
            Debug.LogWarning($"Spawner: Table {table.name} has no Collider! Using table position instead.");
            return table.transform.position; // Par défaut, retourne la position centrale de la table
        }
    }
}
