using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameServer : MonoBehaviour {

    public bool startGame = false;

    public Transform startGameButtonPrefab;

    public SpacePlayer[] playerPrefabs;

    public SpacePlayer player;

    public Transform[] planetPrefabs;

    public exSpriteFont joinServerButtonPrefab;
    private List<exSpriteFont> joinButtons = null;

    public int planetCounts = 5;
    public Vector2 extends;

    Dictionary<NetworkPlayer, int> playerIds;
    List<int> ids;

	// Use this for initialization
	IEnumerator Start () {
        if (GameController.Instance.isServer)
        {
            Network.InitializeServer(4, 12340, false);
            MasterServer.RegisterHost("ngj20014-spacelove", "game" + Random.Range(100, 1000));
            yield return false;
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
            MasterServer.UnregisterHost();

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
                    {
                        print("Safety");
                        break;
                    }
                }
                while (nearest < 3);

                Network.Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], pos, Quaternion.identity, 0);
            }

            networkView.RPC("StartTheGame", RPCMode.All);
            StartTheGame();
        }

        while (!Network.isServer && !Network.isClient)
        {
            MasterServer.RequestHostList("ngj20014-spacelove");
            yield return new WaitForSeconds(5);
        }

        if (joinButtons != null)
            foreach (var t in joinButtons)
                Destroy(t.gameObject);
	}

    void OnMasterServerEvent(MasterServerEvent mse)
    {
        if (mse == MasterServerEvent.HostListReceived && !Network.isServer && !Network.isClient)
        {
            if (joinButtons != null)
                foreach (var t in joinButtons)
                    Destroy(t.gameObject);
            joinButtons = new List<exSpriteFont>();
            var list = MasterServer.PollHostList();
            for (int i = 0; i < list.Length; i++)
            {
                var button = Instantiate(joinServerButtonPrefab, new Vector3(0, extends.y - i * 1.5f), Quaternion.identity) as exSpriteFont;
                button.text = list[i].gameName;
                button.GetComponent<JoinServerButton>().data = list[i];

                joinButtons.Add(button);
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
        // give control to the player;
    }

    [RPC]
    void JoinedGame(int id)
    {
        if (joinButtons != null)
            foreach (var t in joinButtons)
                Destroy(t.gameObject);
        player = Network.Instantiate(playerPrefabs[id], playerPrefabs[id].transform.position, Quaternion.identity, 0) as SpacePlayer;
    }
}
