using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : CtrlParent
{
    public static readonly Vector3 right = new Vector3(1, 1, 1);
    public static readonly Vector3 left = new Vector3(-1, 1, 1);

    public Vector2 moveSpeed = new Vector2(1f, 2f);
    public bool isRun;
    public bool isRight;


    public bool isRoll;
    public float rollTime = 0.2f;
    public float rollRange = 0.5f;
    public float rollTemp;
    public float moveDis;


    public override void initAwakeChild()
    {
        CoolTime.Instence.RollCool(0f);
    }
    private void Update()
    {
        if (this.rollTemp > 0f)
        {
            this.rollTemp -= Time.deltaTime;
            CoolTime.Instence.RollCool((rollTemp / DataManager.Instence.PlayerData.RollDelay));
            if (this.rollTemp <= 0f)
            {
                CoolTime.Instence.RollCool(0f);
            }
        }
    }
    public void callRoll(Vector2 moveDic)
    {
        switch (rootCtrl.stateCtrl.nowState)
        {
            case StateKind.Idle:
            case StateKind.Move:
                if (this.rollTemp > 0f)
                {
                    return;
                }
                StartCoroutine(cor_Roll(moveDic));
                break;
            case StateKind.Attack:
                break;

            case StateKind.Stun:
                break;
            case StateKind.Rope:
                break;
        }
    }
    private IEnumerator cor_Roll(Vector3 moveDic)
    {
        this.rollTemp = DataManager.Instence.PlayerData.RollDelay;// * (1f - DataManager.Instence.NowPlayerData.MoveSpeedAdd);
        float rollPowar = DataManager.Instence.NowPlayerData.MoveLv * 0.03f;
        CoolTime.Instence.RollCool(1f);
        DataManager.Instence.soundCall("Roll", this.transform);
        float rollTemp = rollTime;//여긴 이동 temp
        rootCtrl.inputCtrl.isLock = true;
        isRoll = true;
        float rollSpeed = rollRange / rollTime;
        while (rollTemp > 0)
        {
            rollTemp -= Time.deltaTime;
            moveUpdate(moveDic, rollSpeed);
            lookUpdate(rootCtrl.animaterCtrl.pivotCtrl.root.position + moveDic);
            yield return null;
        }
        rootCtrl.inputCtrl.isLock = false;
        while (rollPowar > 0f)
        {
            rollPowar -= Time.deltaTime;
            yield return null;
        }
        isRoll = false;

    }
    public void moveUpdate(Vector2 moveDic)
    {
        moveDis = moveDic.magnitude;
        moveUpdate(moveDic, (isRun ? moveSpeed.y : moveSpeed.x));
    }
    public void moveUpdate(Vector2 moveDic, float moveSpeed)
    {
        switch (rootCtrl.stateCtrl.nowState)
        {
            case StateKind.None:
            case StateKind.Idle:
            case StateKind.Move:
                rootCtrl.stateCtrl.changeState((moveSpeed * moveDic).magnitude < 0.1f ? StateKind.Idle : StateKind.Move);
                break;
            case StateKind.Attack:
                break;

            case StateKind.Stun:
                break;
            case StateKind.Rope:
                break;
        }
        rootCtrl.transform.Translate(moveDic * moveSpeed * Time.deltaTime);
        rootCtrl.animaterCtrl.modelAni.SetFloat("Move", Mathf.Min(moveDic.magnitude, 1f));
    }
    public void lookUpdate(Vector3 lootWorldPos)
    {
        if ((lootWorldPos.x - rootCtrl.transform.position.x) >= 0)
        {
            isRight = true;
            //오른쪽
            rootCtrl.animaterCtrl.modelTran.localScale = right;
        }
        else
        {
            isRight = false;
            //왼쪽
            rootCtrl.animaterCtrl.modelTran.localScale = left;
        }
    }
}
