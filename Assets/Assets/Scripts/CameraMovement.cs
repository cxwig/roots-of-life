using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject targetObject;
    public float smoothSpeed = 0.250f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = targetObject.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
