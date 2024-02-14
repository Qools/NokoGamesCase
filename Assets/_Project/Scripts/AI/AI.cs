using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private Transform _spawnerTransform;
    [SerializeField] private Transform _transformerTransform;

    [SerializeField] private Transform character;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    private Vector3 direction;

    public float rigidbodyVelocity;

    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStarted)
            return;

        Rotate();
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += OnGameStarted;

        EventSystem.OnInvetoryEmpty += _onInvetoryEmpty;
        EventSystem.OnInventoryFull += _onInventoryFull;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;

        EventSystem.OnInvetoryEmpty -= _onInvetoryEmpty;
        EventSystem.OnInventoryFull -= _onInventoryFull;
    }

    private void OnGameStarted()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = false;

        _moveToTarget(_spawnerTransform);
    }

    public void Rotate()
    {
        character.rotation = Quaternion.RotateTowards(
            character.rotation,
            Quaternion.LookRotation(direction),
            _rotationSpeed * Time.fixedDeltaTime);
    }

    private void _updateAnimator(string triggerName)
    {
        _animator.ResetTrigger(triggerName);
        _animator.SetTrigger(triggerName);
    }

    private void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _updateAnimator(PlayerPrefKeys.Stop);
    }

    private void _moveToTarget(Transform target)
    {
        Vector3 distance = target.position - this.transform.position;

        float duration = distance.magnitude / _movementSpeed;

        direction = distance;

        _updateAnimator(PlayerPrefKeys.Walk);

        _rigidbody.DOMove(new Vector3(target.position.x, this.transform.position.y, target.position.z), duration).OnComplete(() => Stop());
    }

    private void _onInventoryFull()
    {
        _moveToTarget(_transformerTransform);
    }

    private void _onInvetoryEmpty()
    {
        _moveToTarget(_spawnerTransform);
    }
}
