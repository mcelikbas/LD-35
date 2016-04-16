using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float speed = 20.0f;
    public float speedDecayLimit = 10.0f;
    public bool isMoving = false;


    void Update ()
    {
        float horInput = Input.GetAxis("Horizontal");
        if (horInput != 0.0f)
        {
            isMoving = true;
            transform.Translate(horInput * speed * Time.deltaTime, 0, 0);

            if (speed > speedDecayLimit)
                speed -= speed * Time.deltaTime;
        }
        else
        {
            isMoving = false;
            ResetSpeed();
        }
	}

    void ResetSpeed ()
    {
        speed = 20.0f;
    }
}
