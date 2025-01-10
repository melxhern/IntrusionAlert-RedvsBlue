using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetworkRelay : NetworkBehaviour
{

    [Command]
    public void CmdActivateAntivirus(uint computerNetId)
    {
        if (computerNetId == 0)
        {
            Debug.LogError("Invalid computerNetId");
            return;
        }

        Debug.Log($"CmdActivateAntivirus: {computerNetId}");
        AntivirusManager antivirusManager = FindObjectOfType<AntivirusManager>();
        if (antivirusManager == null)
        {
            Debug.LogError("AntivirusManager not found!");
            return;
        }

        antivirusManager.ServerActivateAntivirus(computerNetId);
    }



}
