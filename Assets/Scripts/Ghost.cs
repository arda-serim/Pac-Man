using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour
{
    enum Phase
    {
        Chase,
        Scatter,
        Frightened,
        Dead
    }
    Phase phase;

    protected GameObject pacman;

    protected bool scatterMode;

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
        phase = Phase.Chase;
        StartCoroutine(ChaseScatterChanger());
    }

    private void Update()
    {
        SpriteChecker();
        if (isSpriteChanged && !tempBool)
        {
            StartCoroutine(IsSpriteChanger());
        }
        PhaseController();
        transform.rotation = Quaternion.identity;
        MoveForward();
        SendRays();
    }

    /// <summary>
    /// Change phase between chase and scatter. Hardly make mistake becaues of everytime change phase it controls that it is in sactter or chase.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ChaseScatterChanger()
    {
        while (true)
        {
            if (phase == Phase.Chase)
            {
                yield return new WaitForSeconds(Random.Range(7,14));
                if (phase == Phase.Chase)
                {
                    phase = Phase.Scatter;

                    yield return new WaitForSeconds(Random.Range(5, 10));
                    if (phase == Phase.Scatter)
                    {
                        phase = Phase.Chase;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Change ghost phase to frightened.Hardly make mistake becaues of everytime change phase it controls that it is in frightened
    /// </summary>
    /// <returns></returns>
    protected IEnumerator FrightenedChanger()
    {
        phase = Phase.Frightened;

        yield return new WaitForSeconds(10);

        if (phase == Phase.Frightened)
        {
            phase = Phase.Chase;
        }
    }

    /// <summary>
    /// Controls and change ophase to Dead
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == pacman)
        {
            phase = Phase.Dead;
        }
    }

    /// <summary>
    /// Make phase move
    /// </summary>
    protected void PhaseController()
    {
        switch (phase)
        {
            case Phase.Chase:
                scatterMode = false;
                Chase(SetWaypoint());
                break;
            case Phase.Scatter:
                scatterMode = true;
                Chase(SetWaypoint());
                break;
            case Phase.Frightened:
                break;
            case Phase.Dead:
                break;
        }
    }

    /// <summary>
    /// Move to the faced ghost direction
    /// </summary>
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

    /// <summary>
    /// Chase phase thing. (In scatter mode it chases the self scatter waypoint)
    /// </summary>
    /// <param name="waypoint"></param>
    protected void Chase(Vector3 waypoint)
    {
        if (!isSpriteChanged)
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
    }

    /// <summary>
    /// Send ray for not doing wrong turn
    /// </summary>
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

    /// <summary>
    /// Checks last frame sprite to this frame sprite to change isSpriteChange variable
    /// </summary>
    protected void SpriteChecker()
    {
        if (tempSprite != spriteRenderer.sprite)
        {
            isSpriteChanged = true;
        }

        tempSprite = spriteRenderer.sprite;
    }

    /// <summary>
    /// When isSpriteChanged == true. wait 0.3f then make it false:
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IsSpriteChanger()
    {
        tempBool = true;

        yield return new WaitForSeconds(0.3f);
        isSpriteChanged = false;
        tempBool = false;
    }

    /// <summary>
    /// Detect if ghost in cannotTurnUp place
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("CannotTurnUp"))
        {
            cannotTurnUp = true;
        }
    }

    /// <summary>
    /// Make cannotTurnUp false when exit area
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("CannotTurnUp"))
        {
            cannotTurnUp = false;
        }
    }

    /// <summary>
    /// Every ghost set self waypoint
    /// </summary>
    /// <returns></returns>
    public abstract Vector3 SetWaypoint();
}
