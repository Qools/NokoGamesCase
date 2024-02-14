using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerStorage : BaseStorage
{
    public override void SetItemPosition(Item item)
    {        
        base.SetItemPosition(item);

        item.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
    }

    public override Item GetItem()
    {
        base.GetItem();

        EventSystem.CallItemTakenFromTransformerStorage();

        return base._item;
    }
}
