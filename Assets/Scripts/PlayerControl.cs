using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public bool lockMovement = false;
    private float speedBurst = 30.0f;
    private float speedDecayLimit = 15.0f;
    public float speed;

    private float jumpSpeed = 15.0f;
    public bool grounded = false;
    private float poundSpeed = 20.0f;
    public bool canPound = false;


    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }


    void Update ()
    {
        // MOVEMENT
        float horInput = Input.GetAxis("Horizontal");
        if (!lockMovement)
        {
            if (horInput != 0.0f)
            {
                Move(horInput);
            }
            else
            {
                ResetSpeed();
            }
        }

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

    void Move (float dir)
    {
        // we want a small acceleration at begining of movement, then speed is decaying
        transform.Translate(dir * speed * Time.deltaTime, 0, 0);
        if (speed > speedDecayLimit)
            speed -= speed * Time.deltaTime;
        if (grounded)
            ShapeshiftInCircle();
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

    void ResetSpeed ()
    {
        speed = speedBurst;
        if (grounded)
            ShapeshiftInSquare();
    }


    void OnCollisionStay2D (Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
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
