using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        player = GameObject.Find("player");
    }
    void LateUpdate()
    {
        MoveCamera();
    }
    void MoveCamera()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}