using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents movement of characters.
/// </summary>
public class CharacterMovement : MonoBehaviour {
    public Rigidbody2D rb;
    public float speed = 1;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f) * speed;
    }
}
