using UnityEngine;

public class HorizontalTracker : MonoBehaviour
{
    [Header("Object to Track")]
    public Transform headTransform;

    [Header("Settings")]
    public bool smoothRotation = false;
    public float rotationSpeed = 10f;

    private void LateUpdate()
    {
        if (headTransform == null)
            return;

        // Get the head's current Y rotation only.
        Vector3 headEuler = headTransform.eulerAngles;

        Quaternion targetRotation = Quaternion.Euler(
            0f,
            headEuler.y,
            0f
        );

        if (smoothRotation)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            transform.rotation = targetRotation;
        }
    }
}