using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] protected Animation _spawnMachineAnimation;
    [SerializeField] protected Transform _spawnStorage;
    [SerializeField] protected BaseStorage _storage;

    [SerializeField] protected GameObject _spawnItemPrefab;

    protected int _spawnedItemCount;
    [SerializeField] protected float _spawnTime;
    protected virtual void  Start()
    {
        _spawnedItemCount = 0;
    }

    protected virtual void OnEnable()
    {
        EventSystem.OnStartGame += _onGameStart;
    }

    protected virtual void OnDisable()
    {
        EventSystem.OnStartGame -= _onGameStart;
    }

    protected virtual void _onGameStart()
    {
        InvokeRepeating(nameof(_spawnItem), _spawnTime, _spawnTime);
    }

    [Button]
    protected void _playAnim()
    {
        _spawnMachineAnimation.Play();
    }

    protected virtual void _spawnItem()
    {
        if (_storage.IsFull())
        {
            return;
        }

        GameObject spawned = Instantiate(_spawnItemPrefab, _spawnStorage.position, Quaternion.identity, _spawnStorage);

        _storage.SetItemPosition(spawned.GetComponent<Item>());

        _playAnim();

        _spawnedItemCount++;
    }

    protected virtual void _onItemTakenFromStorage()
    {
        _spawnedItemCount--;
    }
}
