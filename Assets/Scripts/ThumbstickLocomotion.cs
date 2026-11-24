using UnityEngine;

// Left thumbstick: move relative to where you are looking.
// Right thumbstick: snap turn left/right (comfort option).
public class ThumbstickLocomotion : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public bool enableSnapTurn = false; // the Meta rig already has its own stick turn
    public float snapTurnDegrees = 30f;
    public float deadZone = 0.2f;
    public float turnThreshold = 0.7f;
    public float areaLimit = 14f; // keep the player inside the room walls (±)

    Transform head;
    bool turnReady = true;

    void Start()
    {
        var rig = GetComponentInChildren<OVRCameraRig>();
        if (rig != null) head = rig.centerEyeAnchor;
    }

    void Update()
    {
        if (head == null) return;

        // Move
        Vector2 move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        if (move.magnitude > deadZone)
        {
            Vector3 forward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;
            Vector3 delta = (forward * move.y + right * move.x) * (moveSpeed * Time.deltaTime);
            Vector3 p = transform.position + delta;
            p.x = Mathf.Clamp(p.x, -areaLimit, areaLimit);
            p.z = Mathf.Clamp(p.z, -areaLimit, areaLimit);
            transform.position = p;
        }

        // Snap turn around the head so you don't swing sideways
        float turn = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
        if (enableSnapTurn && turnReady && Mathf.Abs(turn) > turnThreshold)
        {
            transform.RotateAround(head.position, Vector3.up, Mathf.Sign(turn) * snapTurnDegrees);
            turnReady = false;
        }
        else if (Mathf.Abs(turn) < 0.3f)
        {
            turnReady = true;
        }
    }
}
