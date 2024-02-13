using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform _storage;

    [SerializeField] private int _maxItemStorage;
    private int _currentStorage;

    [SerializeField] private float _timeToCollect;
    private float _timer;

    [SerializeField] private Vector3 _offset;
    private Vector3 _lastItemPosition;

    private void Start()
    {
        _lastItemPosition = Vector3.zero;

        _timer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerPrefKeys.ItemSpawner))
        {
            if (other.TryGetComponent(out SpawnerMachineStorage storage))
            {
                _timer += Time.fixedDeltaTime;
                if (_timer < _timeToCollect)
                {
                    return;
                }

                if (!storage.IsEmpty())
                {
                    return;
                }

                if (_checkItemCount())
                {
                    _getItem(storage.GetItem());

                    _timer = 0f;
                }

                else
                {
                    _timer = 0f;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerPrefKeys.ItemSpawner))
        {
            _timer = 0f;
        }
    }

    private void _getItem(Transform item)
    {
        if (_currentStorage == 0)
        {
            _lastItemPosition = Vector3.zero;
        }

        item.SetParent(_storage);

        item.localPosition = Vector3.zero;
        item.localPosition = _lastItemPosition + _offset;
        item.localRotation = Quaternion.Euler(Vector3.zero);

        _lastItemPosition = item.localPosition;

        _currentStorage++;
    }

    private bool _checkItemCount()
    {
        bool value = false;

        if (_currentStorage < _maxItemStorage)
        {
            value = true;
        }

        return value;
    }
}
