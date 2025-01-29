using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AntivirusTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Référence au texte UI pour afficher le timer
    private float endOfProtectionTime;
    private bool isTimerActive = false;

    public void StartAntivirusTimer(float networkTime, float protectionEndTime)
    {
        endOfProtectionTime = protectionEndTime;
        isTimerActive = true;
        UpdateTimerUI(networkTime); // Met à jour immédiatement l'UI
    }

    private void Update()
    {
        if (isTimerActive)
        {
            float timeRemaining = endOfProtectionTime - (float)Mirror.NetworkTime.time;

            if (timeRemaining > 0)
            {
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerActive = false;
                UpdateTimerUI(timeRemaining);
                Debug.Log("Antivirus protection time has expired!");
            }
        }
    }

    private void UpdateTimerUI(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemaining <= 10)
        {
            timerText.color = Color.red; // Change la couleur à rouge pour les 10 dernières secondes
        }
        else
        {
            timerText.color = Color.black;
        }
    }
}
