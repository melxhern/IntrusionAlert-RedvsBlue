using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button StartGameButton;

    // Start is called before the first frame update
    void Start()
    {
        if (StartGameButton == null)
        {
            StartGameButton = GameObject.Find("StartGameButton")?.GetComponent<Button>();
            if (StartGameButton == null)
            {
                Debug.LogError("StartGameButton introuvable dans la scène !");
                return;
            }
        }

        if (NetworkServer.active)
        {
            Debug.Log("Le serveur est actif.");
            StartGameButton.gameObject.SetActive(true);
            StartGameButton.onClick.AddListener(StartGameClicked);
        }
        else
        {
            Debug.Log("Le serveur n'est pas actif.");
            StartGameButton.gameObject.SetActive(false);
        }
    }

    private void StartGameClicked()
    {
        NetworkManager.singleton.ServerChangeScene("Game");
    }





}