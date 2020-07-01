using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]int speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveForward();
    }

    void MoveForward()
    {
        Debug.Log(transform.position + transform.right);
        rb.MovePosition(transform.position + transform.right * Time.deltaTime * speed);
    }

    void Turn()
    {

    }
}
