using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Player : MonoBehaviour
{
    #region Variables
    // Move player in 2D space
    public Animator anim;
    public static bool isMining = true;
    public static Vector3 playerPos;
    public LayerMask block;
    float realReach;
    public Vector3 spawnFallback;
    [SerializeField] float blockSize, gameReach;
    [SerializeField] public Transform playerHeadLower, playerHead, playerMiddle;
    [SerializeField] public static GameObject focus, mouseOver, target;
    [SerializeField] float maxSpeed = 3.4f, jumpHeight = 6.5f, gravityScale = 1.5f;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    public Transform movementChecker;

    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        SpawnTest();
        playerPos = gameObject.transform.position;
        realReach = gameReach *= blockSize;
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        #region Controlls

        // Movement controls
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        }
        else
        {
            if (isGrounded || r2d.velocity.magnitude < 0.1f)
            {
                moveDirection = 0;
            }
        }
        #endregion

        #region Updating player direction
        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }
        #endregion

        #region Jumping
        // Jumping
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }
        #endregion

        #region Mining logic
        //Mining
        if (target != null)
        {
            if (target.transform.position.y >= playerMiddle.transform.position.y)
            {
                Vector2 direction = (target.transform.position - playerHead.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(playerHead.transform.position, direction, realReach, block);

                Debug.DrawRay(playerHead.transform.position, direction * 100f, Color.red);
                if (!hit)
                {
                    print("Out Of Range");
                    target = null;
                    return;
                }

                if (hit.transform.gameObject.tag == "Minable")
                {
                    focus = target;
                    target.GetComponent<BlockManager>().Mine();
                }
                target = null;
            }
            else if (target.transform.position.y <= playerMiddle.transform.position.y)
            {
                Vector2 direction = (target.transform.position - playerHeadLower.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(playerHeadLower.transform.position, direction, realReach, block);

                Debug.DrawRay(playerHeadLower.transform.position, direction * realReach, Color.red);
                if (!hit)
                {
                    print("Out Of Range");
                    target = null;
                    return;
                }

                if (hit.transform.gameObject.tag == "Minable")
                {
                    focus = target;
                    target.GetComponent<BlockManager>().Mine();
                }
                target = null;
            }
        }
        #endregion
    }
    #endregion

    #region Mining function
    IEnumerator Mine()
    {
        anim.SetBool("PickSwing", true);
        isMining = true;
        yield return new WaitForSeconds(1);
        isMining = false;
    }
    #endregion

    #region Fixed Update
    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
    #endregion

    #region Spawn test function
    void SpawnTest()
    {
        RaycastHit2D ray = Physics2D.Raycast(playerMiddle.transform.position, Vector2.down, 5f);

        Debug.DrawRay(playerMiddle.transform.position, Vector2.down, Color.red, 5f);
        if (ray)
        {
            gameObject.transform.position = gameObject.transform.position += spawnFallback;
            SpawnTest();
        }
    }
    #endregion
}