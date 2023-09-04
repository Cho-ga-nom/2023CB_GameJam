using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpCtrl : CtrlParent
{
    public int maxHp = 3;
    public int nowHp;

    public override void initAwakeChild()
    {
        nowHp = maxHp;
    }
    public void sendDamage(int damage, Vector3 pos)
    {
        switch (rootCtrl.stateCtrl.nowState)
        {
            case StateKind.None:
            case StateKind.Idle:
            case StateKind.Move:
                break;
            case StateKind.Attack:
            case StateKind.Stun:
            case StateKind.Rope:
                //����
                return;
        }
        //Todo ���� ����Ʈ - ��Ʈ
        Transform effectTran = DataManager.Instence.effectCall("Hit");
        effectTran.position = pos;
        effectTran.gameObject.SetActive(true);
        DataManager.Instence.soundCall("PlayerHit", transform);

        nowHp -= damage;
        if (nowHp <= 0)
        {
            callDeath();
        }
    }
    public void callDeath()
    {
        rootCtrl.stateCtrl.changeState(StateKind.Stun);//����
    }


    public void repairHp()
    {
        nowHp = maxHp;
    }
}
