using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : BaseSpawner
{
    [SerializeField] private BaseStorage _transformerStorage;
    [SerializeField] private BaseStorage _transformerInputStorage;

    protected override void OnEnable()
    {
        base.OnEnable();

        EventSystem.OnItemTakenFromTransformerStorage += _onItemTakenFromStorage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventSystem.OnItemTakenFromTransformerStorage -= _onItemTakenFromStorage;
    }

    protected override void _onGameStart()
    {
        InvokeRepeating(nameof(_getRequiredItem), _spawnTime, _spawnTime);
    }

    private void _getRequiredItem()
    {
        if (_transformerInputStorage.IsEmpty())
        {
            return;
        }

        if (_transformerStorage.IsFull())
        {
            return;
        }

        Destroy( _transformerInputStorage.GetItem().gameObject );

        Invoke(nameof(_spawnItem), _spawnTime);
    }
}
