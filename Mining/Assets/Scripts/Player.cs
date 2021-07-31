using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Player : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Animator anim;
    public static bool isMining = true;
    public static GameObject focus;
    public static GameObject mouseOver;
    public static Transform playerPos;
    public static GameObject target;
    public LayerMask block;
    public Transform playerHead;
    public float gameReach;
    public Transform playerHeadLower;
    float realReach;
    public float blockSize;
    public Vector3 high;
    public Vector3 low;
    Vector3 zero;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;

    // Use this for initialization
    void Start()
    {
        realReach = gameReach *= blockSize;
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
    }


    // Update is called once per frame
    void Update()
    {
        playerPos = gameObject.transform;

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

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }

        //Mining
        if (target != null)
        {
            if (target.transform.position.y > playerHead.transform.position.y)
            {
                playerHead.transform.localPosition = high;
                print("block to be mined by higherhead");
                Vector2 direction = (target.transform.position - playerHead.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(playerHead.transform.position, direction, realReach, block);

                Debug.DrawRay(playerHead.transform.position, direction * 100f, Color.red);
                if (!hit)
                {
                    return;
                }

                if (hit.transform.gameObject.tag == "Minable")
                {
                    focus = target;
                    target.GetComponent<BlockManagerStone>().Mine();
                }
                playerHead.transform.position = zero;
                target = null;
            }
            else if (target.transform.position.y <= playerHead.transform.position.y)
            {
                playerHeadLower.transform.localPosition = low;
                print("block to be mined by lowerHead");
                Vector2 direction = (target.transform.position - playerHeadLower.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(playerHeadLower.transform.position, direction, realReach, block);

                Debug.DrawRay(playerHeadLower.transform.position, direction * realReach, Color.red);
                if (!hit)
                {
                    return;
                }

                if (hit.transform.gameObject.tag == "Minable")
                {
                    focus = target;
                    target.GetComponent<BlockManagerStone>().Mine();
                }
                playerHeadLower.transform.position = zero;
                target = null;
            }
        }
    }

    IEnumerator Mine()
    {
        anim.SetBool("PickSwing", true);
        isMining = true;
        yield return new WaitForSeconds(1);
        isMining = false;
    }

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
}