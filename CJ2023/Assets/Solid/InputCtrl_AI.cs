using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtrl_AI : InputCtrl
{


    public InterObj_Thief interObj;
    public Transform targetTran => targetCtrl.nowTarget;
    public TargetCtrl targetCtrl;
    public override void initAwakeChild()
    {
        base.initAwakeChild();
        targetCtrl = rootCtrl.FindCtrl<TargetCtrl>();
    }
    public override void initStart()
    {
        if (interObj == null)
        {
            interObj = rootCtrl.GetComponentInChildren<InterObj_Thief>();
            interObj.rootCtrl = rootCtrl;
            interObj.PivotTran = rootCtrl.transform;
        }
        base.initStart();
    }

    public override void InputUpdate()
    {
        if (isLock) return;
        switch (rootCtrl.stateCtrl.nowState)
        {
            case StateKind.None:
                rootCtrl.stateCtrl.changeState(StateKind.Idle);
                break;
            case StateKind.Idle:
                if (targetTran != null)
                {
                    rootCtrl.stateCtrl.changeState(StateKind.Move);
                }
                else
                {
                    targetCtrl.getTarget();
                }
                break;
            case StateKind.Move:
                if (targetCtrl.checkTargetState() || targetTran == null)
                {
                    rootCtrl.stateCtrl.changeState(StateKind.Idle);
                    return;
                }
                targetCtrl.targetUpdate();
                PathManager.Instence.findPath(rootCtrl.animaterCtrl.pivotCtrl.root.position, targetTran.position, ref targetCtrl.pathInfo);

                if (targetCtrl.pathInfo != null)
                {
                    if (targetCtrl.pathInfo.path.Count > 0 && targetCtrl.pathInfo.isArrive == false)
                    {
                        rootCtrl.moveCtrl.moveUpdate((targetCtrl.pathInfo.nextPos(rootCtrl.animaterCtrl.pivotCtrl.root.position) - rootCtrl.transform.position).normalized);
                        rootCtrl.moveCtrl.lookUpdate(targetTran.position);
                    }
                    else if (targetCtrl.isArrive == false)
                    {
                        //Debug.Log("목적지로 가는중");
                        if (Vector2.Distance(targetTran.position, rootCtrl.animaterCtrl.pivotCtrl.root.position) < 0.1f)
                        {
                            targetCtrl.isArrive = true;
                            targetCtrl.arriveTarget();
                        }
                        else
                        {
                            rootCtrl.moveCtrl.moveUpdate((targetTran.position - rootCtrl.animaterCtrl.pivotCtrl.root.position).normalized);
                            rootCtrl.moveCtrl.lookUpdate(targetTran.position);
                        }
                    }
                    else
                    {
                        Debug.Log("어디냐");
                    }
                }
                break;
            case StateKind.Attack:
                break;

            case StateKind.Stun:
                break;
            case StateKind.Rope:
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (targetCtrl != null && targetCtrl.pathInfo.path.Count > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < targetCtrl.pathInfo.path.Count - 1; i++)
            {
                Gizmos.DrawLine(targetCtrl.pathInfo.path[i].pivotTran.position, targetCtrl.pathInfo.path[i + 1].pivotTran.position);
            }
        }
    }


}
