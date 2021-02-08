using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    GameObject player;
    //PlayerController player;
    //Transform playerTransform;
    void Start()
    {
        player = GameObject.Find("Player");
        print(player);
        //player = playerObj.GetComponent<PlayerController>();
        //playerTransform = playerObj.transform;
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