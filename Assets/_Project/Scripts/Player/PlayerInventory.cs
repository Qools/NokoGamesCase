using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform _storage;

    private List<Item> _inventory = new List<Item>();

    [SerializeField] private int _maxItemStorage;
    private int _currentStorage;

    [SerializeField] private float _timeToCollect;
    [SerializeField] private float _timeToDeliver;
    private float _timer;

    [SerializeField] private Vector3 _offset;
    private Vector3 _lastItemPosition;

    [SerializeField] private bool _isPlayer;

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

            case PlayerPrefKeys.TranformerInput:
                if(other.TryGetComponent(out TransformerInputStorage transformetInputStorage))
                {
                    _onEnterTransformerInput(transformetInputStorage);
                }
            break;

            case PlayerPrefKeys.TransformerStorage:
                if (other.TryGetComponent(out TransformerStorage tranformerStorage))
                {
                    _onEnterTransformerZone(tranformerStorage);
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

    private void _getItem(Item item)
    {
        if (_currentStorage == 0)
        {
            _lastItemPosition = Vector3.zero;
        }

        item.transform.SetParent(_storage);

        item.transform.localPosition = Vector3.zero;
        item.transform.localPosition = _lastItemPosition + _offset;
        item.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _lastItemPosition = item.transform.localPosition;

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

        else
        {
            if (!_isPlayer)
                EventSystem.CallInvetoryFull();
        }

        return value;
    }

    private bool _isEmpty()
    {
        bool value = false;

        if (_currentStorage == 0)
        {
            value = true;

            if(!_isPlayer)
                EventSystem.CallInventoryEmpty();
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

        if (storage.IsEmpty())
        {
            return;
        }

        if (_checkMaxItemCount())
        {
            _getItem(storage.GetItem());

            _timer = 0f;
        }
    }

    private void _onEnterGarbageZone()
    {
        _timer += Time.fixedDeltaTime;
        if (_isEmpty())
        {
            return;
        }

        if (_timer >= _timeToDeliver)
        {
            _remoteItem();

            _timer = 0f;

            return;
        }
    }

    private void _onEnterTransformerInput(TransformerInputStorage transformerInputStorage)
    {
        _timer += Time.fixedDeltaTime;
        if (_isEmpty())
        {
            return;
        }

        if (_timer >= _timeToDeliver)
        {
            DepositeItem(transformerInputStorage);
            _timer = 0f;

            return;
        }
    }

    private void _onEnterTransformerZone(TransformerStorage storage)
    {
        _timer += Time.fixedDeltaTime;
        if (_timer < _timeToCollect)
        {
            return;
        }

        if (storage.IsEmpty())
        {
            return;
        }

        if (_checkMaxItemCount())
        {
            _getItem(storage.GetItem());

            _timer = 0f;
        }
    }

    private void _remoteItem()
    {
        Item item = _inventory[_checkInventory()];
        _inventory.RemoveAt(_checkInventory());
        
        _currentStorage--;

        Destroy(item.gameObject);
    }

    public void DepositeItem(TransformerInputStorage transformerInputStorage)
    {
        if (_isEmpty())
        {
            return;
        }

        Item itemToDeliver = _inventory[_checkInventory()];

        if (itemToDeliver.Type == transformerInputStorage.requiredItem)
        {
            _inventory.Remove(itemToDeliver);
            transformerInputStorage.SetItemPosition(itemToDeliver);
            _currentStorage--;
        }

        else
        {
            return;
        }
    }

    private int _checkInventory()
    {

        int id = 0;

        if (_inventory.Count - 1 > 0)
        {
            id = _inventory.Count - 1;
        }

        return id;
    }
}
