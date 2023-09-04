using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterObj_StatueTable : InterObj
{

    public override void initAwake()
    {
        myKind = InterKind.StatueTable;
        if (GameManager.Instence.statueList.Contains(this) == false)
        {
            GameManager.Instence.statueList.Add(this);
        }
        if (interItem == null)
        {
            interItem = this.GetComponentInChildren<InterObj_Statue>();
        }
        interItem.isInterLock = true;
        interItem.statueTable = this;
    }

    public InterObj_Statue interItem;

    public bool isItemOn = true;//전시중
    public bool isTheft = false;
    public bool isCarry = false;
    public bool isTargetOn => isCarry == false && isTheft == false;

    public override void setInterChild(InterCtrl interCtrl, InterObj interObj)
    {
        if (interObj == interItem)
        {
            isItemOn = true;
            //isCarry = false;
            interCtrl.rootCtrl.carryCtrl.carryOff(interObj);
            interObj.isInterLock = true;
            interObj.transform.SetParent(this.transform);
            interObj.transform.localPosition = Vector3.zero;
            interObj.transform.localEulerAngles = Vector3.zero;
        }
    }

    public override bool shortUse(InterCtrl interCtrl)
    {
        if (isItemOn)
        {
            interItem.isInterLock = false;
            interItem.shortUse(interCtrl);
            return true;
        }
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

    //Todo 잠금 장치작업해야함
}
