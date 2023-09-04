using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterObj_Prison : InterObj
{

    public override void initAwake()
    {
        myKind = InterKind.Prison;

    }



    public override void setInterChild(InterCtrl interCtrl, InterObj interObj)
    {
        if (interObj.myKind == InterKind.Thief)
        {
            //°¨¿ÁÇà
            //isCarry = false;
            InterObj_Thief thief = interObj as InterObj_Thief;
            if (thief.rootCtrl.stateCtrl.nowState == StateKind.Rope)
            {
                interCtrl.rootCtrl.playerCtrl.tieAmount++;
            }
            interCtrl.rootCtrl.carryCtrl.carryOff(interObj);
            interObj.isInterLock = true;
            interObj.transform.SetParent(this.transform);
            interObj.transform.localPosition = Vector3.zero;
            interObj.transform.localEulerAngles = Vector3.zero;

            GameManager.Instence.scoreUp(thief);
        }
    }

    public override bool shortUse(InterCtrl interCtrl)
    {
        return true;
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

}
