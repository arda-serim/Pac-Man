using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]int speed;
    Dictionary<string, RaycastHit2D> rays = new Dictionary<string, RaycastHit2D>();
    Collider2D col;
    float errorMargin = 0.1f;
    float raycastDistance = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Debug.Log(col.bounds.size.y);
    }

    void Update()
    {
        SendRays();
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

    void SendRays()
    {
        rays.Clear();

        rays.Add("TopLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, col.bounds.size.y / 2 - errorMargin),
            Vector3.left , raycastDistance, 1 << 8));
        rays.Add("TopLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2 + errorMargin, col.bounds.size.y / 2),
            Vector3.up  , raycastDistance, 1 << 8));
        rays.Add("TopRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2 - errorMargin),
            Vector3.right, raycastDistance, 1 << 8));
        rays.Add("TopRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2 - errorMargin, col.bounds.size.y / 2),
            Vector3.up, raycastDistance, 1 << 8));
        rays.Add("BottomRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2 + errorMargin),
            Vector3.right, raycastDistance, 1 << 8));
        rays.Add("BottomRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2 - errorMargin, -col.bounds.size.y / 2),
            Vector3.down, raycastDistance, 1 << 8));
        rays.Add("BottomLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2 + errorMargin),
            Vector3.left , raycastDistance, 1 << 8));
        rays.Add("BottomLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2 + errorMargin, -col.bounds.size.y / 2),
            Vector3.down , raycastDistance, 1 << 8));
    }
}
