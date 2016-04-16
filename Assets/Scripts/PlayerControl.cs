using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;

    public bool lockMovement = false;
    private float speedBurst = 30.0f;
    private float speedDecayLimit = 15.0f;
    public float speed;

    private float jumpSpeed = 10.0f;
    public bool grounded = false;
    private float poundSpeed = 10.0f;
    public bool canPound = false;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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

    }

    void Move (float dir)
    {
        // we want a small acceleration at begining of movement, then speed is decaying
        transform.Translate(dir * speed * Time.deltaTime, 0, 0);
        if (speed > speedDecayLimit)
            speed -= speed * Time.deltaTime;
    }

    void Jump ()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        grounded = false;
        canPound = true;
    }

    void Pound ()
    {
        lockMovement = true;
        rb.AddForce(Vector3.down * poundSpeed, ForceMode2D.Impulse);
        StartCoroutine(AllowMovement());
    }


    void ResetSpeed ()
    {
        speed = speedBurst;
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
