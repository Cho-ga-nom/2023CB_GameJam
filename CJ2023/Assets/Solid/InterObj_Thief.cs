using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterObj_Thief : InterObj
{
    public RootCtrl rootCtrl;
    public float ropeTime = 0.2f;
    public Coroutine corRopeCheck;
    public bool isRoping;

    public override void initAwake()
    {
        myKind = InterKind.Thief;
    }


    public override bool shortUse(InterCtrl interCtrl)
    {
        switch (rootCtrl.stateCtrl.nowState)
        {
            case StateKind.None:
                break;
            case StateKind.Idle:
                break;
            case StateKind.Move:
                break;
            case StateKind.Attack:
                break;

            case StateKind.Stun:
            case StateKind.Rope:
                rootCtrl.stateCtrl.stunTran?.GetComponent<PoolChild>().disableCall();
                rootCtrl.stateCtrl.stunTran = null;
                rootCtrl.stateCtrl.isInterHold = true;
                rootCtrl.collider.enabled = false;
                interCtrl.rootCtrl.carryCtrl.carryThief(this);//들쳐매기
                interCtrl.useObj = this;
                break;
        }
        return true;
    }

    public override void useEnd(InterCtrl interCtrl)
    {

        if (interCtrl.useObj == this)
        {
            if (interCtrl.selectObj != null && this.checkInterKind(interCtrl.selectObj))
            {
                // 감옥행
                interCtrl.selectObj.setInterChild(interCtrl, this);
            }
            else
            {
                rootCtrl.stateCtrl.isInterHold = false;
                rootCtrl.collider.enabled = true;
                //내려 놓기
            }

            interCtrl.useObj = null;
            interCtrl.rootCtrl.carryCtrl.carryOff(this);
        }

    }

    public override bool longUse(InterCtrl interCtrl)
    {
        if (rootCtrl.stateCtrl.nowState == StateKind.Stun && rootCtrl.stateCtrl.nextState != StateKind.Rope)
        {
            if (corRopeCheck == null)
            {
                if (interCtrl.rootCtrl.playerCtrl.tieAmount > 0)
                {
                    CoolTime.Instence.tieCool(0f);
                    interCtrl.rootCtrl.playerCtrl.tieAmount -= 1;
                    rootCtrl.stateCtrl.isInterHold = true;
                    corRopeCheck = StartCoroutine(cor_Roping());
                }
            }
            return true;
        }
        return false;
    }
    public IEnumerator cor_Roping()
    {
        float ropeTemp = ropeTime;
        isRoping = true;
        while (ropeTemp > 0f && isRoping)
        {
            ropeTemp -= Time.deltaTime;
            CoolTime.Instence.tieCool(1f - (ropeTemp / ropeTime));
            yield return null;
        }
        CoolTime.Instence.tieCool(0f);
        if (isRoping)
        {
            //묶음
            rootCtrl.stateCtrl.changeState(StateKind.Rope);

        }
        rootCtrl.stateCtrl.isInterHold = false;
        corRopeCheck = null;
    }

    public override void longCancle(InterCtrl interCtrl)
    {
        isRoping = false;
        CoolTime.Instence.tieCool(0f);
        if (corRopeCheck != null)
        {
            StopCoroutine(corRopeCheck);
            rootCtrl.stateCtrl.isInterHold = false;
            corRopeCheck = null;
        }


        rootCtrl.stateCtrl.isInterHold = false;
    }



}
