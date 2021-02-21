using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager
{
    private List<Room> Rooms;

    public RoomManager(List<Room> rooms)
    {
        Rooms = rooms;

        rooms.ForEach(room =>
        {
            Debug.Log(room.Rect.ToString());
        });
    }

    public Pos PlayerSpawntPos()
    {
        return SpawnPos(0);
    }

    //public List<Pos> EnemySpawnPos()
    //{
    //    List<Pos> pos = new List<Pos>();
    //    int enemyCount = Random.Range(1, Rooms.Count);
    //    for (int i = 0; i < enemyCount; i++)
    //    {
    //        pos.Add(SpawnPos(Random.Range(0, Rooms.Count - 1)));
    //    }
    //    return pos;
    //}

    public Pos EnemySpawnPos()
    {
        //List<Pos> pos = new List<Pos>();
        //int enemyCount = Random.Range(1, Rooms.Count);
        //for (int i = 0; i < pos.Count; i++)
        //{
        //    pos.Add(SpawnPos(Random.Range(0, Rooms.Count - 1)));
        //}
        return SpawnPos(Random.Range(0, Rooms.Count - 1));
    }

    public Pos ExitSpawnPos()
    {
        return SpawnPos(0);
        return SpawnPos(Rooms.Count - 1);
    }

    private Pos SpawnPos(int roomIndex)
    {
        if (0 <= roomIndex && roomIndex < Rooms.Count)
        {
            return Rooms[roomIndex].RandomPosInRect();
        }
        Debug.Assert(false, "SpawnPos: out of range");
        return new Pos(0, 0);
    }
}
