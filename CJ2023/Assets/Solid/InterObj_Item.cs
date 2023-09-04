using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterObj_Item : InterObj, I_PoolChild
{
    public bool isRope;
    public float moveSpeed = 0.1f;
    public bool isFollow;
    private InterCtrl followCtrl;
    public bool isAdd;
    public override void initAwake()
    {
        isInterLock = true;
    }
    public void Update()
    {
        if (isAdd) return;
        if (isFollow)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, followCtrl.rootCtrl.animaterCtrl.pivotCtrl.Body.position, moveSpeed);
            float dis = Vector2.Distance(this.transform.position, followCtrl.rootCtrl.animaterCtrl.pivotCtrl.Body.position);
            if (dis < 0.2f)
            {
                isAdd = true;
                isFollow = false;
                if (isRope)
                {
                    GameManager.Instence.playerCtrl.playerCtrl.tieAmount++;
                }
                else
                {
                    //GameManager.Instence.playerCtrl.playerCtrl.trapAmount++;
                }
                disableAction?.Invoke(this);
                this.gameObject.SetActive(false);
            }
        }
    }
    public override void interEnable(InterCtrl interCtrl)
    {
        followCtrl = interCtrl;
        isFollow = true;
        base.interEnable(interCtrl);
    }
    public override void interDisable(InterCtrl interCtrl)
    {
        followCtrl = null;
        isFollow = false;
        base.interDisable(interCtrl);
    }
    public override bool shortUse(InterCtrl interCtrl)
    {
        return false;
    }

    public override void useEnd(InterCtrl interCtrl)
    {
    }

    public override bool longUse(InterCtrl interCtrl)
    {
        return false;
    }

    public override void longCancle(InterCtrl interCtrl)
    {
    }

    public Action<I_PoolChild> disableAction;
    public void setDisableAction(Action<I_PoolChild> newDisableAction)
    {
        disableAction = newDisableAction;
    }

    public Transform getTran()
    {
        return this.transform;
    }
}
