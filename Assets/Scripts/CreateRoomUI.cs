using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CreateRoomUI : MonoBehaviour
{
    private CreateGameRoomData roomData;

    // Start is called before the first frame update
    void Start()
    {
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 4 };
    }


    public void CreateRoom()
    {
        // var manager = NetworkManager.singleton as AmongUsRoomManager;

        // manager.minPlayerCount = 3; 
        // manager.imposterCount = 1; 
        // manager.maxConnections = 4; 
        // manager.StartHost();
    }

    public class CreateGameRoomData
    {
        public int imposterCount;
        public int maxPlayerCount;
    }
}
