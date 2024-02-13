using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerStorage : MonoBehaviour
{
    [SerializeField] private List<Transform> _storages = new List<Transform>();
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private Vector3 _offset;
    private Vector3 _storagePos;

    // Start is called before the first frame update
    void Start()
    {
        _storagePos = _storages[0].position;
    }

    public void SetItemPosition(Item item, int count)
    {
        int reminder = count % _storages.Count;

        if (reminder == 0)
        {
            _offset += new Vector3(0f, 0.2f, 0f);
        }

        _storagePos = _storages[reminder].position + _offset;

        item.transform.SetParent(_storages[reminder]);
        item.transform.position = _storagePos;
        item.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _items.Add(item);
    }

    public Item GetItem()
    {
        Item item = _items[_checkInventory()];

        _items.Remove(item);

        _offset -= new Vector3(0f, 0.2f, 0f);

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
