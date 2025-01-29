using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 480;
    [SerializeField] private TextMeshProUGUI timerText;

    private bool isGameStarted = false;

    private void Start()
    {
        // Abonne-toi à l'événement OnGameStarted du GameManager
        Assets.Scripts.GameManager.OnGameStarted += StartTimer;
    }

    private void OnDestroy()
    {
        // Désabonne-toi de l'événement pour éviter les erreurs
        Assets.Scripts.GameManager.OnGameStarted -= StartTimer;
    }

    private void Update()
    {
        if (isGameStarted && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void StartTimer()
    {
        isGameStarted = true; // Démarre le timer lorsque la partie commence
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timeRemaining <= 10)
        {
            timerText.color = Color.red;

        }
        if (timeRemaining <= 10)
        {
            timerText.color = Color.red;
        }

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            Debug.Log("Time has run out!");

            // Appelle le GameResultManager pour vérifier le résultat
            var gameResultManager = FindObjectOfType<GameResultManager>();
            if (gameResultManager != null)
            {
                gameResultManager.CheckGameResult();
            }
            else
            {
                Debug.LogError("GameResultManager introuvable dans la scène !");
            }

            isGameStarted = false; // Arrête le timer
        }


    }
}
