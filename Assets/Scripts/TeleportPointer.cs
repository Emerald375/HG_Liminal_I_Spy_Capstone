using UnityEngine;

/// <summary>
/// Attach one of these to each controller anchor (one on the left hand anchor,
/// one on the right hand anchor). Handles raycasting at hotspots, driving their
/// highlight state, and reading the trigger press that fires a teleport.
///
/// One of the two instances can also be flagged as the PC fallback pointer: when
/// no headset is present it will use the main camera's forward direction (driven
/// by your existing mouse-look) and a left mouse click instead of OVRInput.
/// </summary>
public class TeleportPointer : MonoBehaviour
{
    public enum Hand { Primary, Secondary }

    [Header("Setup")]
    [Tooltip("Which OVR controller this pointer represents. Maps directly to OVRInput.Button.PrimaryIndexTrigger / SecondaryIndexTrigger.")]
    [SerializeField] private Hand hand = Hand.Primary;

    [Tooltip("Where the ray is cast from, using this transform's forward direction. Usually the controller anchor (e.g. RightHandAnchor / LeftHandAnchor).")]
    [SerializeField] private Transform rayOrigin;

    [Tooltip("If true, and no headset is currently present (Editor/PC play), this pointer switches to the main camera's forward direction plus a left mouse click.")]
    [SerializeField] private bool isPCFallbackPointer = false;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private LayerMask hotspotLayerMask;

    [Header("Visuals (optional)")]
    [Tooltip("Optional LineRenderer to visualize the pointer ray.")]
    [SerializeField] private LineRenderer lineRenderer;

    private TeleportHotspot currentlyHighlighted;

    private void Update()
    {
        Transform origin = GetActiveRayOrigin();
        if (origin == null)
        {
            // Not the active pointer this frame (e.g. VR-only pointer while on PC).
            UpdateHighlight(null);
            return;
        }

        bool hitSomething = Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, maxDistance, hotspotLayerMask, QueryTriggerInteraction.Collide);
        TeleportHotspot hotspot = hitSomething ? hit.collider.GetComponentInParent<TeleportHotspot>() : null;

        UpdateHighlight(hotspot);
        UpdateLineRenderer(origin, hitSomething ? hit.point : origin.position + origin.forward * maxDistance);

        if (hotspot != null && WasTriggerPressedThisFrame())
        {
            TeleportManager.Instance.TeleportTo(hotspot);
        }
    }

    private Transform GetActiveRayOrigin()
    {
        if (!OVRManager.isHmdPresent)
        {
            if (!isPCFallbackPointer) return null;
            return Camera.main != null ? Camera.main.transform : rayOrigin;
        }

        return rayOrigin;
    }

    private bool WasTriggerPressedThisFrame()
    {
        if (!OVRManager.isHmdPresent)
        {
            return isPCFallbackPointer && Input.GetMouseButtonDown(0);
        }

        OVRInput.Button button = hand == Hand.Primary
            ? OVRInput.Button.PrimaryIndexTrigger
            : OVRInput.Button.SecondaryIndexTrigger;

        return OVRInput.GetDown(button);
    }

    private void UpdateHighlight(TeleportHotspot hotspot)
    {
        if (hotspot == currentlyHighlighted) return;

        if (currentlyHighlighted != null)
        {
            currentlyHighlighted.SetHighlighted(false);
        }

        currentlyHighlighted = hotspot;

        if (currentlyHighlighted != null)
        {
            currentlyHighlighted.SetHighlighted(true);
        }
    }

    private void UpdateLineRenderer(Transform origin, Vector3 endPoint)
    {
        if (lineRenderer == null) return;
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, endPoint);
    }
}