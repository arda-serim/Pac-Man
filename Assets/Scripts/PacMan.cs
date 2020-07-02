using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]int speed;
    Dictionary<string, RaycastHit2D> rays = new Dictionary<string, RaycastHit2D>();
    Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rays.Add("TopLeftHor",Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x/2, col.bounds.size.y/2), 
            transform.position + new Vector3(-col.bounds.size.x / 2, col.bounds.size.y / 2) + Vector3.left // left top to left
            , 0.5f, 1 << 8));
        rays.Add("TopLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, col.bounds.size.y / 2),
            transform.position + new Vector3(-col.bounds.size.x / 2, col.bounds.size.y / 2) + Vector3.up // left top to up
            , 0.5f, 1 << 8));
        rays.Add("TopRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2),
            transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2) + Vector3.right // Right top to right
            , 0.5f, 1 << 8));
        rays.Add("TopRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2),
            transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2) + Vector3.up // right top to up
            , 0.5f, 1 << 8));
        rays.Add("BottomRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2),
            transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2) + Vector3.right // right bottom to right
            , 0.5f, 1 << 8));
        rays.Add("BottomRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2),
            transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2) + Vector3.down // right bottom to down
            , 0.5f, 1 << 8));
        rays.Add("BottomLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2),
            transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2) + Vector3.left // left bottom to left
            , 0.5f, 1 << 8));
        rays.Add("BottomLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2),
            transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2) + Vector3.down // left bottom to down
            , 0.5f, 1 << 8));

    }

    void Update()
    {

        Turn();
        MoveForward();
    }

    void MoveForward()
    {
        rb.MovePosition(transform.position + transform.right * Time.deltaTime * speed);
    }

    void Turn()
    {
        if (Input.GetKeyDown(KeyCode.A) && !rays["TopLeftHor"] && !rays["BottomLeftHor"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.W) && !rays["TopLeftVer"] && !rays["TopRightVer"] )
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !rays["TopRightHor"] && !rays["BottomRightHor"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S) && !rays["BottomLeftVer"] && !rays["BottomRightVer"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }

    }
}
