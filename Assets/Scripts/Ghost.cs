using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Ghost : MonoBehaviour
{
    protected enum Phase
    {
        Chase,
        Scatter,
        Frightened,
        Dead
    }

    protected Phase phase;

    protected GameObject pacman;

    [SerializeField] protected Sprite[] sprites = new Sprite[12];

    [SerializeField]protected int speed;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    Animator animator;

    protected Dictionary<string, RaycastHit2D> rays = new Dictionary<string, RaycastHit2D>();
    private Collider2D col;
    float errorMargin = 0;
    float raycastDistance = 0.5f;

    int direction;
    float minDistance;

    protected bool isSpriteChanged;
    protected bool tempBool;
    Sprite tempSprite;

    void Awake()
    {
        pacman = GameObject.Find("PacMan");
        col = gameObject.GetComponent<Collider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        List<GameObject> gameObjects = GameObject.FindGameObjectsWithTag("Ghost").ToList<GameObject>();

        for (int i = 0; i < 3; i++ )
        {
            for (int j = i + 1; j < gameObjects.Count; j++)
            {
                Physics2D.IgnoreCollision(gameObjects[i].GetComponent<Collider2D>(), gameObjects[j].GetComponent<Collider2D>());
            }
        }
    }

    void OnEnable()
    {
        SendRays();
        isSpriteChanged = false;
        tempBool = false;
        tempSprite = spriteRenderer.sprite;
        phase = Phase.Chase;
        Physics2D.IgnoreCollision(pacman.GetComponent<Collider2D>(), col, false);

        GameManager.Instance.frightened += FrightenedChangerStarter;
    }
    private void OnDisable()
    {
        GameManager.Instance.frightened -= FrightenedChangerStarter;
    }

    void Update()
    {
        SpriteChecker();
        if (isSpriteChanged && !tempBool)
        {
            StartCoroutine(IsSpriteChangedChanger());
        }
        StartCoroutine(ChaseChanger());
        StartCoroutine(ScatterChanger());
        Turn(SetWaypoint());
        transform.rotation = Quaternion.identity;
        MoveForward();
        SendRays();
    }

    /// <summary>
    /// Change phase between chase and scatter. Hardly make mistake becaues of everytime change phase it controls that it is in sactter or chase.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ChaseChanger()
    {
        if (phase == Phase.Chase)
        {
            yield return new WaitForSeconds(20);
            if (phase == Phase.Chase)
            {
                phase = Phase.Scatter;
            }
        }
    }

    protected IEnumerator ScatterChanger()
    {
        if (phase == Phase.Scatter)
        {
            yield return new WaitForSeconds(7);
            if (phase == Phase.Scatter)
            {
                phase = Phase.Chase;
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
        spriteRenderer.sprite = sprites[4];
        animator.SetInteger("AnimationPhase", 4);

        yield return new WaitForSeconds(7);
        if (phase == Phase.Frightened)
        {
            animator.SetInteger("AnimationPhase", 5);
        }

        yield return new WaitForSeconds(3);
        if (phase == Phase.Frightened)
        {
            phase = Phase.Chase;
        }
    }

    void FrightenedChangerStarter()
    {
        if (phase != Phase.Dead)
            StartCoroutine(FrightenedChanger());
    }

    /// <summary>
    /// Controls and change ophase to Dead
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ( phase == Phase.Frightened && collision.gameObject == pacman)
        {
            phase = Phase.Dead;
            Physics2D.IgnoreCollision(pacman.GetComponent<Collider2D>(), col);

        }
    }


    /// <summary>
    /// Move to the faced ghost direction
    /// </summary>
    protected void MoveForward()
    {
        Vector3 tempVec = Vector3.zero;

        if (direction == 1)
        {
            tempVec = -transform.up;
        }
        else if (direction == 2)
        {
            tempVec = -transform.right;
        }
        else if (direction == 0)
        {
            tempVec = transform.right;
        }
        else if (direction == 3)
        {
            tempVec = transform.up;
        }

        rb.MovePosition(transform.position + tempVec * Time.deltaTime * speed);
    }

    /// <summary>
    /// Chase phase thing. (In scatter mode it chases the self scatter waypoint)
    /// </summary>
    /// <param name="waypoint"></param>
    protected void Turn(Vector3 waypoint)
    {
        if (!isSpriteChanged)
        {
            minDistance = 100;

            if (direction != 2 && !rays["TopRightHor"] && !rays["BottomRightHor"] && !rays["MiddleRight"] && Vector3.Distance(transform.position + new Vector3(col.bounds.size.x / 2, 0), waypoint) <= minDistance)
            {
                minDistance = Vector3.Distance(transform.position + new Vector3(col.bounds.size.x / 2, 0), waypoint);
                direction = 0;
            }
            if (direction != 3 && !rays["BottomLeftVer"] && !rays["BottomRightVer"] && !rays["MiddleBottom"] && Vector3.Distance(transform.position + new Vector3(0, -col.bounds.size.y / 2), waypoint) <= minDistance)
            {
                minDistance = Vector3.Distance(transform.position + new Vector3(0, -col.bounds.size.y / 2), waypoint);
                direction = 1;
            }
            if (direction != 0 & !rays["TopLeftHor"] && !rays["BottomLeftHor"] && !rays["MiddleLeft"] && Vector3.Distance(transform.position + new Vector3(-col.bounds.size.x / 2, 0), waypoint) <= minDistance)
            {
                minDistance = Vector3.Distance(transform.position + new Vector3(-col.bounds.size.x / 2, 0), waypoint);
                direction = 2;
            }
            if (direction != 1 && !(((gameObject.transform.position.x > -1 && gameObject.transform.position.x < 1) && (transform.position.y < 1.5f && transform.position.y > 1)) || ((transform.position.x > -1 && transform.position.x < 1) && (transform.position.y < -2.3f && transform.position.y > -3)))  && !rays["TopLeftVer"] && !rays["TopRightVer"] && !rays["MiddleTop"] && Vector3.Distance(transform.position + new Vector3(0, col.bounds.size.y / 2), waypoint) <= minDistance)
            {
                minDistance = Vector3.Distance(transform.position + new Vector3(0, col.bounds.size.y / 2), waypoint);
                direction = 3;
            }

            if (phase == Phase.Frightened)
            {
                spriteRenderer.sprite = sprites[4];
                return;
            }

            if (phase == Phase.Dead)
            {
                spriteRenderer.sprite = sprites[direction + 5];
                animator.SetInteger("AnimationPhase", direction + 6);
                return;
            }

            //spriteRenderer.sprite = sprites[direction];
            animator.SetInteger("AnimationPhase", direction);
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
    protected IEnumerator IsSpriteChangedChanger()
    {
        tempBool = true;

        yield return new WaitForSeconds(0.4f);

        isSpriteChanged = false;
        tempBool = false;
    }



    /// <summary>
    /// Every ghost set self waypoint
    /// </summary>
    /// <returns></returns>
    public abstract Vector3 SetWaypoint();
}
