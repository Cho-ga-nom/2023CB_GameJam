using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCtrl : CtrlParent, I_PoolChild
{
    public Vector3 spawnPivot;
    public bool isInit;

    public Action<PoolCtrl> disableAction;
    public override void initAwakeChild()
    {

    }

    private void OnEnable()
    {
        if (isInit == false)
        {
            setSpawnPivot(this.transform.position);
        }
    }
    public void disalble()
    {
        //풀링 회수
        disableAction?.Invoke(this);
    }

    public void setSpawnPivot(Vector3 position)
    {
        isInit = true;
        spawnPivot = position;
        this.transform.position = position;
    }

    public void setDisableAction(Action<I_PoolChild> newDisableAction)
    {
        disableAction = newDisableAction;
    }
    public Transform getTran()
    {
        return this.transform;
    }
}
