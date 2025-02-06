using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MissionObjectManager : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update.
    /// Checks if the server is active and registers mission objects if it is.
    /// </summary>
    void Start()
    {
        if (NetworkServer.active)
        {
            RegisterMissionObjects();
        }
        else
        {
            Debug.LogWarning("MissionObjectManager: This script should only run on the server.");
        }
    }

    /// <summary>
    /// Registers mission objects on the server.
    /// </summary>
    [Server]
    void RegisterMissionObjects()
    {
        Debug.Log("Registering MissionObjects...");
        GameObject[] missionObjects = GameObject.FindGameObjectsWithTag("MissionObject");
        Debug.Log($"Found {missionObjects.Length} MissionObjects.");

        NetworkServer.SpawnObjects();
        foreach (var kvp in NetworkServer.spawned)
        {
            Debug.Log($"Spawned Object: {kvp.Value.name}, NetID: {kvp.Key}");
        }

        Debug.Log("MissionObjects registered.");
    }
}

