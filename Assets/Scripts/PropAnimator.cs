using Oculus.Interaction;
using UnityEngine;

// Simple looping animation (spin, bob, sway, pulse) for floating props.
// Pauses while the prop is held and continues from wherever it is released.
// Global controls (speed, pause, size) are driven by the VR menu via PropController.
[RequireComponent(typeof(Grabbable))]
public class PropAnimator : MonoBehaviour
{
    // Global controls
    public static float SpeedMultiplier = 1f;
    public static float ScaleMultiplier = 1f;
    public static bool Paused;

    public Vector3 spinDegPerSec = new Vector3(0, 45, 0);
    public float bobAmplitude = 0.1f;
    public float bobFrequency = 1f;
    public float swayAmplitude = 0f;
    public float swayFrequency = 1f;
    public Vector3 swayAxis = Vector3.right;
    public float pulseAmplitude = 0f;
    public float pulseFrequency = 1f;

    Grabbable grabbable;
    Rigidbody rb;
    Vector3 anchor;
    Vector3 baseScale;
    Vector3 startPos;
    Quaternion startRot;
    float animTime;
    bool held;
    bool dropped;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        rb = GetComponent<Rigidbody>();
        anchor = startPos = transform.position;
        startRot = transform.rotation;
        baseScale = transform.localScale;
        animTime = Random.value * 10f;
    }

    void OnEnable() => grabbable.WhenPointerEventRaised += OnPointerEvent;
    void OnDisable() => grabbable.WhenPointerEventRaised -= OnPointerEvent;

    Vector3 Offset(float t)
    {
        return Vector3.up * (Mathf.Sin(t * bobFrequency) * bobAmplitude)
             + swayAxis.normalized * (Mathf.Sin(t * swayFrequency) * swayAmplitude);
    }

    void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            held = true;
        }
        else if ((evt.Type == PointerEventType.Unselect || evt.Type == PointerEventType.Cancel)
                 && grabbable.SelectingPointsCount == 0)
        {
            held = false;
            // Re-anchor so the animation resumes from here without a jump.
            anchor = transform.position - Offset(animTime);
        }
    }

    // ---- called by the menu ----

    /// Let gravity take over so the prop falls and can be thrown around.
    public void Drop()
    {
        dropped = true;
        if (rb == null) return;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    /// Put the prop back into its floating animation where it currently is.
    public void Float()
    {
        dropped = false;
        if (rb != null)
        {
            if (!rb.isKinematic) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        anchor = transform.position - Offset(animTime);
    }

    /// Return to the original spawn position and rotation, floating again.
    public void ResetToStart()
    {
        held = false;
        Float();
        transform.SetPositionAndRotation(startPos, startRot);
        anchor = startPos;
    }

    void Update()
    {
        if (held || dropped || Paused) return;
        animTime += Time.deltaTime * SpeedMultiplier;
        transform.position = anchor + Offset(animTime);
        transform.Rotate(spinDegPerSec * (Time.deltaTime * SpeedMultiplier), Space.Self);
        float pulse = pulseAmplitude > 0f ? 1f + Mathf.Sin(animTime * pulseFrequency) * pulseAmplitude : 1f;
        transform.localScale = baseScale * (ScaleMultiplier * pulse);
    }
}
