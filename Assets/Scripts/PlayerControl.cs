using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    Camera cam;

    private float speed = 15.0f;
    private float jumpSpeed = 12.0f;
    private float poundSpeed = 20.0f;

    public bool lockMovement = false;
    public bool grounded = false;
    public bool canPound = false;

    public enum playerState
    {
        IDLE,
        RUNNING,
        INAIR,
        POUNDING
    };
    public playerState pState;
    public playerState lastState;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        cam = Camera.main;

        pState = playerState.IDLE;
        lastState = playerState.IDLE;
    }

    void Update ()
    {
        // JUMPING && POUNDING
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                Jump();
            }
            else
            {
                if (canPound)
                {
                    Pound();
                }
            }
        }

        CheckState();
    }

    void FixedUpdate ()
    {
        // MOVEMENT
        float horInput = Input.GetAxisRaw("Horizontal");
        if (!lockMovement)
        {
            if (horInput != 0.0f)
            {
                rb.velocity = new Vector3(horInput * speed, rb.velocity.y, 0);
                if (grounded)
                    ChangeState(playerState.RUNNING);
            }
            else
            {
                if (grounded)
                {
                    StopMovement();
                    ChangeState(playerState.IDLE);
                }
            }
        }
    }

    void ChangeState (playerState newState)
    {
        lastState = pState;
        pState = newState;
    }

    void CheckState ()
    {
        if (pState == playerState.IDLE || pState == playerState.POUNDING)
        {
            ShapeshiftInSquare();
        }
        else if (pState == playerState.RUNNING)
        {
            ShapeshiftInCircle();
        }
        else if (pState == playerState.INAIR)
        {
            ShapeshiftInTriangle();
        }
    }
    
    void Jump ()
    {
        grounded = false;
        canPound = true;
        ChangeState(playerState.INAIR);
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
    }

    void Pound ()
    {
        lockMovement = true;
        StopMovement();
        ChangeState(playerState.POUNDING);
        cam.GetComponent<CameraControl>().isShaking = true;
        rb.AddForce(Vector3.down * poundSpeed, ForceMode2D.Impulse);
        StartCoroutine(AllowMovement());
    }

    void ShapeshiftInTriangle ()
    {
        animator.SetBool("Triangle", true);
        animator.SetBool("Square", false);
        animator.SetBool("Circle", false);
    }

    void ShapeshiftInSquare ()
    {
        animator.SetBool("Square", true);
        animator.SetBool("Triangle", false);
        animator.SetBool("Circle", false);
    }

    void ShapeshiftInCircle ()
    {
        animator.SetBool("Circle", true);
        animator.SetBool("Square", false);
        animator.SetBool("Triangle", false);
    }


    void OnCollisionStay2D (Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle"))
        {
            grounded = true;
            canPound = false;
        }
    }

    void OnCollisionExit2D (Collision2D coll)
    {
        grounded = false;
        canPound = true;
        ChangeState(playerState.INAIR);
    }

    void StopMovement ()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    IEnumerator AllowMovement ()
    {
        yield return new WaitForSeconds(0.3f);
        lockMovement = false;
    }
}
