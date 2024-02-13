using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform _storage;

    private List<Transform> _inventory = new List<Transform>();

    [SerializeField] private int _maxItemStorage;
    private int _currentStorage;

    [SerializeField] private float _timeToCollect;
    [SerializeField] private float _timeToDeliver;
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
        switch (other.tag)
        {
            case PlayerPrefKeys.GarbageZone:
                _onEnterGarbageZone();
            break;

            case PlayerPrefKeys.ItemSpawner:
                if (other.TryGetComponent(out SpawnerMachineStorage storage))
                {
                    _onEnterSpawnerZone(storage);
                }
            break;
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

        _inventory.Add(item);

        _currentStorage++;
    }

    private bool _checkMaxItemCount()
    {
        bool value = false;

        if (_currentStorage < _maxItemStorage)
        {
            value = true;
        }

        return value;
    }

    private bool _isEmpty()
    {
        bool value = false;

        if (_currentStorage == 0)
        {
            value = true;
        }

        return value;
    }

    private void _onEnterSpawnerZone(SpawnerMachineStorage storage)
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

        if (_checkMaxItemCount())
        {
            _getItem(storage.GetItem());

            _timer = 0f;
        }

        else
        {
            _timer = 0f;
        }
    }

    private void _onEnterGarbageZone()
    {
        _timer += Time.fixedDeltaTime;
        if (_isEmpty())
        {
            _timer = 0f;

            return;
        }

        if (_timer >= _timeToDeliver)
        {
            _remoteItem();

            _timer = 0f;

            return;
        }
    }

    private void _remoteItem()
    {
        int id = 0;

        if (_inventory.Count - 1 >= 0)
        {
            id = _inventory.Count - 1;
        }

        Transform item = _inventory[id];
        _inventory.RemoveAt(id);
        
        _currentStorage--;

        Destroy(item.gameObject);
    }
}
