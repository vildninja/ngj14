using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameServer : MonoBehaviour {

    public bool isServer;
    public bool startGame = false;

    public Transform startGameButtonPrefab;

    public Transform[] playerPrefabs;

    public Transform player;

    public Transform[] planetPrefabs;

    public int planetCounts = 5;
    public Vector2 extends;

    Dictionary<NetworkPlayer, int> playerIds;
    List<int> ids;

	// Use this for initialization
	IEnumerator Start () {
        if (isServer)
        {
            Network.InitializeServer(4, 12340, false);
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

            for (int n = 0; n < planetCounts; n++)
            {
                Vector3 pos;
                float nearest;
                int safety = 50;
                do
                {
                    pos = transform.position + new Vector3(extends.x * Random.Range(-1f, 1f), extends.y * Random.Range(-1f, 1f));
                    nearest = float.MaxValue;
                    foreach (var t in FindObjectsOfType<Transform>())
                        if (Vector3.Distance(pos, t.position) < nearest)
                            nearest = Vector3.Distance(pos, t.position);

                    if (safety-- < 0)
                        break;
                }
                while (nearest > 2);

                Network.Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], pos, Quaternion.identity, 0);
            }

            networkView.RPC("StartTheGame", RPCMode.All);
            StartTheGame();
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
        // give control to the player;
    }

    [RPC]
    void JoinedGame(int id)
    {
        player = Network.Instantiate(playerPrefabs[id], playerPrefabs[id].position, Quaternion.identity, 0) as Transform;
    }
}
