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
    private bool playerTurn = true; 

    void Awake()
    {
        MapInit();
    }

    void MapInit()
    {
        DestroyObjects();
        DangeonGenerator dg = new DangeonGenerator();
        (TileType[,] tiles, List<Room> rooms) map = dg.Generate(mapSize);
        roomManager = new RoomManager(map.rooms);
        tiles = map.tiles;

        GameObject player2 = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        PlayerController playerController = player2.GetComponent<PlayerController>();
        playerController.Setup(this, roomManager);
        RenderTiles();
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
                row += (int) tiles[i, mapSize.Height-j-1] + ",";
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
        for (int j = 0; j < mapSize.Height; j++)
        {
            for (int i = 0; i < mapSize.Width; i++)
            {
                GameObject obj = GameObject.Find("tile" + i + "_" + j);
                if (obj) Destroy(obj);
            }
        }
        GameObject player = GameObject.Find("player");
        if (player) Destroy(player);
    }

    void EnemiesSpawn()
    {
        List<Pos> posList = roomManager.EnemySpawnPos();
        posList.ForEach(pos =>
        {
            tiles[pos.X, pos.Y] = TileType.Enemy;
            GameObject ene = Instantiate(enemy, new Vector3(1.0f * pos.X, 1.0f * pos.Y, 0.0f), Quaternion.identity);
            ene.name = "enemy" + pos.X + "_" + pos.Y;
        });
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
        if (tiles[next.X, next.Y] == TileType.Floor || tiles[next.X, next.Y] == TileType.Exit)
        {
            tiles[next.X, next.Y] = tileType;
            tiles[cur.X, cur.Y] = TileType.Floor;
            playerTurn = false;
            return next;
        }
        return cur;
    }
}
