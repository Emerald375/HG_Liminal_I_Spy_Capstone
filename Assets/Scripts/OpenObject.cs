using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OpenObject : MonoBehaviour
{
    public enum OpenState
    {
        Closed,
        Opening,
        Open,
        Closing
    }


    [Header("Hover / Pointing")]

    [Tooltip("Controller transform used for the primary VR pointing ray.")]
    public Transform primaryPointer;

    [Tooltip("Controller transform used for the secondary VR pointing ray.")]
    public Transform secondaryPointer;

    [Tooltip("Maximum distance the VR pointer can detect this object.")]
    public float pointerDistance = 10f;

    [Tooltip("Which layers can be detected by the VR pointer.")]
    public LayerMask pointerLayers = ~0;

    [Tooltip("Allow this object to be opened and closed using PC mouse controls.")]
    public bool enablePCControls = true;


    [Header("Quest Controls")]

    [Tooltip("Allow the Primary Index Trigger to open or close this object.")]
    public bool primaryIndexTrigger = true;

    [Tooltip("Allow the Secondary Index Trigger to open or close this object.")]
    public bool secondaryIndexTrigger = true;


    [Header("Closed State")]

    [Tooltip("Local position of the object when closed.")]
    public Vector3 closedPosition;

    [Tooltip("Local rotation of the object when closed.")]
    public Vector3 closedRotation;

    [Tooltip("Local scale of the object when closed.")]
    public Vector3 closedScale = Vector3.one;


    [Header("Open State")]

    [Tooltip("Local position of the object when open.")]
    public Vector3 openPosition;

    [Tooltip("Local rotation of the object when open.")]
    public Vector3 openRotation;

    [Tooltip("Local scale of the object when open.")]
    public Vector3 openScale = Vector3.one;


    [Header("Animation")]

    [Tooltip("How long it takes to fully open or close the object.")]
    public float animationDuration = 0.5f;

    [Tooltip("Animation curve used when opening and closing.")]
    public AnimationCurve animationCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);


    [Header("Events")]

    [Tooltip("Called when the object begins opening.")]
    public UnityEvent onOpen;

    [Tooltip("Called when the object begins closing.")]
    public UnityEvent onClose;

    [Tooltip("Called when the object has fully opened.")]
    public UnityEvent onOpened;

    [Tooltip("Called when the object has fully closed.")]
    public UnityEvent onClosed;


    [Header("Debug")]

    [SerializeField]
    private OpenState currentState = OpenState.Closed;


    private bool isHovered = false;

    private Coroutine animationCoroutine;


    /// <summary>
    /// Returns the current open state of this object.
    /// </summary>
    public OpenState CurrentState
    {
        get { return currentState; }
    }


    /// <summary>
    /// Returns true when the object is fully open.
    /// </summary>
    public bool IsOpen
    {
        get { return currentState == OpenState.Open; }
    }


    /// <summary>
    /// Returns true when the object is fully closed.
    /// </summary>
    public bool IsClosed
    {
        get { return currentState == OpenState.Closed; }
    }


    /// <summary>
    /// Returns true while the player is pointing at this object.
    /// </summary>
    public bool IsHovered
    {
        get { return isHovered; }
    }


    private void Start()
    {
        // Set the object to its defined closed state.
        transform.localPosition = closedPosition;
        transform.localRotation = Quaternion.Euler(closedRotation);
        transform.localScale = closedScale;
    }


    private void Update()
    {
        CheckHover();

        if (isHovered)
        {
            CheckQuestInput();
            CheckPCInput();
        }
    }


    // =========================================================
    // HOVER / POINTING
    // =========================================================

    private void CheckHover()
    {
        isHovered = false;


        // Check primary pointer.
        if (primaryPointer != null)
        {
            if (PointerHitsObject(primaryPointer))
            {
                isHovered = true;
                return;
            }
        }


        // Check secondary pointer.
        if (secondaryPointer != null)
        {
            if (PointerHitsObject(secondaryPointer))
            {
                isHovered = true;
                return;
            }
        }
    }


    private bool PointerHitsObject(Transform pointer)
    {
        Ray ray = new Ray(
            pointer.position,
            pointer.forward
        );

        RaycastHit hit;


        if (Physics.Raycast(
            ray,
            out hit,
            pointerDistance,
            pointerLayers,
            QueryTriggerInteraction.Collide))
        {
            // Check whether the pointer hit this object
            // or one of its child objects.
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }


    // =========================================================
    // QUEST INPUT
    // =========================================================

    private void CheckQuestInput()
    {
        bool primaryPressed = false;
        bool secondaryPressed = false;


#if UNITY_ANDROID || UNITY_EDITOR

        if (primaryIndexTrigger)
        {
            primaryPressed = OVRInput.GetDown(
                OVRInput.Button.PrimaryIndexTrigger
            );
        }


        if (secondaryIndexTrigger)
        {
            secondaryPressed = OVRInput.GetDown(
                OVRInput.Button.SecondaryIndexTrigger
            );
        }

#endif


        if (primaryPressed || secondaryPressed)
        {
            ToggleObject();
        }
    }


    // =========================================================
    // PC INPUT
    // =========================================================

    private void CheckPCInput()
    {
        if (!enablePCControls)
            return;


        // Left Mouse Button.
        if (Input.GetMouseButtonDown(0))
        {
            ToggleObject();
        }
    }


    // =========================================================
    // TOGGLE OBJECT
    // =========================================================

    /// <summary>
    /// Opens or closes the object depending on
    /// its current state.
    /// </summary>
    public void ToggleObject()
    {
        // If currently closed or closing,
        // begin opening.
        if (currentState == OpenState.Closed ||
            currentState == OpenState.Closing)
        {
            OpenObjectAnimation();
        }

        // If currently open or opening,
        // begin closing.
        else
        {
            CloseObjectAnimation();
        }
    }


    // =========================================================
    // OPEN OBJECT
    // =========================================================

    /// <summary>
    /// Begins opening the object.
    /// </summary>
    public void OpenObjectAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }


        currentState = OpenState.Opening;


        if (onOpen != null)
        {
            onOpen.Invoke();
        }


        animationCoroutine = StartCoroutine(
            AnimateToState(
                openPosition,
                Quaternion.Euler(openRotation),
                openScale,
                OpenState.Open
            )
        );
    }


    // =========================================================
    // CLOSE OBJECT
    // =========================================================

    /// <summary>
    /// Begins closing the object.
    /// </summary>
    public void CloseObjectAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }


        currentState = OpenState.Closing;


        if (onClose != null)
        {
            onClose.Invoke();
        }


        animationCoroutine = StartCoroutine(
            AnimateToState(
                closedPosition,
                Quaternion.Euler(closedRotation),
                closedScale,
                OpenState.Closed
            )
        );
    }


    // =========================================================
    // ANIMATION
    // =========================================================

    private IEnumerator AnimateToState(
        Vector3 targetPosition,
        Quaternion targetRotation,
        Vector3 targetScale,
        OpenState completedState)
    {
        // Store the CURRENT transform values.
        //
        // This is important because it means that if the
        // object is clicked halfway through opening or closing,
        // it will animate smoothly from its current position.
        Vector3 startPosition = transform.localPosition;

        Quaternion startRotation = transform.localRotation;

        Vector3 startScale = transform.localScale;


        float elapsed = 0f;


        if (animationDuration <= 0f)
        {
            transform.localPosition = targetPosition;

            transform.localRotation = targetRotation;

            transform.localScale = targetScale;
        }
        else
        {
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;


                float percentage = Mathf.Clamp01(
                    elapsed / animationDuration
                );


                float curvePercentage =
                    animationCurve.Evaluate(percentage);


                transform.localPosition = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    curvePercentage
                );


                transform.localRotation = Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    curvePercentage
                );


                transform.localScale = Vector3.Lerp(
                    startScale,
                    targetScale,
                    curvePercentage
                );


                yield return null;
            }


            // Ensure the object reaches the exact target state.
            transform.localPosition = targetPosition;

            transform.localRotation = targetRotation;

            transform.localScale = targetScale;
        }


        currentState = completedState;

        animationCoroutine = null;


        // Invoke the appropriate completed event.
        if (currentState == OpenState.Open)
        {
            if (onOpened != null)
            {
                onOpened.Invoke();
            }
        }
        else if (currentState == OpenState.Closed)
        {
            if (onClosed != null)
            {
                onClosed.Invoke();
            }
        }
    }


    // =========================================================
    // MANUAL CONTROLS
    // =========================================================

    /// <summary>
    /// Forces the object to begin opening.
    /// </summary>
    public void ForceOpen()
    {
        OpenObjectAnimation();
    }


    /// <summary>
    /// Forces the object to begin closing.
    /// </summary>
    public void ForceClose()
    {
        CloseObjectAnimation();
    }


    /// <summary>
    /// Instantly moves the object to its open state.
    /// </summary>
    public void SetOpenInstantly()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }


        transform.localPosition = openPosition;

        transform.localRotation =
            Quaternion.Euler(openRotation);

        transform.localScale = openScale;

        currentState = OpenState.Open;

        animationCoroutine = null;
    }


    /// <summary>
    /// Instantly moves the object to its closed state.
    /// </summary>
    public void SetClosedInstantly()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }


        transform.localPosition = closedPosition;

        transform.localRotation =
            Quaternion.Euler(closedRotation);

        transform.localScale = closedScale;

        currentState = OpenState.Closed;

        animationCoroutine = null;
    }
}