
using UnityEngine;
using TMPro;

public class AntivirusTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Référence au texte UI pour afficher le timer
    private float endOfProtectionTime;
    private bool isTimerActive = false;

    /// <summary>
    /// Starts the antivirus timer.
    /// </summary>
    /// <param name="networkTime">The current network time.</param>
    /// <param name="protectionEndTime">The time when the protection ends.</param>
    public void StartAntivirusTimer(float networkTime, float protectionEndTime)
    {
        endOfProtectionTime = protectionEndTime;
        isTimerActive = true;
        UpdateTimerUI(networkTime); // Met à jour immédiatement l'UI
    }

    /// <summary>
    /// Update is called once per frame.
    /// Updates the timer if it is active.
    /// </summary>
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
            }
        }
    }

    /// <summary>
    /// Updates the timer UI.
    /// </summary>
    /// <param name="timeRemaining">The remaining time to display.</param>
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
