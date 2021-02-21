using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapManager : MonoBehaviour
{

    public GameObject player;
    public GameObject tile;
    public GameObject enemy;
    public GameObject exit;
    public Text debugText;
    public Size mapSize;

    private TileType[,] tiles;
    private RoomManager roomManager;
    private bool playerTurn;
    private PlayerController playerController;

    void Awake()
    {
        MapInit();
    }

    void MapInit()
    {
        playerTurn = false;
        DestroyObjects();
        DangeonGenerator dg = new DangeonGenerator();
        (TileType[,] tiles, List<Room> rooms) map = dg.Generate(mapSize);
        roomManager = new RoomManager(map.rooms);
        tiles = map.tiles;

        RenderTiles();
        PlayerSpawn();
        EnemiesSpawn();
        ExitSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject obj in objects)
            {

                Move move = Move.Random();
                Vector3 pos = obj.transform.position;
                Pos cur = new Pos((int)pos.x, (int)pos.y);
                Pos next = new Pos((int)pos.x + move.X, (int)pos.y + move.Y);
                Pos n = Walk(TileType.Enemy, cur, next);
                obj.transform.position = new Vector3(n.X, n.Y, 0);
            }
            playerTurn = true;
        }
    }

    void DebugText()
    {
        string debug = "";
        for (int j = 0; j < mapSize.Height; j++)
        {
            string row = "";
            for (int i = 0; i < mapSize.Width; i++)
            {
                row += (int)tiles[i, mapSize.Height - j - 1] + ",";
            }
            debug += row + "\n";
        }
        //debugText.text = debug;
    }

    void RenderTiles()
    {
        for (int j = 0; j < mapSize.Height; j++)
        {
            for (int i = 0; i < mapSize.Width; i++)
            {
                if (tiles[i, j] == TileType.Wall)
                {
                    GameObject obj = Instantiate(tile, new Vector3(1.0f * i, 1.0f * j, 0.0f), Quaternion.identity);
                    obj.name = "tile" + i + "_" + j;
                }
            }
        }
    }

    void DestroyObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    void PlayerSpawn()
    {
        playerController = player.GetComponent<PlayerController>();
        Pos pos = playerController.Spawn(this, roomManager);
        player.transform.position = new Vector3(pos.X, pos.Y, 0);
        tiles[pos.X, pos.Y] = TileType.Player;
    }

    void EnemiesSpawn()
    {

        int len = Random.Range(0, 3);
        for (int i = 0; i < len; i++)
        {
            GameObject ene = Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
            EnemyController enemyController = ene.GetComponent<EnemyController>();
            Pos pos = enemyController.Spawn(this, roomManager);
            tiles[pos.X, pos.Y] = TileType.Enemy;
        }
    }

    void ExitSpawn()
    {
        Pos pos = roomManager.ExitSpawnPos();
        tiles[pos.X, pos.Y] = TileType.Exit;
        GameObject exi = Instantiate(exit, new Vector3(1.0f * pos.X, 1.0f * pos.Y, 0.0f), Quaternion.identity);
        exi.name = "exit" + pos.X + "_" + pos.Y;
    }


    public Pos Walk(TileType tileType, Pos cur, Pos next)
    {
        if (tiles[next.X, next.Y] == TileType.Floor)
        {
            tiles[next.X, next.Y] = tileType;
            tiles[cur.X, cur.Y] = TileType.Floor;
            return next;
        }
        return cur;
    }


    public bool CanWalk(Pos cur, Pos next)
    {
        if (tiles[next.X, next.Y] == TileType.Exit)
        {
            tiles[next.X, next.Y] = TileType.Player;
            MapInit();
        }
        else if (tiles[next.X, next.Y] == TileType.Floor)
        {
            TileType tmp = tiles[next.X, next.Y];
            tiles[next.X, next.Y] = TileType.Player;
            tiles[cur.X, cur.Y] = tmp;
            return true;
        }
        return false;
    }

    public void PlayerTurn()
    {
        playerTurn = true;
    }

    public void EnemyTurn()
    {
        playerTurn = false;
    }
}
