using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    CircleCollider2D col;
    Rigidbody2D rb;

    Vector2 movement;

    void Awake() {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + movement.normalized * speed * Time.deltaTime);
    }
}
