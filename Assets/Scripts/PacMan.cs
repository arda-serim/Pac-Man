using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]int speed;
    Dictionary<string, RaycastHit2D> rays = new Dictionary<string, RaycastHit2D>();
    Collider2D col;
    float errorMargin = 0;
    float raycastDistance = 0.13f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        Turn();
        MoveForward();
        SendRays();
    }

    void MoveForward()
    {
        rb.MovePosition(transform.position + transform.right * Time.deltaTime * speed);
    }

    void Turn()
    {
        if (Input.GetKeyDown(KeyCode.A) && !rays["TopLeftHor"] && !rays["BottomLeftHor"] && !rays["MiddleLeft"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Input.GetKeyDown(KeyCode.W) && !rays["TopLeftVer"] && !rays["TopRightVer"] && !rays["MiddleTop"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !rays["TopRightHor"] && !rays["BottomRightHor"] && !rays["MiddleRight"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S) && !rays["BottomLeftVer"] && !rays["BottomRightVer"] && !rays["MiddleBottom"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }

    }

    void TurnAuto()
    {
        if (!rays["TopLeftHor"] && !rays["BottomLeftHor"] && !rays["MiddleLeft"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (!rays["TopLeftVer"] && !rays["TopRightVer"] && !rays["MiddleTop"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        /*else if (!rays["TopRightHor"] && !rays["BottomRightHor"] && !rays["MiddleLeft"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }*/
        /*else if (!rays["BottomLeftVer"] && !rays["BottomRightVer"] && !rays["MiddleBottom"])
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }*/
    }

    void SendRays()
    {
        rays.Clear();

        rays.Add("TopLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, col.bounds.size.y / 2 - errorMargin),
            Vector3.left , raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("TopLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2 + errorMargin, col.bounds.size.y / 2),
            Vector3.up  , raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("TopRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2 - errorMargin),
            Vector3.right, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("TopRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2 - errorMargin, col.bounds.size.y / 2),
            Vector3.up, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2 + errorMargin),
            Vector3.right, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2 - errorMargin, -col.bounds.size.y / 2),
            Vector3.down, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2 + errorMargin),
            Vector3.left , raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2 + errorMargin, -col.bounds.size.y / 2),
            Vector3.down , raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleTop", Physics2D.Raycast(transform.position + new Vector3(0, col.bounds.size.y / 2), Vector3.up, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleRight", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, 0), Vector3.right, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleBottom", Physics2D.Raycast(transform.position + new Vector3(0, -col.bounds.size.y / 2), Vector3.down, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleLeft", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, 0), Vector3.left, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
    }

}
