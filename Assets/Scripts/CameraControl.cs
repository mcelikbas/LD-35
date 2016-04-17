using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public float shakeDuration = 0.5f;
    private float mTime = 0.0f;
    public bool isShaking = false;

    void Update () 
	{
        ShakeCamera();
    }

    void LateUpdate ()
    {
        if (target)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }


    void ShakeCamera ()
    {
        if (isShaking)
        {
            float noise = Mathf.PerlinNoise(Time.time, 0.0f);
            Vector3 randomPoint = Random.insideUnitSphere;
            Vector3 point = new Vector3(Mathf.Clamp(randomPoint.x, 0.0f, 0.5f), Mathf.Clamp(randomPoint.y, 0.0f, 0.5f), 0.0f);
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.localPosition = targetPos + point * noise;
            mTime += Time.deltaTime;
        }

        if (mTime > shakeDuration)
        {
            isShaking = false;
            mTime = 0.0f;
        }
    }
}
