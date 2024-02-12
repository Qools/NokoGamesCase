using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Joystick : MonoBehaviour
{
    public static Joystick current;

    [SerializeField] private CanvasGroup _joystickCanvasGroup;

    public RectTransform center;
    public RectTransform knob;
    public float range;
    public bool fixedJoystick;
    public bool visibleJoystick;

    [SerializeField]
    private Vector2 direction;


    public float knobDistance;

    Vector2 start;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        ShowHide(false);

        _joystickCanvasGroup.alpha = 0f;
        _joystickCanvasGroup.blocksRaycasts = false;
        _joystickCanvasGroup.interactable = false;

    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += _enableJoystick;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= _enableJoystick;
    }

    void Update()
    {
        if (!GameManager.Instance.isGameStarted)
            return;

        Vector2 pos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (visibleJoystick)
            {
                ShowHide(true);
            }
            start = pos;

            knob.position = pos;
            center.position = pos;
        }
        else if (Input.GetMouseButton(0))
        {
            knob.position = pos;
            knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range);

            if (knob.position != Input.mousePosition && !fixedJoystick)
            {
                Vector3 outsideBoundsVector = Input.mousePosition - knob.position;
                center.position += outsideBoundsVector;
            }

            knobDistance = Vector3.Distance(knob.position, center.position) / (center.sizeDelta.x * range);
            direction = (knob.position - center.position).normalized;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ShowHide(false);
            direction = Vector2.zero;

            EventSystem.CallJoystickButtonUp();
        }
    }

    public float GetAxis(string _direction)
    {
        switch (_direction)
        {
            case PlayerPrefKeys.Horizontal: return direction.x;
            case PlayerPrefKeys.Vertical: return direction.y;
            default: return 0;
        }
    }

    public float GetAxisRaw(string _direction)
    {
        switch (_direction)
        {
            case PlayerPrefKeys.Horizontal:
                if (direction.x > 0.0f) return 1f;
                if (direction.x < 0.0f) return -1f;
                return 0;
            case PlayerPrefKeys.Vertical:
                if (direction.y > 0.0f) return 1f;
                if (direction.y < 0.0f) return -1f;
                return 0;
            default: return 0;
        }
    }

    void ShowHide(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);
    }

    private void _enableJoystick()
    {
        _joystickCanvasGroup.alpha = 1f;
        _joystickCanvasGroup.blocksRaycasts = true;
        _joystickCanvasGroup.interactable = true;
    }
}


