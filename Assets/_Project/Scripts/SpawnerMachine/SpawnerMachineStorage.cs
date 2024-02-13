using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMachineStorage : MonoBehaviour
{
    [SerializeField] private List<Transform> _storages = new List<Transform>();
    [SerializeField] private List<Transform> _items = new List<Transform>();
    [SerializeField] private Vector3 _offset;
    private Vector3 _storagePos;

    // Start is called before the first frame update
    void Start()
    {
        _storagePos = _storages[0].position;
    }

    public void SetItemPosition(GameObject item, int count)
    {
        int reminder = count % _storages.Count;

        if (reminder == 0)
        {
            _offset += new Vector3(0f, 0.2f + 0f);
        }

        _storagePos = _storages[reminder].position + _offset;

        item.transform.SetParent(_storages[reminder]);
        item.transform.position = _storagePos;

        _items.Add(item.transform);
    }

    public Transform GetItem()
    {
        int id = 0;

        if (_items.Count - 1 >= 0)
        {
            id = _items.Count - 1;
        }

        Transform item = _items[id];

        _items.RemoveAt(id);

        _offset -= new Vector3(0f, 0.2f + 0f);

        EventSystem.CallItemTakenFromSpawnerStorage();

        return item;
    }

    public bool IsEmpty()
    {
        bool value = true;

        if (_items.Count == 0)
        {
            value = false;
        }

        return value;
    }
}