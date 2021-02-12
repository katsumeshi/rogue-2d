using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager
{
    private List<Room> Rooms;

    public RoomManager(List<Room> rooms)
    {
        Rooms = rooms;
    }

    public Pos PlayerStartPos()
    {
        return Rooms[0].RandomPosInRect();
    }
}
