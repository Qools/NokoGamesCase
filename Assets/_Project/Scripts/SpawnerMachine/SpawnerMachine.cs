using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SpawnerMachine : BaseSpawner
{
    [SerializeField] private SpawnerMachineStorage _machineStorage;

    protected override void OnEnable()
    {
        base.OnEnable();

        EventSystem.OnItemTakenFromSpawnerStorage += _onItemTakenFromStorage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventSystem.OnItemTakenFromSpawnerStorage -= _onItemTakenFromStorage;
    }
}
