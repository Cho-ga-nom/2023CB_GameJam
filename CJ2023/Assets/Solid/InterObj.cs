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
                    //����ǰ ����
                    return true;
                }
                break;
            case InterKind.StatueTable:
                break;
            case InterKind.Thief:
                if (interObj.myKind == InterKind.Prison)
                {
                    //����
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
    public abstract void useEnd(InterCtrl interCtrl);// ���� ����
    public abstract bool longUse(InterCtrl interCtrl);
    public abstract void longCancle(InterCtrl interCtrl);//Ȧ���� ���
}
public enum InterKind
{
    None = 0,//�̼���
    Statue = 1,//����ǰ
    StatueTable = 2,//����ǰ
    Thief = 3, //����
    Prison = 4, //����
    Item = 5,
}