using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
    }

    private void Update() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
