using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    [SerializeField] private Animation _spawnMachineAnimation;
    [SerializeField] private Transform _spawnStorage;
    [SerializeField] private TransformerStorage _transformerStorage;
    [SerializeField] private TransformerInputStorage _transformerInputStorage;

    [SerializeField] private GameObject _spawnItemPrefab;

    [SerializeField] private float _maxSpawnItemCapacity;
    private int _spawnedItemCount;
    [SerializeField] private float _spawnTime;
    private float _timer;

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

        EventSystem.OnItemTakenFromTransformerStorage += _onItemTakenFromStorage;
    }

    private void OnDisable()
    {
        EventSystem.OnStartGame -= _onGameStart;

        EventSystem.OnItemTakenFromTransformerStorage += _onItemTakenFromStorage;
    }

    private void _onGameStart()
    {
        InvokeRepeating(nameof(_getRequiredItem), 0f, _spawnTime);

        //DOVirtual.DelayedCall(_spawnTime, () => this._getRequiredItem());
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

        _transformerStorage.SetItemPosition(spawned.GetComponent<Item>(), _spawnedItemCount);

        _playAnim();

        _spawnedItemCount++;
    }

    private void _getRequiredItem()
    {
        if (_transformerInputStorage.IsEmpty())
        {
            return;
        }

        _transformerInputStorage.GetItem();

        Invoke(nameof(_spawnItem), _spawnTime);

        //DOVirtual.DelayedCall(_spawnTime, () => this._spawnItem());
    }

    private void _checkSpawnedCount()
    {
        if (_spawnedItemCount == _maxSpawnItemCapacity)
        {
            CancelInvoke();
        }

        else
        {
            InvokeRepeating(nameof(_getRequiredItem), 0f, _spawnTime);

            //DOVirtual.DelayedCall(_spawnTime, () => this._getRequiredItem());
        }
    }

    private void _onItemTakenFromStorage()
    {
        _spawnedItemCount--;

        _checkSpawnedCount();
    }
}
