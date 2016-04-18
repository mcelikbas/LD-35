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
    public bool isPounding = false;

    private AudioSource audioSource;
    public AudioClip jumpSnd;
    public AudioClip poundSnd;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        cam = Camera.main;
        audioSource = gameObject.GetComponent<AudioSource>();
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
    }

    void FixedUpdate ()
    {
        // MOVEMENT
        float horInput = Input.GetAxisRaw("Horizontal");
        if (!lockMovement)
        {
            if (horInput != 0.0f)
            {
                //transform.localScale = new Vector3(horInput, 1, 1);
                rb.velocity = new Vector3(horInput * speed, rb.velocity.y, 0);
                if (grounded)
                    ShapeshiftInCircle();
            }
            else
            {
                if (grounded)
                {
                    StopMovement();
                    ShapeshiftInSquare();
                }
            }
        }
    }
    
    void Jump ()
    {
        grounded = false;
        canPound = true;
        ShapeshiftInTriangle();
        audioSource.PlayOneShot(jumpSnd, 0.3F);
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
    }

    void Pound ()
    {
        lockMovement = true;
        StopMovement();
        isPounding = true;
        ShapeshiftInSquare();
        rb.AddForce(Vector3.down * poundSpeed, ForceMode2D.Impulse);
        StartCoroutine(AllowMovement(0.3f));
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


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = col.transform;
        }

        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && isPounding)
        {
            audioSource.PlayOneShot(poundSnd, 0.3F);
            cam.GetComponent<CameraControl>().isShaking = true;
        }
    }

    void OnCollisionStay2D (Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
            canPound = false;
            isPounding = false;
        }
    }

    void OnCollisionExit2D (Collision2D col)
    {
        grounded = false;
        canPound = true;
        ShapeshiftInTriangle();

        if (col.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }

    void StopMovement ()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    IEnumerator AllowMovement (float delay)
    {
        yield return new WaitForSeconds(delay);
        lockMovement = false;
    }
}
