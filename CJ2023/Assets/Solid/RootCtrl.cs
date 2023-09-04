using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCtrl : MonoBehaviour
{
    public int TeamIndex = 0;

    public InputCtrl inputCtrl;
    public MoveCtrl moveCtrl;
    public HpCtrl hpCtrl;
    public AnimaterCtrl animaterCtrl;
    public StateCtrl stateCtrl;
    public WeaponCtrl weaponCtrl;
    public CarryCtrl carryCtrl;
    public InterCtrl interCtrl;
    public PlayerCtrl playerCtrl;
    public TargetCtrl targetCtrl;
    public List<CtrlParent> ctrlList;

    public void Update()
    {
        inputCtrl.InputUpdate();
    }
    public Collider2D collider;
    private bool isInit;
    public void Awake()
    {
        init();
    }
    public void init()
    {
        if (isInit) return;
        isInit = true;
        collider = this.GetComponent<Collider2D>();
        CtrlParent[] ctrls = this.GetComponentsInChildren<CtrlParent>(true);
        foreach (CtrlParent ctrl in ctrls)
        {
            ctrlList.Add(ctrl);
            ctrl.initAwake(this);
            if (ctrl is MoveCtrl)
            {
                moveCtrl = (MoveCtrl)ctrl;
            }
            else if (ctrl is InputCtrl)
            {
                inputCtrl = (InputCtrl)ctrl;
            }
            else if (ctrl is HpCtrl)
            {
                hpCtrl = (HpCtrl)ctrl;
            }
            else if (ctrl is AnimaterCtrl)
            {
                animaterCtrl = (AnimaterCtrl)ctrl;
            }
            else if (ctrl is StateCtrl)
            {
                stateCtrl = (StateCtrl)ctrl;
            }
            else if (ctrl is WeaponCtrl)
            {
                weaponCtrl = (WeaponCtrl)ctrl;
            }
            else if (ctrl is CarryCtrl)
            {
                carryCtrl = (CarryCtrl)ctrl;
            }
            else if (ctrl is InterCtrl)
            {
                interCtrl = (InterCtrl)ctrl;
            }
            else if (ctrl is PlayerCtrl)
            {
                playerCtrl = (PlayerCtrl)ctrl;
            }
            else if (ctrl is TargetCtrl)
            {
                targetCtrl = (TargetCtrl)ctrl;
            }


        }
        foreach (CtrlParent ctrl in ctrlList)
        {
            ctrl.initStart();
        }
        animaterCtrl.pivotCtrl.root = this.transform;
    }
    public void setCtrl(CtrlParent newCtrl)
    {
        ctrlList.Add(newCtrl);
        newCtrl.initAwake(this);
    }
    public T FindCtrl<T>() where T : CtrlParent
    {
        foreach (CtrlParent ctrl in ctrlList)
        {
            if (ctrl is T checkCtrl)
            {
                ctrl.findAwake();
                return checkCtrl;
            }
        }
        T findCtrl = GetComponentInChildren<T>();
        if (findCtrl != null)
        {
            findCtrl.findAwake();
            return findCtrl;
        }
        return null;
    }
}
