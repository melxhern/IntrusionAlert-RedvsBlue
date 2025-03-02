
using UnityEngine;
using Mirror;

public class PlayerNetworkRelay : NetworkBehaviour
{
    /// <summary>
    /// Command to activate the antivirus on a specified computer.
    /// </summary>
    /// <param name="computerNetId">The network ID of the computer to activate the antivirus on.</param>
    [Command]
    public void CmdActivateAntivirus(uint computerNetId)
    {
        if (computerNetId == 0)
        {
            Debug.LogError("Invalid computerNetId");
            return;
        }

        AntivirusManager antivirusManager = FindObjectOfType<AntivirusManager>();
        if (antivirusManager == null)
        {
            Debug.LogError("AntivirusManager not found!");
            return;
        }
        antivirusManager.ServerActivateAntivirus(computerNetId);
    }
}
