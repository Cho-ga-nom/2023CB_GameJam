using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaterCtrl : CtrlParent
{
    public Transform modelTran;
    public Animator modelAni;
    public PivotInfo pivotCtrl;
    public override void initAwakeChild()
    {
        if (modelTran == null)
        {
            modelTran = this.transform;
        }
        if (modelAni == null)
        {
            modelAni = this.GetComponentInChildren<Animator>();
        }
        if (pivotCtrl == null)
        {
            pivotCtrl = this.GetComponentInChildren<PivotInfo>();
        }
    }
}
