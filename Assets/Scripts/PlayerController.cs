using System.Collections;
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
    public Move move = new Move(0, 0);
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Direction.Up;
            WalkSwitch();
            move = new Move(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Direction.Left;
            WalkSwitch();
            move = new Move(-1, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Direction.Right;
            WalkSwitch();
            move = new Move(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Direction.Down;
            WalkSwitch();
            move = new Move(0, -1);
        }

    }

    private void WalkSwitch()
    {
        animator.SetInteger("direction", (int) direction);
    }

}
