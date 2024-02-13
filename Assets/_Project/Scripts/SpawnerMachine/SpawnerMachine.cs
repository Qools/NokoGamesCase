using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SpawnerMachine : MonoBehaviour
{
    [SerializeField] private Animation _spawnMachineAnimation;
    [SerializeField] private Transform _spawnStorage;
    [SerializeField] private SpawnerMachineStorage _machineStorage;

    [SerializeField] private GameObject _spawnItemPrefab;

    [SerializeField] private float _maxSpawnItemCapacity;
    private int _spawnedItemCount;
    [SerializeField] private float _spawnTime;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
       _spawnedItemCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        EventSystem.OnStartGame += _onGameStart;

        EventSystem.OnItemTakenFromSpawnerStorage += _onItemTakenFromStorage;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= _onGameStart;

        EventSystem.OnItemTakenFromSpawnerStorage -= _onItemTakenFromStorage;
    }

    private void _onGameStart()
    {
        InvokeRepeating(nameof(_spawnItem), _spawnTime, _spawnTime);
    }

    [Button]
    private void _playAnim()
    {
        _spawnMachineAnimation.Play();
    }

    private void _spawnItem()
    {
        _checkSpawnedCount();

        GameObject spawned = Instantiate(_spawnItemPrefab, _spawnStorage.position, Quaternion.identity, _spawnStorage);

        _machineStorage.SetItemPosition(spawned, _spawnedItemCount);

        _playAnim();

        _spawnedItemCount++;
    }

    private void _checkSpawnedCount()
    {
        if (_spawnedItemCount == _maxSpawnItemCapacity)
        {
            CancelInvoke();
        }

        else
        {
            InvokeRepeating(nameof(_spawnItem), _spawnTime, _spawnTime);
        }
    } 

    private void _onItemTakenFromStorage()
    {
        _spawnedItemCount--;

        _checkSpawnedCount();
    }
}
