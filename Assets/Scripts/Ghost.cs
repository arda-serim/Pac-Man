using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour
{
    protected GameObject pacman;

    [SerializeField] protected Sprite[] sprites = new Sprite[4];

    [SerializeField]protected int speed;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    protected Dictionary<string, RaycastHit2D> rays = new Dictionary<string, RaycastHit2D>();
    private Collider2D col;
    float errorMargin = 0;
    float raycastDistance = 0.5f;

    int spriteValue;
    float minDistance;

    protected bool isSpriteChanged;
    protected bool tempBool;
    Sprite tempSprite;

    bool cannotTurnUp;

    void Start()
    {
        pacman = GameObject.Find("PacMan");
        col = gameObject.GetComponent<Collider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SendRays();
        isSpriteChanged = false;
        tempBool = false;
        tempSprite = spriteRenderer.sprite;
    }

     
    protected void MoveForward()
    {
        Vector3 tempVec = Vector3.zero;

        if (spriteRenderer.sprite.name.Contains("Down"))
        {
            tempVec = -transform.up;
        }
        else if (spriteRenderer.sprite.name.Contains("Left"))
        {
            tempVec = -transform.right;
        }
        else if (spriteRenderer.sprite.name.Contains("Right"))
        {
            tempVec = transform.right;
        }
        else if (spriteRenderer.sprite.name.Contains("Up"))
        {
            tempVec = transform.up;
        }

        rb.MovePosition(transform.position + tempVec * Time.deltaTime * speed);
    }

    protected void Turn(Vector3 waypoint)
    {
        minDistance = 100;

        if (!spriteRenderer.sprite.name.Contains("Left") && !rays["TopRightHor"] && !rays["BottomRightHor"] && !rays["MiddleRight"] && Vector3.Distance(transform.position + new Vector3(col.bounds.size.x / 2, 0), waypoint) <= minDistance)
        {
            minDistance = Vector3.Distance(transform.position + new Vector3(col.bounds.size.x / 2, 0), waypoint);
            spriteValue = 0;
        }
        if (!spriteRenderer.sprite.name.Contains("Up") && !rays["BottomLeftVer"] && !rays["BottomRightVer"] && !rays["MiddleBottom"] && Vector3.Distance(transform.position + new Vector3(0, -col.bounds.size.y / 2), waypoint) <= minDistance)
        {
            minDistance = Vector3.Distance(transform.position + new Vector3(0, -col.bounds.size.y / 2), waypoint);
            spriteValue = 1;
        }
        if (!spriteRenderer.sprite.name.Contains("Right") & !rays["TopLeftHor"] && !rays["BottomLeftHor"] && !rays["MiddleLeft"] && Vector3.Distance(transform.position + new Vector3(-col.bounds.size.x / 2, 0), waypoint) <= minDistance)
        {
            minDistance = Vector3.Distance(transform.position + new Vector3(-col.bounds.size.x / 2, 0), waypoint);
            spriteValue = 2;
        }
        if (!spriteRenderer.sprite.name.Contains("Down") && !cannotTurnUp && !rays["TopLeftVer"] && !rays["TopRightVer"] && !rays["MiddleTop"] && Vector3.Distance(transform.position + new Vector3(0, col.bounds.size.y / 2), waypoint) <= minDistance)
        {
            minDistance = Vector3.Distance(transform.position + new Vector3(0, col.bounds.size.y / 2), waypoint);
            spriteValue = 3;
        }

        spriteRenderer.sprite = sprites[spriteValue];
    }

    protected void SendRays()
    {
        rays.Clear();

        rays.Add("TopLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, col.bounds.size.y / 2 - errorMargin),
            Vector3.left, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("TopLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2 + errorMargin, col.bounds.size.y / 2),
            Vector3.up, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("TopRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, col.bounds.size.y / 2 - errorMargin),
            Vector3.right, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("TopRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2 - errorMargin, col.bounds.size.y / 2),
            Vector3.up, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomRightHor", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, -col.bounds.size.y / 2 + errorMargin),
            Vector3.right, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomRightVer", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2 - errorMargin, -col.bounds.size.y / 2),
            Vector3.down, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomLeftHor", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, -col.bounds.size.y / 2 + errorMargin),
            Vector3.left, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("BottomLeftVer", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2 + errorMargin, -col.bounds.size.y / 2),
            Vector3.down, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleTop", Physics2D.Raycast(transform.position + new Vector3(0, col.bounds.size.y / 2), Vector3.up, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleRight", Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2, 0), Vector3.right, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleBottom", Physics2D.Raycast(transform.position + new Vector3(0, -col.bounds.size.y / 2), Vector3.down, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
        rays.Add("MiddleLeft", Physics2D.Raycast(transform.position + new Vector3(-col.bounds.size.x / 2, 0), Vector3.left, raycastDistance, 1 << LayerMask.NameToLayer("Maze")));
    }

    protected void SpriteChecker()
    {
        if (tempSprite != spriteRenderer.sprite)
        {
            isSpriteChanged = true;
        }

        tempSprite = spriteRenderer.sprite;
    }

    protected IEnumerator IsSpriteChangedChanger()
    {
        tempBool = true;

        yield return new WaitForSeconds(0.3f);

        isSpriteChanged = false;
        tempBool = false;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("CannotTurnUp"))
        {
            cannotTurnUp = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("CannotTurnUp"))
        {
            cannotTurnUp = false;
        }
    }

    public abstract Vector3 SetWaypoint();
}
