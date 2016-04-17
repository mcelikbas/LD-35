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


        Vector3 newPos = target.transform.position;
        Camera camera = GetComponent<Camera>();

        Vector3 roundPos = new Vector3(RoundToNearestPixel(newPos.x, camera), RoundToNearestPixel(newPos.y, camera), camera.transform.position.z);
        transform.position = roundPos;

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



    float RoundToNearestPixel (float unityUnits, Camera viewingCamera)
    {
        float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
        return adjustedUnityUnits;
    }
}
