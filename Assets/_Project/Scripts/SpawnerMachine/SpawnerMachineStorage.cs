using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMachineStorage : BaseStorage
{
    public override Item GetItem()
    {
        base.GetItem();

        EventSystem.CallItemTakenFromSpawnerStorage();

        return base._item;
    }
}