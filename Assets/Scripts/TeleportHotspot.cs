using UnityEngine;

/// <summary>
/// Attach this to each teleport hotspot in the scene.
/// Handles its own highlight visuals and active/inactive state.
/// TeleportManager and TeleportPointer both talk to this component;
/// it doesn't reach out to them.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TeleportHotspot : MonoBehaviour
{
    [Header("Visuals")]
    [Tooltip("Particle effect shown when the hotspot is idle (not being pointed at).")]
    [SerializeField] private ParticleSystem idleEffect;

    [Tooltip("Particle effect shown when a controller is pointing at this hotspot.")]
    [SerializeField] private ParticleSystem highlightEffect;

    [Header("Teleport")]
    [Tooltip("Where the player will be moved to. If left empty, this object's own transform is used.")]
    [SerializeField] private Transform teleportPoint;

    [Tooltip("Sound played when the player teleports to this hotspot. If left empty, TeleportManager's default sound is used instead.")]
    [SerializeField] private AudioClip teleportSound;

    private Collider hotspotCollider;
    private bool isHighlighted;

    /// <summary>False while the player is currently standing on this hotspot.</summary>
    public bool IsActive { get; private set; } = true;

    public Transform TeleportPoint => teleportPoint != null ? teleportPoint : transform;
    public AudioClip TeleportSound => teleportSound;

    private void Awake()
    {
        hotspotCollider = GetComponent<Collider>();

        // Start idle, not highlighted.
        if (idleEffect != null) idleEffect.gameObject.SetActive(true);
        if (highlightEffect != null) highlightEffect.gameObject.SetActive(false);
    }

    /// <summary>Called by TeleportPointer every frame, based on whether it's currently aimed at this hotspot.</summary>
    public void SetHighlighted(bool highlighted)
    {
        if (isHighlighted == highlighted) return;
        isHighlighted = highlighted;

        if (idleEffect != null) idleEffect.gameObject.SetActive(!highlighted);
        if (highlightEffect != null) highlightEffect.gameObject.SetActive(highlighted);
    }

    /// <summary>Makes this hotspot targetable again (player is no longer standing on it).</summary>
    public void Activate()
    {
        IsActive = true;
        if (hotspotCollider != null) hotspotCollider.enabled = true;

        isHighlighted = false;
        if (idleEffect != null) idleEffect.gameObject.SetActive(true);
        if (highlightEffect != null) highlightEffect.gameObject.SetActive(false);
    }

    /// <summary>Makes this hotspot un-targetable (player is currently standing on it).</summary>
    public void Deactivate()
    {
        IsActive = false;
        isHighlighted = false;

        // Disabling the collider is what actually stops raycasts from hitting it,
        // so pointers can't highlight or teleport to a hotspot the player is on.
        if (hotspotCollider != null) hotspotCollider.enabled = false;

        if (idleEffect != null) idleEffect.gameObject.SetActive(false);
        if (highlightEffect != null) highlightEffect.gameObject.SetActive(false);
    }
}