using UnityEngine;

/// <summary>
/// Central authority for teleporting the player and keeping hotspot active/inactive
/// state in sync. The hotspot the player is currently standing on is always the
/// deactivated one; teleporting swaps that flag to the new hotspot and re-activates
/// the old one.
/// Put this on a single persistent object in the scene.
/// </summary>
public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance { get; private set; }

    [Header("Player Rig")]
    [Tooltip("Root transform of the VR rig (e.g. OVRCameraRig) that gets moved to teleport.")]
    [SerializeField] private Transform playerRig;

    [Tooltip("Head/camera transform (e.g. CenterEyeAnchor). Used to work out where the player's feet actually are within the play space, so teleports land accurately no matter where they're standing relative to the rig's origin.")]
    [SerializeField] private Transform headTransform;

    [Header("Starting State")]
    [Tooltip("The hotspot the player starts the scene standing on. It's deactivated on Start with no sound or movement.")]
    [SerializeField] private TeleportHotspot startingHotspot;

    [Header("Audio")]
    [Tooltip("Fallback sound used if the target hotspot doesn't specify its own teleport sound.")]
    [SerializeField] private AudioClip defaultTeleportSound;
    [SerializeField] private AudioSource audioSource;

    private TeleportHotspot currentHotspot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (startingHotspot != null)
        {
            currentHotspot = startingHotspot;
            currentHotspot.Deactivate();
        }
    }

    /// <summary>
    /// Call this when the player triggers a teleport to the given hotspot.
    /// Plays the teleport sound, moves the player, and swaps which hotspot is active.
    /// </summary>
    public void TeleportTo(TeleportHotspot target)
    {
        if (target == null || !target.IsActive || target == currentHotspot) return;

        PlayTeleportSound(target);
        MovePlayer(target.TeleportPoint.position);

        if (currentHotspot != null)
        {
            currentHotspot.Activate();
        }

        target.Deactivate();
        currentHotspot = target;
    }

    private void MovePlayer(Vector3 destination)
    {
        if (playerRig == null || headTransform == null)
        {
            Debug.LogWarning("TeleportManager: playerRig or headTransform not assigned.");
            return;
        }

        // Keep the player's physical offset within the play space, but land the
        // *head* exactly on the destination XZ, not the rig's arbitrary origin.
        Vector3 headOffset = headTransform.position - playerRig.position;
        headOffset.y = 0f;

        playerRig.position = destination - headOffset;
    }

    private void PlayTeleportSound(TeleportHotspot target)
    {
        AudioClip clip = target.TeleportSound != null ? target.TeleportSound : defaultTeleportSound;
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}