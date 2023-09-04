using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetCtrl : CtrlParent
{
    public CharacterData charaData;
    public bool isArrive;
    public Transform nowTarget;
    public PathInfo pathInfo = new PathInfo();
    public virtual float rangeDis => 0.2f;
    public override void initAwakeChild()
    {
    }
    public override void initStart()
    {
        rootCtrl.hpCtrl.maxHp = charaData.MaxHP;
        rootCtrl.moveCtrl.moveSpeed = charaData.Speed;
        rootCtrl.stateCtrl.stunTime = charaData.StunTime;
        rootCtrl.stateCtrl.ropeTime = charaData.TieTime;
        base.initStart();
    }

    public abstract Transform getTarget();
    public abstract bool checkTargetState();

    public bool checkTargetDis(float dis)
    {
        if (rangeDis > dis)
        {
            arriveTarget();
            return true;
        }
        return false;
    }
    public virtual void arriveTarget()
    {

    }
    public virtual void targetUpdate()
    {

    }
}
