using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using StarterAssets;

public class Computer : NetworkBehaviour
{
    [SyncVar]
    private double endOfProtectionTime = 0;

    [SyncVar]
    public short currentStatus = 0; // 0 = idle, 1 = being protected, 2 = protected, -1 = being pirated, -2 = pirated

    [SyncVar]
    public List<string> events = new List<string>();

    /// <summary>
    /// Opens the UI for the computer based on the player's role and the computer's status.
    /// </summary>
    /// <param name="player">The player interacting with the computer.</param>
    public void OpenUI(GameObject player)
    {
        Debug.Log("Current status: " + currentStatus);
        if (currentStatus == 2 && endOfProtectionTime <= NetworkTime.time)
        {
            CmdStopProtection();
        }

        var thirdPersonController = player.GetComponent<ThirdPersonController>();
        var role = thirdPersonController.GetRole();
        var computerUI = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(obj => obj.name == "computerUI"); // permet de trouver l'objet meme si désactivé
        if (computerUI == null)
        {
            Debug.LogError("L'UI AntivirusMissionUI est introuvable !");
        }

        var computerUIManager = computerUI.GetComponent<ComputerUIManager>();

        if (currentStatus == -2)
        {
            computerUIManager.HackedUI();
            return;
        }


        if (role == PlayerRole.BlueTeam)
        {
            if (endOfProtectionTime > NetworkTime.time)
            {
                computerUIManager.ProtectedUI(gameObject, endOfProtectionTime);
            }
            else
            {
                computerUIManager.StartUI();
            }
        }
        else if (role == PlayerRole.RedTeam)
        {
            if (thirdPersonController.IsHoldingKey)
            {
                computerUIManager.HackStartUI(endOfProtectionTime > NetworkTime.time);
                CmdPirateComputer();
            }
            else
            {
                computerUIManager.NoInteractionUI();
            }
        }

    }

    /// <summary>
    /// Command to set the computer's status to being protected.
    /// </summary>
    [Command(requiresAuthority = false)]
    public void CmdProtectComputer()
    {
        currentStatus = 1;
    }

    /// <summary>
    /// Command to set the computer's status to protected.
    /// </summary>
    [Command(requiresAuthority = false)]
    public void CmdComputerProtected()
    {
        if (currentStatus != -2)
        {
            endOfProtectionTime = NetworkTime.time + 60;
            currentStatus = 2;
        }

    }

    /// <summary>
    /// Command to set the computer's status to being pirated.
    /// </summary>
    [Command(requiresAuthority = false)]
    public void CmdPirateComputer()
    {
        if (currentStatus != 2)
        {
            currentStatus = -1;
        }

        events.Add("Suspicious USB blocked at " + System.TimeSpan.FromSeconds(NetworkTime.time).ToString(@"hh\:mm\:ss"));
    }

    /// <summary>
    /// Command to set the computer's status to pirated.
    /// </summary>
    [Command(requiresAuthority = false)]
    public void CmdComputerPirated()
    {
        if (currentStatus != 2)
        {
            currentStatus = -2;
        }

        var gameResultManager = FindObjectOfType<GameResultManager>();
        if (gameResultManager != null)
        {
            gameResultManager.CheckIfAllComputersHacked();
        }

    }

    /// <summary>
    /// Command to stop the protection of the computer.
    /// </summary>
    [Command(requiresAuthority = false)]
    public void CmdStopProtection()
    {
        currentStatus = 0;
    }

    /// <summary>
    /// Command to add an event to the computer's event list.
    /// </summary>
    /// <param name="eventText">The event text to add.</param>
    [Command(requiresAuthority = false)]
    public void CmdAddEvent(string eventText)
    {
        events.Add(eventText);
    }
}
