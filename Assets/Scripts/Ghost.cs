using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour
{
    private int speed;
    private Rigidbody2D rb;
    private Vector2
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        

    }

    void MoveForward()
    {
        rb.MovePosition(transform.position + transform.right * Time.deltaTime * speed);
    }
}
