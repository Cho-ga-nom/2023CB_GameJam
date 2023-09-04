using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarryCtrl : CtrlParent
{
    public InterObj carryObj;
    public Transform carryPivot;
    public override void initAwakeChild()
    {
    }
    public override void initStart()
    {
        carryPivot = rootCtrl.animaterCtrl.pivotCtrl.CarryPivot;
    }
    public void carryItem(InterObj_StatueTable statue)
    {
        statue.isItemOn = false;
        statue.isCarry = true;
        carryObj = statue.interItem;
        carryObj.PivotTran.SetParent(carryPivot);
        carryObj.PivotTran.localPosition = Vector3.zero;
        carryObj.PivotTran.localEulerAngles = Vector3.zero;
        rootCtrl.animaterCtrl.modelAni.SetBool("IsCarry", true);
        rootCtrl.moveCtrl.isRun = false;
    }
    public void carryThief(InterObj newCarryObj)
    {
        carryObj = newCarryObj;
        carryObj.PivotTran.SetParent(carryPivot);
        carryObj.PivotTran.localPosition = Vector3.zero;
        carryObj.PivotTran.localEulerAngles = Vector3.zero;
        rootCtrl.animaterCtrl.modelAni.SetBool("IsCarry", true);
        rootCtrl.moveCtrl.isRun = false;
    }
    public void carryOff(InterObj offObj)
    {
        if (carryObj == offObj)
        {
            rootCtrl.moveCtrl.isRun = true;
            carryObj.PivotTran.SetParent(null);
            //carryObj.DOMove(carryObj.transform.position + (rootCtrl.moveCtrl.isRight ? Vector3.right : Vector3.left), 1f);
            rootCtrl.animaterCtrl.modelAni.SetBool("IsCarry", false);
        }
    }
}
