using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerStorage : MonoBehaviour
{
    [SerializeField] private List<Transform> _storages = new List<Transform>();
    [SerializeField] private List<Item> _items = new List<Item>();
    
    [SerializeField] private Vector3 _offset;
    private Vector3 _startingOffset;
    private Vector3 _storagePos;

    private int _storageIndex;

    // Start is called before the first frame update
    void Start()
    {
        _storagePos = _storages[0].position;

        _startingOffset = _offset;

        _storageIndex = 0;
    }

    public void SetItemPosition(Item item)
    {        
        int reminder = _storageIndex % _storages.Count;

        if (reminder == 0)
        {
            _offset += _startingOffset;
        }

        _storagePos = _storages[reminder].position + _offset;

        item.transform.SetParent(_storages[reminder]);
        item.transform.position = _storagePos;
        item.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

        _storageIndex++;
        _items.Add(item);
    }

    public Item GetItem()
    {
        Item item = _items[_checkInventory()];

        _items.Remove(item);

        int reminder = _storageIndex % _storages.Count;

        if (reminder == 0)
        {
            _offset -= _startingOffset;
        }

        _storageIndex--;

        EventSystem.CallItemTakenFromTransformerStorage();

        return item;
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

    private int _checkInventory()
    {
        int id = 0;

        if (_items.Count - 1 > 0)
        {
            id = _items.Count - 1;
        }

        return id;
    }
}