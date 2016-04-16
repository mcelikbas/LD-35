using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public bool lockMovement = false;
    private float speed = 15.0f;

    private float jumpSpeed = 10.0f;
    public bool grounded = false;
    private float poundSpeed = 15.0f;
    public bool canPound = false;


    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }


    void Update ()
    {
        print(rb.velocity.x);

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

        // SHAPESHIFTING
        if (Input.GetButtonDown("Triangle"))
        {
            ShapeshiftInTriangle();
        }
        else if (Input.GetButtonDown("Square"))
        {
            ShapeshiftInSquare();
        }
        else if (Input.GetButtonDown("Circle"))
        {
            ShapeshiftInCircle();
        }
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
                    ShapeshiftInCircle();
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                ShapeshiftInSquare();
            }
        }
    }

    void Jump ()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        grounded = false;
        canPound = true;
        ShapeshiftInTriangle();
    }

    void Pound ()
    {
        ShapeshiftInSquare();
        lockMovement = true;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
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


    IEnumerator AllowMovement ()
    {
        yield return new WaitForSeconds(0.3f);
        lockMovement = false;
    }
}
