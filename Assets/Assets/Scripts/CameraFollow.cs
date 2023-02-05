using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float offset = -10f;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 target;
    [SerializeField] GameObject gameHandler;
    RootManager rootManager;


    // Start is called before the first frame update
    void Awake ()
    {
        rootManager = gameHandler.GetComponent<RootManager>();
    }

    void Update()
    {
        target = new Vector3(transform.position.x, rootManager.GetLowest(), offset);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = target;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
