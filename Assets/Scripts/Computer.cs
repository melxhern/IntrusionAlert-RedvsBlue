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
            .FirstOrDefault(obj => obj.name == "computerUI"); // permet de trouver l'objet m�me si d�sactiv�
        if (computerUI == null)
        {
            Debug.LogError("L'UI AntivirusMissionUI est introuvable !");
        }

        var computerUIManager = computerUI.GetComponent<ComputerUIManager>();

        if (currentStatus == -2)
        {
            Debug.Log("Hacked UI" + currentStatus);
            computerUIManager.HackedUI();
            return;
        }


        if (role == PlayerRole.BlueTeam)
        {
            Debug.Log("end of protection time: " + endOfProtectionTime);
            Debug.Log("network time: " + NetworkTime.time);
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
            Debug.Log("role : " + role);
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

        var gameResultManager = FindObjectOfType<GameResultManager>();
        if (gameResultManager != null)
        {
            Debug.Log("Checking if all computers are hacked");
            gameResultManager.CheckIfAllComputersHacked();
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
