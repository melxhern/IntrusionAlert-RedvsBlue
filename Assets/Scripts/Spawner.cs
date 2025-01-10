using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
    public GameObject gameManagerPrefab; // Assigné dans l'Inspector

    public override void OnStartServer()
    {
        base.OnStartServer();

        if (gameManagerPrefab != null)
        {
            GameObject gameManagerInstance = Instantiate(gameManagerPrefab);

            // Spawner sur le réseau
            NetworkServer.Spawn(gameManagerInstance);

            Debug.Log("GameManager spawned on the server!");
        }
        else
        {
            Debug.LogError("GameManager prefab is not assigned!");
        }
    }
}
