using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public GameObject player;
    public GameObject tile;
    public Size mapSize;

    private TileType[,] tiles;
    private RoomManager roomManager;
    private PlayerController playerController;
    private GameObject playerObj;
    private Pos playerPos;

    // Start is called before the first frame update
    void Awake()
    {
        DangeonGenerator dg = new DangeonGenerator();
        (TileType[,] tiles, List<Room> rooms) map = dg.Generate(mapSize);
        roomManager = new RoomManager(map.rooms);
        tiles = map.tiles;
        RenderTiles();
        PlayerSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
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
                    obj.name = "tile" + i + j;
                }
            }
        }
    }

    void PlayerSpawn()
    {
        playerPos = roomManager.PlayerStartPos();
        playerObj = Instantiate(player, new Vector3(1.0f * playerPos.X, 1.0f * playerPos.Y, 0.0f), Quaternion.identity);
        playerObj.name = "player";
        playerController = playerObj.GetComponent<PlayerController>();
    }

    void PlayerMove()
    {
        Move move = playerController.move;
        int x = playerPos.X + move.X;
        int y = playerPos.Y + move.Y;
        if (tiles[x, y] == TileType.Floor)
        {
            playerPos = new Pos(x, y);
            playerObj.transform.Translate(new Vector3(move.X, move.Y, 0));
        }
        playerController.move = new Move(0, 0);
    }
}
