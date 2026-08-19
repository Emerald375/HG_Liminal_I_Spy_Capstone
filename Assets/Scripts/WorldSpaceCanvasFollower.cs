using UnityEngine;

public class WorldSpaceCanvasFollower : MonoBehaviour
{
    [Header("Horizontal Tracker")]
    public Transform horizontalTracker;

    [Header("Canvas Position")]
    public Vector3 localPosition = new Vector3(0f, 0f, 0f);

    [Header("Canvas Rotation")]
    public Vector3 localRotation = Vector3.zero;

    private void Start()
    {
        if (horizontalTracker == null)
            return;

        // Parent the canvas to the horizontal tracker.
        transform.SetParent(horizontalTracker);

        // Reset local transform.
        transform.localPosition = localPosition;
        transform.localEulerAngles = localRotation;
    }
}