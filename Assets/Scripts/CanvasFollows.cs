using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollows : MonoBehaviour
{
    [SerializeField] private Transform eyeCamera;

    [SerializeField] private float distance = 2f;
    [SerializeField] private float speed = 5f;

    void Update()
    {
        Vector3 targetPosition = eyeCamera.position + (eyeCamera.forward * distance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        Vector3 lookAtTarget = transform.position + eyeCamera.forward;
        transform.LookAt(lookAtTarget, Vector3.up);
    }
}
