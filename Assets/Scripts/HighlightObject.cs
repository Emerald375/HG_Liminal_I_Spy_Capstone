using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    [Header("Controller")]
    public Transform controllerTransform;
    public LayerMask highlightLayer;

    [Header("Outline")]
    public Outline outline;

    [Header("Outline Colours")]
    public Color hoverColor = Color.yellow;
    public Color indexTriggerColor = Color.green;
    public Color handTriggerColor = Color.blue;

    private enum HighlightState
    {
        None,
        Hover,
        IndexTrigger,
        HandTrigger
    }

    private HighlightState currentState = HighlightState.None;

    void Start()
    {
        // Automatically find the Outline if one hasn't been assigned
        if (outline == null)
        {
            outline = GetComponent<Outline>();
        }

        // Make sure the outline starts disabled
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    void Update()
    {
        RaycastHit hit;

        // Check if the controller is pointing at this object
        if (Physics.Raycast(
            controllerTransform.position,
            controllerTransform.forward,
            out hit,
            Mathf.Infinity,
            highlightLayer))
        {
            // Make sure the object being hit is this object
            if (hit.collider.gameObject == gameObject)
            {
                HandleHover();
            }
        }
        else
        {
            // Controller is no longer pointing at the object
            ResetHighlight();
        }
    }

    void HandleHover()
    {
        // Enable the outline
        if (outline != null)
        {
            outline.enabled = true;
        }

        // Hover state
        if (currentState == HighlightState.None)
        {
            currentState = HighlightState.Hover;
            SetOutlineColour(hoverColor);
        }

        // Index Trigger OR Left Mouse Button
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
            Input.GetMouseButtonDown(0))
        {
            currentState = HighlightState.IndexTrigger;
            SetOutlineColour(indexTriggerColor);
        }

        // Hand Trigger OR Right Mouse Button
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) ||
            Input.GetMouseButtonDown(1))
        {
            currentState = HighlightState.HandTrigger;
            SetOutlineColour(handTriggerColor);
        }
    }

    void SetOutlineColour(Color colour)
    {
        if (outline != null)
        {
            outline.OutlineColor = colour;
        }
    }

    void ResetHighlight()
    {
        currentState = HighlightState.None;

        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}