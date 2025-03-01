
using UnityEngine;
using UnityEngine.UI;

public class OnlineUI : MonoBehaviour
{
    [SerializeField]
    private InputField nicknameInputField;
    // [SerializeField]
    // private GameObject createRoomUI;
    private CreateGameRoomData roomData;

    
    private void Start()
    {
        // Initialize room data with default values
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 4 };
    }

    public class CreateGameRoomData
    {
        public int imposterCount;
        public int maxPlayerCount;
    }
}
