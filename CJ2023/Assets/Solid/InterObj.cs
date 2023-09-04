using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterObj : MonoBehaviour
{
    public Transform PivotTran;
    public InterKind myKind;
    public bool isInterLock;
    public bool isNotFind;
    public Collider2D collider;

    private void Awake()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider2D>();
        }
        if (PivotTran == null)
        {
            PivotTran = this.transform;
        }
        initAwake();
    }
    public abstract void initAwake();
    public bool checkInterKind(InterObj interObj)
    {
        switch (myKind)
        {
            case InterKind.None:
                break;
            case InterKind.Statue:
                if (interObj.myKind == InterKind.StatueTable)
                {
                    //전시품 복귀
                    return true;
                }
                break;
            case InterKind.StatueTable:
                break;
            case InterKind.Thief:
                if (interObj.myKind == InterKind.Prison)
                {
                    //수감
                    return true;
                }
                break;
            case InterKind.Prison:
                break;

        }
        return false;
    }

    public virtual void setInterChild(InterCtrl interCtrl, InterObj interObj)
    {
        throw new NotImplementedException();
    }
    public virtual void interEnable(InterCtrl interCtrl)
    {

    }

    public virtual void interDisable(InterCtrl interCtrl)
    {

    }

    public abstract bool shortUse(InterCtrl interCtrl);
    public abstract void useEnd(InterCtrl interCtrl);// 내려 놓음
    public abstract bool longUse(InterCtrl interCtrl);
    public abstract void longCancle(InterCtrl interCtrl);//홀드중 취소
}
public enum InterKind
{
    None = 0,//미설정
    Statue = 1,//전시품
    StatueTable = 2,//전시품
    Thief = 3, //도둑
    Prison = 4, //감옥
    Item = 5,
}