using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Joystick _joystick;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    private Vector3 direction;

    public float rigidbodyVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStarted)
            return;

        direction = new Vector3(Joystick.current.GetAxis(PlayerPrefKeys.Horizontal), 0f, Joystick.current.GetAxis(PlayerPrefKeys.Vertical));

        Rotate();

        MoveForward();

        _updateAnimator();
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += OnGameStarted;
        EventSystem.OnGameOver += OnGameOver;

        EventSystem.OnJoystickButtonUp += OnJoystickButtonUp;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;
        EventSystem.OnGameOver -= OnGameOver;

        EventSystem.OnJoystickButtonUp -= OnJoystickButtonUp;
    }

    private void OnGameStarted()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = false;
    }

    private void OnGameOver(GameResult gameResult)
    {
        Stop();
    }

    public void Rotate()
    {
        character.rotation = Quaternion.RotateTowards(
            character.rotation,
            Quaternion.LookRotation(direction),
            _rotationSpeed * Time.fixedDeltaTime);
    }

    public void MoveForward()
    {
        Vector3 newVelocity = Joystick.current.knobDistance * _movementSpeed * Time.fixedDeltaTime * direction;
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;

        rigidbodyVelocity = _rigidbody.velocity.magnitude;
    }

    private void _updateAnimator()
    {
        _animator.SetFloat(PlayerPrefKeys.Speed, rigidbodyVelocity);
    }

    private void OnJoystickButtonUp()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }
}
