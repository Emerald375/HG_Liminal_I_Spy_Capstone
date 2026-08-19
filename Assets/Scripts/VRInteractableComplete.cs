using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class VRInteractableComplete : MonoBehaviour
{
    public enum CompletionState
    {
        Available,
        Completing,
        Complete,
        Disabled
    }

    [Header("Hover / Pointing")]
    [Tooltip("Controller transform used for the VR pointing ray.")]
    public Transform primaryPointer;

    [Tooltip("Optional second controller transform used for the VR pointing ray.")]
    public Transform secondaryPointer;

    [Tooltip("Maximum distance the VR pointer can detect this object.")]
    public float pointerDistance = 10f;

    [Tooltip("Which layers can be detected by the VR pointer.")]
    public LayerMask pointerLayers = ~0;

    [Tooltip("If enabled, the object can also be activated with the PC mouse.")]
    public bool enablePCControls = true;

    [Header("Quest 3 Controls")]
    [Tooltip("Primary Index Trigger = right controller trigger.")]
    public bool primaryIndexTrigger = true;

    [Tooltip("Secondary Index Trigger = left controller trigger.")]
    public bool secondaryIndexTrigger = true;

    [Header("Completion Animation")]
    [Tooltip("How long it takes for the object to shrink to zero.")]
    public float shrinkDuration = 0.5f;

    [Tooltip("Optional delay after the shrinking animation has finished.")]
    public float completionDelay = 0f;

    [Header("After Completion")]
    [Tooltip("Disable the GameObject after completion.")]
    public bool disableObject = true;

    [Tooltip("Destroy the GameObject after completion. Overrides disabling.")]
    public bool destroyObject = false;

    [Tooltip("Delay before disabling/destroying the object.")]
    public float disableDestroyDelay = 0f;

    [Header("Sound")]
    [Tooltip("Sound played when the object is completed.")]
    public AudioClip completionSound;

    [Tooltip("Audio source used to play the completion sound.")]
    public AudioSource audioSource;

    [Tooltip("Volume of the completion sound.")]
    [Range(0f, 1f)]
    public float soundVolume = 1f;

    [Header("Points / Completion Information")]
    [Tooltip("Optional number of points associated with this object.")]
    public int points = 1;

    [Tooltip("Called when the object is successfully completed.")]
    public UnityEvent onComplete;

    [Tooltip("Called when the object becomes disabled.")]
    public UnityEvent onDisabled;

    [Tooltip("Sends the point value to another script.")]
    public UnityEvent<int> onPointsAwarded;

    [Header("Debug")]
    [SerializeField]
    private CompletionState currentState = CompletionState.Available;

    private Vector3 originalScale;
    private bool isHovered = false;
    private bool hasBeenCompleted = false;

    /// <summary>
    /// Current state of this object.
    /// </summary>
    public CompletionState CurrentState
    {
        get { return currentState; }
    }

    /// <summary>
    /// Returns true once the object has been completed.
    /// </summary>
    public bool IsComplete
    {
        get { return currentState == CompletionState.Complete || currentState == CompletionState.Disabled; }
    }

    /// <summary>
    /// Returns true once the object has been disabled.
    /// </summary>
    public bool IsDisabled
    {
        get { return currentState == CompletionState.Disabled; }
    }

    /// <summary>
    /// Returns whether the player is currently pointing at the object.
    /// </summary>
    public bool IsHovered
    {
        get { return isHovered; }
    }

    private void Start()
    {
        originalScale = transform.localScale;

        // Automatically find the main camera for PC controls.
        if (enablePCControls && Camera.main != null)
        {
            // Nothing required here.
        }
    }

    private void Update()
    {
        if (hasBeenCompleted)
            return;

        CheckVRHover();

        if (isHovered)
        {
            CheckQuestInput();
            CheckPCInput();
        }
    }

    // ---------------------------------------------------------
    // VR HOVER / POINTING
    // ---------------------------------------------------------

    private void CheckVRHover()
    {
        isHovered = false;

        // Check primary controller pointer.
        if (primaryPointer != null)
        {
            if (PointerHitsObject(primaryPointer))
            {
                isHovered = true;
                return;
            }
        }

        // Check secondary controller pointer.
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
            // Check if the ray hit this object or one of its children.
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    // ---------------------------------------------------------
    // QUEST 3 INPUT
    // ---------------------------------------------------------

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
            CompleteObject();
        }
    }

    // ---------------------------------------------------------
    // PC INPUT
    // ---------------------------------------------------------

    private void CheckPCInput()
    {
        if (!enablePCControls)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CompleteObject();
        }
    }

    // ---------------------------------------------------------
    // COMPLETION
    // ---------------------------------------------------------

    public void CompleteObject()
    {
        if (hasBeenCompleted)
            return;

        hasBeenCompleted = true;
        currentState = CompletionState.Completing;

        // Play sound immediately when activated.
        PlayCompletionSound();

        // Notify other scripts immediately.
        if (onComplete != null)
        {
            onComplete.Invoke();
        }

        // Award points / send information to another script.
        if (onPointsAwarded != null)
        {
            onPointsAwarded.Invoke(points);
        }

        StartCoroutine(CompleteAnimation());
    }

    // ---------------------------------------------------------
    // SHRINK ANIMATION
    // ---------------------------------------------------------

    private IEnumerator CompleteAnimation()
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

        currentState = CompletionState.Complete;

        // Optional delay after the shrinking animation.
        if (completionDelay > 0f)
        {
            yield return new WaitForSeconds(completionDelay);
        }

        // Additional delay before disabling/destroying.
        if (disableDestroyDelay > 0f)
        {
            yield return new WaitForSeconds(disableDestroyDelay);
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

    // ---------------------------------------------------------
    // DISABLE
    // ---------------------------------------------------------

    private void DisableObject()
    {
        currentState = CompletionState.Disabled;

        if (onDisabled != null)
        {
            onDisabled.Invoke();
        }

        gameObject.SetActive(false);
    }

    // ---------------------------------------------------------
    // SOUND
    // ---------------------------------------------------------

    private void PlayCompletionSound()
    {
        if (completionSound == null)
            return;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(
                completionSound,
                soundVolume
            );
        }
        else
        {
            AudioSource.PlayClipAtPoint(
                completionSound,
                transform.position,
                soundVolume
            );
        }
    }

    // ---------------------------------------------------------
    // MANUAL CONTROL
    // ---------------------------------------------------------

    /// <summary>
    /// Allows another script to manually complete this object.
    /// </summary>
    public void ForceComplete()
    {
        CompleteObject();
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

        currentState = CompletionState.Available;
        hasBeenCompleted = false;
        isHovered = false;

        gameObject.SetActive(true);
    }
}