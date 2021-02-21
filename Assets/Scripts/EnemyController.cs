using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Pos Spawn(MapManager mapManager, RoomManager roomManager)
    {
        Pos pos = roomManager.EnemySpawnPos();
        name = "enemy" + pos.X + "_" + pos.Y;
        transform.position = new Vector3(1.0f * pos.X, 1.0f * pos.Y, 0.0f);
        return pos;
    }
}
