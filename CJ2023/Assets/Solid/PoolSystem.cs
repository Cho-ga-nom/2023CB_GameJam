using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolSystem
{
    private Queue<I_PoolChild> pool = new Queue<I_PoolChild>();

    public I_PoolChild prefab;
    public System.Func<I_PoolChild> createItem;

    public int activeCount;
    public int creatCount;

    public I_PoolChild newItem()
    {
        ++activeCount;
        if (pool.Count > 0)
        {
            I_PoolChild item = pool.Dequeue();
            return item;
        }
        ++creatCount;
        I_PoolChild newPoolCtrl = createItem();// GameObject.Instantiate(prefab, parent);
        newPoolCtrl.setDisableAction(collectItem);
        return newPoolCtrl;
    }
    public void collectItem(I_PoolChild item)
    {
        if (pool.Contains(item) == false)
        {
            --activeCount;
            pool.Enqueue(item);
        }
    }
}
public interface I_PoolChild
{
    public void setDisableAction(System.Action<I_PoolChild> newDisableAction);
    public Transform getTran();
}
