using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputCtrl : CtrlParent
{
    public bool isLock;
    public override void initAwakeChild()
    {
    }
    public abstract void InputUpdate();

}
