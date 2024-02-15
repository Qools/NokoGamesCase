using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Sheep : MonoBehaviour
{
    [SerializeField] private float _maxXValue, _maxZValue;
    [SerializeField] private float _minXValue, _minZValue;

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
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = false;

        _moveToTarget(_setRandomPosition());
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

        _moveToTarget(_setRandomPosition());
    }

    private void _moveToTarget(Vector3 target)
    {
        Vector3 distance = target - this.transform.position;

        float duration = distance.magnitude / _movementSpeed;

        direction = distance;

        _updateAnimator(PlayerPrefKeys.Walk);

        _rigidbody.DOMove(new Vector3(target.x, this.transform.position.y, target.z), duration).OnComplete(() => Stop());
    }

    private Vector3 _setRandomPosition()
    {
        return new Vector3(Random.Range(_minXValue, _maxXValue), this.transform.position.y, Random.Range(_minZValue, _maxZValue));
    }
}
