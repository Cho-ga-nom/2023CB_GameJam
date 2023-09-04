using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CtrlParent : MonoBehaviour
{
    public RootCtrl rootCtrl;
    public void initAwake(RootCtrl rootCtrl)
    {
        this.rootCtrl = rootCtrl;
        initAwakeChild();
    }
    public virtual void initStart()
    {

    }
    public void findAwake()
    {
        if (rootCtrl == null)
        {
            rootCtrl = this.GetComponentInParent<RootCtrl>(true);
            rootCtrl.setCtrl(this);
        }
    }

    public abstract void initAwakeChild();

}
