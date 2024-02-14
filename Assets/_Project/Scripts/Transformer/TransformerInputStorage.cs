using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerInputStorage : BaseStorage
{
    public ItemType requiredItem;

    public override void SetItemPosition(Item item)
    {
        base.SetItemPosition(item);

        item.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
    }
}
