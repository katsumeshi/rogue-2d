using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Down,
    Left,
    Up,
    Right
}

public struct Move
{

    public int X;
    public int Y;
    public Direction Direction;

    public Move(int x, int y)
    {
        X = x;
        Y = y;
        Direction = Direction.Up;
        Direction = GetDirection();
    }

    public Direction GetDirection()
    {

        if (X == 0 && Y == 1)
        {
            return Direction.Up;
        }
        else if (X == 0 && Y == -1)
        {
            return Direction.Down;
        }
        else if (X == 1 && Y == 0)
        {
            return Direction.Right;
        }
        else if (X == -1 && Y == 0)
        {
            return Direction.Left;
        }
        return Direction.Up;
    }

    public static Move Up()
    {
        return new Move(0, 1);
    }

    public static Move Down()
    {
        return new Move(0, -1);
    }

    public static Move Left()
    {
        return new Move(-1, 0);
    }

    public static Move Right()
    {
        return new Move(1, 0);
    }

    public static Move Random()
    {
        List<Move> moves = new List<Move>();
        moves.Add(Up());
        moves.Add(Down());
        moves.Add(Left());
        moves.Add(Right());

        int index = UnityEngine.Random.Range(0, moves.Count);
        return moves[index];
    }

    public bool WillMove()
    {
        return X != 0 || Y != 0;
    }
}

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private Pos pos;
    private MapManager mapManager;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move move = new Move(0, 0);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move = Move.Up();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move = Move.Left();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move = Move.Right();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move = Move.Down();
        }

        if (move.WillMove())
        {

            print("12444443333");
            print(pos.ToString());
            animator.SetInteger("direction", (int) move.Direction);
            Pos next = new Pos(pos.X + move.X, pos.Y + move.Y);
            if (mapManager.CanWalk(pos, next))
            {
                pos = next;
                transform.position = new Vector3(pos.X, pos.Y, 0);
            }
            mapManager.EnemyTurn();
        }
    }

    public Pos Spawn(MapManager mapManager, RoomManager roomManager)
    {
        this.mapManager = mapManager;
        pos = roomManager.PlayerSpawntPos();
        transform.position = new Vector3(pos.X, pos.Y, 0);
        print("spawwwnnnn");
        print(pos.ToString());
        return pos;
    }

    public List<TileType> AllowTile()
    {
        return new List<TileType>() { TileType.Floor, TileType.Exit };
    }
}
