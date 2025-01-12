using System.Collections;
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


    public void OpenUI(GameObject player)
    {
        Debug.Log("Current status: " + currentStatus);
        if (currentStatus == 2 && endOfProtectionTime <= NetworkTime.time)
        {
            CmdStopProtection();
        }
        //else if (currentStatus == 1 || currentStatus == -1)
        //{
        //    CmdStopProtection();
        //}

        

        var thirdPersonController = player.GetComponent<ThirdPersonController>();
        var role = thirdPersonController.GetRole();
        var computerUI = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(obj => obj.name == "computerUI"); // permet de trouver l'objet même si désactivé
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
                computerUIManager.ProtectedUI(gameObject);
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
                Debug.Log("No interaction UI");
                computerUIManager.NoInteractionUI();
            } 
        }




    }

    [Command(requiresAuthority = false)]
    public void CmdProtectComputer()
    {
        currentStatus = 1;
    }

    [Command(requiresAuthority = false)]
    public void CmdComputerProtected()
    {
        if (currentStatus != -2)
        {
            endOfProtectionTime = NetworkTime.time + 60;
            currentStatus = 2;
            Debug.Log("Computer protected by server !!!!!");
        }

    }

    [Command(requiresAuthority = false)]
    public void CmdPirateComputer()
    {
        if (currentStatus != 2)
        {
            currentStatus = -1;
        }

        events.Add("Suspicious USB blocked at " + System.TimeSpan.FromSeconds(NetworkTime.time).ToString(@"hh\:mm\:ss"));
    }

    [Command(requiresAuthority = false)]
    public void CmdComputerPirated()
    {
        Debug.Log("Value of current Status" + currentStatus);
        if (currentStatus != 2)
        {
            currentStatus = -2;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdStopProtection()
    {
        currentStatus = 0;
    }

    [Command(requiresAuthority = false)]
    public void CmdAddEvent(string eventText)
    {
        events.Add(eventText);
    }
}
