using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCtrl : CtrlParent
{
    public StateKind nowState;
    public StateKind nextState;
    public Action<StateKind> setStateAction;
    public Action<StateKind> unsetStateAction;
    public Transform stunTran;
    public float stunTime = 1f;
    public float stunTemp;
    public float ropeTime = 3f;
    public float ropeTemp;

    public bool isInterHold;
    public bool isLock = false;

    public override void initAwakeChild()
    {
        nextState = nowState = StateKind.Idle;
    }

    private void Update()
    {
        if (isLock) return;
        stateChecke();
        stateUpdate();
    }
    public void stateUpdate()
    {
        switch (nowState)
        {
            case StateKind.None:
                break;
            case StateKind.Idle:
            case StateKind.Move:

                break;
            case StateKind.Attack:
                break;

            case StateKind.Stun:
                if (isInterHold) return;
                stunTemp -= Time.deltaTime;
                if (stunTemp <= 0f)
                {
                    stunTran?.GetComponent<PoolChild>().disableCall();
                    stunTran = null;
                    changeState(StateKind.Idle);
                }
                break;
            case StateKind.Rope:
                if (isInterHold) return;
                if (stunTemp > 0f)
                {
                    stunTemp -= Time.deltaTime;
                    if (stunTemp <= 0f)
                    {
                        stunTran?.GetComponent<PoolChild>().disableCall();
                        stunTran = null;
                    }
                }
                else
                {
                    ropeTemp -= Time.deltaTime;
                    if (ropeTemp <= 0f)
                    {
                        if (DataManager.Instence.itemPool.ContainsKey("Rope") == false)
                        {
                            DataManager.Instence.itemPool["Rope"] = new PoolSystem() { createItem = () => { return Instantiate(DataManager.Instence.ropePrefab).GetComponent<I_PoolChild>(); } };
                        }
                        InterObj_Item item = DataManager.Instence.itemPool["Rope"].newItem().getTran().GetComponent<InterObj_Item>();
                        item.isAdd = false;
                        item.transform.position = this.rootCtrl.transform.position;
                        item.gameObject.SetActive(true);
                        changeState(StateKind.Idle);
                    }
                }
                break;
        }
    }
    public void stateChecke()
    {
        if (nowState != nextState)
        {
            unsetStateAction?.Invoke(nowState);
            switch (nowState)
            {
                case StateKind.None:
                    break;
                case StateKind.Idle:
                    break;
                case StateKind.Move:
                    break;
                case StateKind.Attack:
                    rootCtrl.animaterCtrl.modelAni.SetBool("IsAttack", false);
                    break;

                case StateKind.Stun:
                    rootCtrl.inputCtrl.isLock = false;
                    rootCtrl.animaterCtrl.modelAni.SetBool("IsStun", false);
                    rootCtrl.hpCtrl.repairHp();
                    break;
                case StateKind.Rope:
                    rootCtrl.inputCtrl.isLock = false;
                    rootCtrl.animaterCtrl.modelAni.SetBool("IsRope", false);
                    rootCtrl.hpCtrl.repairHp();
                    break;
            }
            nowState = nextState;
            setStateAction?.Invoke(nowState);
            switch (nowState)
            {
                case StateKind.None:
                    break;
                case StateKind.Idle:
                    break;
                case StateKind.Move:
                    break;
                case StateKind.Attack:
                    rootCtrl.animaterCtrl.modelAni.SetBool("IsAttack", true);
                    break;

                case StateKind.Stun:
                    rootCtrl.inputCtrl.isLock = true;
                    rootCtrl.animaterCtrl.modelAni.SetBool("IsStun", true);
                    stunTran = DataManager.Instence.effectCall("StunStar");
                    stunTran.GetComponent<PoolChild>().autoTime = 0f;
                    stunTran.position = rootCtrl.animaterCtrl.pivotCtrl.Body.position + (((rootCtrl.animaterCtrl.modelTran.localScale.x > 0 ? -rootCtrl.animaterCtrl.modelTran.right : rootCtrl.animaterCtrl.modelTran.right) + rootCtrl.animaterCtrl.modelTran.up) * 0.7f);
                    stunTran.gameObject.SetActive(true);
                    if (rootCtrl.interCtrl.useObj != null)
                    {//들고있는게 있는 경우임
                        rootCtrl.interCtrl.useDown();//해제임
                    }
                    stunTemp = stunTime;
                    break;
                case StateKind.Rope:
                    if (rootCtrl.interCtrl.useObj != null)
                    {//들고있는게 있는 경우임
                        rootCtrl.interCtrl.useDown();//해제임
                    }
                    rootCtrl.inputCtrl.isLock = true;
                    rootCtrl.animaterCtrl.modelAni.SetBool("IsRope", true);
                    //묶기전에 기절해야해서 안함
                    //if (rootCtrl.interCtrl.useObj != null)
                    //{//들고있는게 있는 경우임
                    //    rootCtrl.interCtrl.useDown();//해제임
                    //}
                    ropeTemp = ropeTime;
                    break;
            }
        }
    }
    public void changeState(StateKind stateKind, bool isForce = false)
    {
        if (nextState != nowState && isForce == false)
        {
            switch (stateKind)
            {
                case StateKind.None:
                    break;
                case StateKind.Idle:
                case StateKind.Move:
                    switch (nextState)
                    {
                        case StateKind.Attack:
                        case StateKind.Stun:
                        case StateKind.Rope:
                            return;
                    }
                    break;
                case StateKind.Attack:
                    switch (nextState)
                    {
                        case StateKind.Stun:
                        case StateKind.Rope:
                            return;
                    }
                    break;
                case StateKind.Stun:
                    switch (nextState)
                    {
                        case StateKind.Rope:
                            return;
                    }
                    break;
                case StateKind.Rope:
                    break;
            }
        }
        nextState = stateKind;
    }

}
public enum StateKind
{
    None = 0,//미설정
    Idle = 1,//대기
    Move = 2,//이동
    Attack = 4,//공격중
    //Interaction = 5,//상호작용

    Stun = 11,//기절
    Rope = 12,//포박
}