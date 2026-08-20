using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SelectObject : MonoBehaviour
{
    public enum SelectionState
    {
        Available,
        Selecting,
        Selected,
        Disabled
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

    [Tooltip("Allow this object to be selected using PC mouse controls.")]
    public bool enablePCControls = true;


    [Header("Quest Controls")]

    [Tooltip("Allow the Primary Index Trigger to select this object.")]
    public bool primaryIndexTrigger = true;

    [Tooltip("Allow the Secondary Index Trigger to select this object.")]
    public bool secondaryIndexTrigger = true;


    [Header("Selection Animation")]

    [Tooltip("How long it takes for the object to shrink to zero.")]
    public float shrinkDuration = 0.5f;

    [Tooltip("Optional delay after the shrink animation has finished.")]
    public float selectionDelay = 0f;


    [Header("After Selection")]

    [Tooltip("Disable this object after it has been selected.")]
    public bool disableObject = true;

    [Tooltip("Destroy this object after it has been selected. This overrides Disable Object.")]
    public bool destroyObject = false;

    [Tooltip("Delay before disabling or destroying the object.")]
    public float disableDestroyDelay = 0f;


    [Header("Selection Sound")]

    [Tooltip("Sound played when this object is selected.")]
    public AudioClip selectionSound;

    [Tooltip("Audio Source used to play the selection sound.")]
    public AudioSource audioSource;

    [Range(0f, 1f)]
    [Tooltip("Volume of the selection sound.")]
    public float soundVolume = 1f;


    [Header("Selection Information")]

    [Tooltip("Optional point value associated with this object.")]
    public int points = 1;


    [Header("Events")]

    [Tooltip("Called immediately when this object is selected.")]
    public UnityEvent onSelected;

    [Tooltip("Called when this object is disabled.")]
    public UnityEvent onDisabled;

    [Tooltip("Sends the point value to another script when selected.")]
    public UnityEvent<int> onPointsAwarded;


    [Header("Debug")]

    [SerializeField]
    private SelectionState currentState = SelectionState.Available;


    private Vector3 originalScale;

    private bool isHovered = false;
    private bool hasBeenSelected = false;


    /// <summary>
    /// Returns the current selection state of this object.
    /// </summary>
    public SelectionState CurrentState
    {
        get { return currentState; }
    }


    /// <summary>
    /// Returns true once this object has been selected.
    /// </summary>
    public bool IsSelected
    {
        get
        {
            return currentState == SelectionState.Selected ||
                   currentState == SelectionState.Disabled;
        }
    }


    /// <summary>
    /// Returns true once this object has been disabled.
    /// </summary>
    public bool IsDisabled
    {
        get
        {
            return currentState == SelectionState.Disabled;
        }
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
        originalScale = transform.localScale;
    }


    private void Update()
    {
        if (hasBeenSelected)
            return;

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
        Ray ray = new Ray(pointer.position, pointer.forward);

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
            Select();
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
            Select();
        }
    }


    // =========================================================
    // SELECT OBJECT
    // =========================================================

    /// <summary>
    /// Selects this object.
    /// </summary>
    public void Select()
    {
        if (hasBeenSelected)
            return;


        hasBeenSelected = true;

        currentState = SelectionState.Selecting;


        // Play the selection sound.
        PlaySelectionSound();


        // Notify other scripts that this object was selected.
        if (onSelected != null)
        {
            onSelected.Invoke();
        }


        // Send the point value to another script.
        if (onPointsAwarded != null)
        {
            onPointsAwarded.Invoke(points);
        }


        StartCoroutine(SelectionAnimation());
    }


    // =========================================================
    // SELECTION ANIMATION
    // =========================================================

    private IEnumerator SelectionAnimation()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsed = 0f;


        if (shrinkDuration <= 0f)
        {
            transform.localScale = targetScale;
        }
        else
        {
            while (elapsed < shrinkDuration)
            {
                elapsed += Time.deltaTime;

                float percentage = Mathf.Clamp01(
                    elapsed / shrinkDuration
                );


                transform.localScale = Vector3.Lerp(
                    startScale,
                    targetScale,
                    percentage
                );

                yield return null;
            }

            transform.localScale = targetScale;
        }


        // The object has now been successfully selected.
        currentState = SelectionState.Selected;


        // Optional delay after shrinking.
        if (selectionDelay > 0f)
        {
            yield return new WaitForSeconds(selectionDelay);
        }


        // Optional delay before disabling/destroying.
        if (disableDestroyDelay > 0f)
        {
            yield return new WaitForSeconds(
                disableDestroyDelay
            );
        }


        // Destroy takes priority over disabling.
        if (destroyObject)
        {
            Destroy(gameObject);
            yield break;
        }


        if (disableObject)
        {
            DisableObject();
        }
    }


    // =========================================================
    // DISABLE OBJECT
    // =========================================================

    private void DisableObject()
    {
        currentState = SelectionState.Disabled;


        if (onDisabled != null)
        {
            onDisabled.Invoke();
        }


        gameObject.SetActive(false);
    }


    // =========================================================
    // SOUND
    // =========================================================

    private void PlaySelectionSound()
    {
        if (selectionSound == null)
            return;


        if (audioSource != null)
        {
            audioSource.PlayOneShot(
                selectionSound,
                soundVolume
            );
        }
        else
        {
            AudioSource.PlayClipAtPoint(
                selectionSound,
                transform.position,
                soundVolume
            );
        }
    }


    // =========================================================
    // MANUAL SELECTION
    // =========================================================

    /// <summary>
    /// Allows another script to select this object.
    /// </summary>
    public void ForceSelect()
    {
        Select();
    }


    /// <summary>
    /// Returns the original scale of the object.
    /// </summary>
    public Vector3 GetOriginalScale()
    {
        return originalScale;
    }


    /// <summary>
    /// Resets the object back to its original state.
    /// </summary>
    public void ResetObject()
    {
        StopAllCoroutines();

        transform.localScale = originalScale;

        currentState = SelectionState.Available;

        hasBeenSelected = false;
        isHovered = false;

        gameObject.SetActive(true);
    }
}