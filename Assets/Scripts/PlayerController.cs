using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;


public struct Move
{
    public int X;
    public int Y;

    public Move(int x, int y)
    {
        X = x;
        Y = y;
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

        int index = UnityEngine.Random.Range(0, moves.Count-1);
        return moves[index];
    }
}

public class PlayerController : MonoBehaviour
{

    public enum Direction
    {
        Down,
        Left,
        Up,
        Right
    }

    private Direction direction = Direction.Up;
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
            direction = Direction.Up;
            WalkSwitch();
            move = Move.Up();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Direction.Left;
            WalkSwitch();
            move = Move.Left();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Direction.Right;
            WalkSwitch();
            move = Move.Right();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Direction.Down;
            WalkSwitch();
            move = Move.Down();
        }

        pos = mapManager.Walk(TileType.Player ,pos, new Pos(pos.X + move.X, pos.Y + move.Y));
        transform.position = new Vector3(pos.X, pos.Y, 0);

    }

    public void Setup(MapManager mapManager,RoomManager roomManager)
    {
        name = "player";
        pos = roomManager.PlayerSpawntPos();
        this.mapManager = mapManager;
    }

    private void WalkSwitch()
    {
        animator.SetInteger("direction", (int) direction);
    }
}
