using UnityEngine;
using UnityEngine.UI;

// Drives the VR prop menu: wires up the UI and applies changes to every PropAnimator.
// Press the left controller X button (or left hand menu gesture is not used) to show/hide the menu.
public class PropController : MonoBehaviour
{
    [Header("Menu")]
    public GameObject menuRoot;        // the world-space menu (shown/hidden)
    public Transform head;             // used to place the menu in front of you
    public float menuDistance = 1.3f;

    [Header("UI")]
    public Button pauseButton;
    public Text pauseLabel;
    public Slider speedSlider;
    public Slider sizeSlider;
    public Button colorsButton;
    public Button dropButton;
    public Text dropLabel;
    public Button resetButton;
    public Button closeButton;

    PropAnimator[] props;
    bool dropped;

    static readonly Color[] palette =
    {
        new Color(1f, 0.25f, 0.45f), new Color(1f, 0.65f, 0.1f), new Color(0.3f, 1f, 0.5f),
        new Color(0.2f, 0.8f, 1f), new Color(0.6f, 0.4f, 1f), new Color(1f, 0.95f, 0.3f),
        new Color(1f, 1f, 1f), new Color(1f, 0.4f, 0.1f)
    };

    void Awake()
    {
        // Reset statics so the editor's "no domain reload" play mode starts clean.
        PropAnimator.SpeedMultiplier = 1f;
        PropAnimator.ScaleMultiplier = 1f;
        PropAnimator.Paused = false;

        pauseButton.onClick.AddListener(TogglePause);
        speedSlider.onValueChanged.AddListener(v => PropAnimator.SpeedMultiplier = v);
        sizeSlider.onValueChanged.AddListener(v => PropAnimator.ScaleMultiplier = v);
        colorsButton.onClick.AddListener(RandomizeColors);
        dropButton.onClick.AddListener(ToggleDrop);
        resetButton.onClick.AddListener(ResetAll);
        if (closeButton != null) closeButton.onClick.AddListener(() => ShowMenu(false));
    }

    void Start()
    {
        props = FindObjectsByType<PropAnimator>();
        if (head == null)
        {
            var rig = FindAnyObjectByType<OVRCameraRig>();
            if (rig != null) head = rig.centerEyeAnchor;
        }
    }

    void Update()
    {
        // Left controller X button toggles the menu
        if (OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.LTouch))
            ShowMenu(!menuRoot.activeSelf);
    }

    public void ShowMenu(bool show)
    {
        if (show && head != null)
        {
            Vector3 flat = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
            if (flat.sqrMagnitude < 0.01f) flat = Vector3.forward;
            Vector3 pos = head.position + flat * menuDistance;
            pos.y = head.position.y - 0.1f;
            menuRoot.transform.position = pos;
            menuRoot.transform.rotation = Quaternion.LookRotation(flat, Vector3.up);
        }
        menuRoot.SetActive(show);
    }

    void TogglePause()
    {
        PropAnimator.Paused = !PropAnimator.Paused;
        pauseLabel.text = PropAnimator.Paused ? "Resume" : "Pause";
    }

    void RandomizeColors()
    {
        foreach (var p in props)
        {
            if (p == null) continue;
            var r = p.GetComponent<Renderer>();
            if (r == null) continue;
            Color c = palette[Random.Range(0, palette.Length)];
            var m = r.material; // per-object copy so shared materials stay untouched
            m.color = c;
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", c * 0.6f);
        }
    }

    void ToggleDrop()
    {
        dropped = !dropped;
        foreach (var p in props)
        {
            if (p == null) continue;
            if (dropped) p.Drop(); else p.Float();
        }
        dropLabel.text = dropped ? "Float props" : "Drop props";
    }

    void ResetAll()
    {
        dropped = false;
        dropLabel.text = "Drop props";
        foreach (var p in props) if (p != null) p.ResetToStart();
        speedSlider.value = 1f;
        sizeSlider.value = 1f;
        PropAnimator.Paused = false;
        pauseLabel.text = "Pause";
    }
}
