using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterObj_Statue : InterObj
{
    public InterObj_StatueTable statueTable;

    public override void initAwake()
    {
        myKind = InterKind.Statue;
    }


    public override bool shortUse(InterCtrl interCtrl)
    {
        interCtrl.rootCtrl.carryCtrl.carryItem(statueTable);
        interCtrl.useObj = this;
        isNotFind = true;
        return true;
    }

    public override void useEnd(InterCtrl interCtrl)
    {
        if (interCtrl.useObj == this)
        {
            if (interCtrl.selectObj != null && this.checkInterKind(interCtrl.selectObj) && interCtrl.selectObj == statueTable)
            {
                // 전시품 복귀
                interCtrl.selectObj.setInterChild(interCtrl, this);
            }
            else
            {
                //내려 놓기
                interCtrl.rootCtrl.carryCtrl.carryOff(this);
            }
            isNotFind = false;
            statueTable.isCarry = false;
            interCtrl.useObj = null;
        }

    }

    public override bool longUse(InterCtrl interCtrl)
    {
        return false;
    }

    public override void longCancle(InterCtrl interCtrl)
    {
    }


}
