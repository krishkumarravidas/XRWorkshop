using Oculus.Interaction;
using UnityEngine;

// Stops SineWaveMovement while the object is held (by controller or hand),
// and re-anchors it where it is released.
[RequireComponent(typeof(Grabbable))]
public class GrabPausesMovement : MonoBehaviour
{
    [SerializeField] SineWaveMovement movement;
    Grabbable grabbable;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        if (movement == null) movement = GetComponent<SineWaveMovement>();
    }

    void OnEnable() => grabbable.WhenPointerEventRaised += OnPointerEvent;
    void OnDisable() => grabbable.WhenPointerEventRaised -= OnPointerEvent;

    void OnPointerEvent(PointerEvent evt)
    {
        if (movement == null) return;
        if (evt.Type == PointerEventType.Select)
        {
            movement.enabled = false;
        }
        else if ((evt.Type == PointerEventType.Unselect || evt.Type == PointerEventType.Cancel)
                 && grabbable.SelectingPointsCount == 0)
        {
            movement.ResetOrigin();
            movement.enabled = true;
        }
    }
}
