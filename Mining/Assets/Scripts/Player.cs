using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Player : MonoBehaviour
{
    #region Variables
    //Variables
    // Move player in 2D space
    public static GameObject player;
    public Animator anim;
    public static bool isMining = true;
    public static Vector3 playerPos;
    public LayerMask block;
    public static float realReach;
    public Vector3 spawnFallback;
    [SerializeField] public Transform playerMiddle;
    [SerializeField] public static GameObject mouseOver;
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
        player = gameObject;
        SpawnTest();
        playerPos = gameObject.transform.position;
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
        //Sending a raycast to see if the player spawned in a block
        RaycastHit2D ray = Physics2D.Raycast(playerMiddle.transform.position, Vector2.down, 5f);

        Debug.DrawRay(playerMiddle.transform.position, Vector2.down, Color.red, 5f);
        //If they spawned in a block then teliport them up by 1 untill they are not in a block
        if (ray)
        {
            gameObject.transform.position = gameObject.transform.position += spawnFallback;
            SpawnTest();
        }
    }
    #endregion
}