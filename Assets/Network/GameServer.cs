using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameServer : MonoBehaviour {

    public bool isServer;
    public bool startGame = false;

    public Transform startGameButtonPrefab;

    public Transform[] playerPrefabs;

    Dictionary<NetworkPlayer, int> playerIds;
    List<int> ids;

	// Use this for initialization
	IEnumerator Start () {
        if (isServer)
        {
            Network.InitializeServer(4, 12340);
            var startButton = Instantiate(startGameButtonPrefab);

            playerIds = new Dictionary<NetworkPlayer, int>();
            ids = new List<int>();
            ids.Add(0);
            ids.Add(1);
            ids.Add(2);
            ids.Add(3);

            int i = ids[Random.Range(0, ids.Count)];
            ids.Remove(i);
            playerIds.Add(Network.player, i);
            JoinedGame(i);

            while (!startGame)
            {
                yield return false;
            }

        }
	}

    void OnPlayerConnected(NetworkPlayer pid)
    {
        int i = ids[Random.Range(0, ids.Count)];
        ids.Remove(i);
        playerIds.Add(pid, i);

        networkView.RPC("JoinedGame", pid, i);
    }

    void OnPlayerDisconnected(NetworkPlayer pid)
    {
        ids.Add(playerIds[pid]);
        playerIds.Remove(pid);
    }

    [RPC]
    void StartTheGame()
    {

    }

    [RPC]
    void JoinedGame(int id)
    {
        Network.Instantiate(playerPrefabs[id], playerPrefabs[id].position, Quaternion.identity, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
