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

        /// <summary>
        /// Use this for initialization.
        /// Registers the StartGameMessage handler and waits for the server to be active.
        /// </summary>
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
        }

        /// <summary>
        /// Handler for the StartGameMessage.
        /// Invokes the OnGameStarted action.
        /// </summary>
        private void OnStartGameReceived(StartGameMessage obj)
        {
            OnGameStarted?.Invoke();
        }

        /// <summary>
        /// Adds a player to the list of players.
        /// </summary>
        /// <param name="player">The player to add.</param>
        public void AddPlayer(ThirdPersonController player)
        {
            _players.Add(player);
        }


    }


}