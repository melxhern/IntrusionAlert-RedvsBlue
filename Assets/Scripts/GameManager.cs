using Mirror;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StartGameMessage : NetworkMessage
{

}

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private void Awake()
        {
            Instance = this;
        }

        public static Action OnGameStarted;

        private List<ThirdPersonController> _players = new List<ThirdPersonController>();

        // Use this for initialization
        IEnumerator Start()
        {
            NetworkClient.RegisterHandler<StartGameMessage>(OnStartGameReceived);
            while (!NetworkServer.active)
            {
                yield return null;
            }
            while (_players.Count != NetworkServer.connections.Count || !_players.TrueForAll(x => x.connectionToClient.isReady))
            {
                yield return null;
            }

            int rand = UnityEngine.Random.Range(0, _players.Count);
            _players[rand].Role = PlayerRole.RedTeam;

            NetworkServer.SendToReady(new StartGameMessage());
            //if (NetworkClient.active)
            //{
            //    OnGameStarted?.Invoke();
            //}
        }

        private void OnStartGameReceived(StartGameMessage obj)
        {
            OnGameStarted?.Invoke();
        }

        public void AddPlayer(ThirdPersonController player)
        {
            _players.Add(player);
        }
    }
}