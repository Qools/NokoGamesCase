using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    [SerializeField] protected List<Transform> _storages = new List<Transform>();
    [SerializeField] protected List<Item> _items = new List<Item>();

    [SerializeField] protected Vector3 _offset;
    protected Vector3 _storagePos;
    protected Vector3 _startingOffset;

    protected int _storageIndex;

    protected Item _item;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _storagePos = _storages[0].position;

        _startingOffset = _offset;

        _storageIndex = 0;
    }

    public virtual void SetItemPosition(Item item)
    {
        int reminder = _storageIndex % _storages.Count;

        if (reminder == 0)
        {
            _offset += _startingOffset;

            if (_items.Count < 6)
            {
                _offset = _startingOffset;
            }
        }

        _storagePos = _storages[reminder].position + _offset;

        item.transform.SetParent(_storages[reminder]);
        item.transform.position = _storagePos;
        item.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _storageIndex++;

        _items.Add(item);
    }

    public virtual Item GetItem()
    {
        _item = _items[_checkInventory()];

        _items.Remove(_item);

        EventSystem.CallItemTakenFromSpawnerStorage();

        int reminder = _storageIndex % _storages.Count;

        if (reminder == 0)
        {
            _offset -= _startingOffset;
        }

        _storageIndex--;

        return _item;
    }

    public bool IsEmpty()
    {
        bool value = false;

        if (_items.Count == 0)
        {
            value = true;
        }

        return value;
    }

    protected int _checkInventory()
    {
        int id = 0;

        if (_items.Count - 1 > 0)
        {
            id = _items.Count - 1;
        }

        return id;
    }
}
