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
    Dictionary<int, int> score;
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

            SpawnPlanets();

        }

        while (!Network.isServer && !Network.isClient)
        {
            MasterServer.RequestHostList("ngj20014-spacelove");
            yield return new WaitForSeconds(5);
        }

        if (joinButtons != null)
            foreach (var t in joinButtons)
                if (t && t.gameObject)
                    Destroy(t.gameObject);

        // follow progress
        while (Network.isServer)
        {
            score = new Dictionary<int, int>();
            yield return new WaitForSeconds(.5f);
            if ((playerIds.Count > 1 && FindObjectsOfType<SpacePlayer>().Length <= 1) || FindObjectsOfType<SpacePlayer>().Length == 0)
            {
                // game over

                foreach (var p in FindObjectsOfType<PlanetController>())
                    Network.Destroy(p.networkView.viewID);

                foreach (var r in FindObjectsOfType<Rocket>())
                    Network.Destroy(r.networkView.viewID);

                var winner = FindObjectOfType<SpacePlayer>();
                Transform victoryAnim = null;
                if (winner)
                {
                    if (winner.victoryAnimation)
                    {
                        victoryAnim = Network.Instantiate(winner.victoryAnimation, Vector3.zero, Quaternion.identity, 0) as Transform;
                    }
                }
                else
                {
                    // draw
                }

                ResetGame();
                networkView.RPC("ResetGame", RPCMode.Others);
                yield return new WaitForSeconds(1);

                if (winner)
                    Network.Destroy(winner.networkView.viewID);

                yield return new WaitForSeconds(1);

                var startButton = Instantiate(startGameButtonPrefab);

                foreach (var p in playerIds)
                {
                    if (p.Key != Network.player)
                        networkView.RPC("JoinedGame", p.Key, p.Value);
                    else
                        JoinedGame(p.Value);
                }

                while (!startGame)
                {
                    yield return false;
                }

                if (victoryAnim)
                    Network.Destroy(victoryAnim.networkView.viewID);

                SpawnPlanets();
            }
        }
	}

    void SpawnPlanets()
    {

        foreach (var p in planetPrefabs)
        {
            Network.Instantiate(p, p.transform.position, p.transform.rotation, 0);
        }

        StartTheGame();
        networkView.RPC("StartTheGame", RPCMode.All);
    }

    [RPC]
    void ResetGame()
    {
        startGame = false;

        foreach (var s in FindObjectsOfType<RelationSpiral>())
            Destroy(s.gameObject);
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
        foreach (var p in FindObjectsOfType<SpacePlayer>())
            p.canMove = true;
    }

    [RPC]
    void JoinedGame(int id)
    {
        if (joinButtons != null)
            foreach (var t in joinButtons)
                if (t && t.gameObject)
                    Destroy(t.gameObject);
        player = Network.Instantiate(playerPrefabs[id], playerPrefabs[id].transform.position, Quaternion.identity, 0) as SpacePlayer;
    }
}
