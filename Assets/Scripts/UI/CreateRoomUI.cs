
using UnityEngine;


public class CreateRoomUI : MonoBehaviour
{
    private CreateGameRoomData roomData;

    // Start is called before the first frame update
    void Start()
    {
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 4 };
    }

    public class CreateGameRoomData
    {
        public int imposterCount;
        public int maxPlayerCount;
    }
}
