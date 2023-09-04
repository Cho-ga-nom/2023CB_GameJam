using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl_AI_Attack : TargetCtrl
{
    //public Transform exitTran;//탈출로
    //public InterObj_StatueTable nowStaute;
    public RootCtrl playerRoot => GameManager.Instence.playerCtrl;
    public float attackRange = 5f;//시야범위라고 봄
    public bool isChase;
    public bool isAttackDelay;
    public bool isAttackOn;
    private Transform patrolTran;
    private PathField playerField;

    public override void initStart()
    {
        attackDelay = charaData.AttackDelay;
        isChase = true;
        patrolTran = new GameObject().transform;
        patrolTran.SetParent(null);
        rootCtrl.moveCtrl.isRun = false;

        rootCtrl.animaterCtrl.pivotCtrl.eventCall += (eventTag) =>
        {
            if (eventTag.Equals("AttackEnd"))
            {
                rootCtrl.stateCtrl.changeState(StateKind.Idle, true);
                rootCtrl.moveCtrl.isRun = false;
                isAttackOn = false;
                isChase = false;
                nowTarget = null;
            }
        };
        rootCtrl.stateCtrl.setStateAction += (state) =>
        {
            switch (state)
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
                    pathInfo.Clear();
                    nowTarget = null;
                    //기절 이후의 행동
                    //기본 - 도망
                    //공격 - 순찰
                    //기술 - 도망
                    //먹보 - 재도전
                    break;
            }
        };
        rootCtrl.stateCtrl.unsetStateAction += (state) =>
        {
            switch (state)
            {
                case StateKind.None:
                    break;
                case StateKind.Idle:
                    break;
                case StateKind.Move:
                    break;
                case StateKind.Attack:
                    isAttackOn = false;
                    break;

                case StateKind.Stun:
                case StateKind.Rope:
                    isArrive = false;
                    isAttackDelay = false;
                    isAttackOn = false;
                    pathInfo.Clear();
                    patrolUpdate();
                    break;
            }
        };
        base.initStart();
    }

    public override Transform getTarget()
    {
        //플레이어를 노려야합니다.
        //if (nowTarget == null)//패트롤 돌다가도 플레이어가 가까우면 인식해야함
        //{
        if (playerRoot != null && isAttackDelay == false)//공격 대기, 공격 중 아님
        {
            float dis = Vector2.Distance(playerRoot.animaterCtrl.pivotCtrl.root.position, rootCtrl.animaterCtrl.pivotCtrl.root.position);
            if (isChase && dis < attackRange)//추격 가능상 태임
            {
                //nowTarget = playerRoot.animaterCtrl.modelCtrl.Body;
                playerField = PathManager.Instence.getContainField(playerRoot.animaterCtrl.pivotCtrl.root.position);
                patrolTran.position = playerField.getOffset() + playerField.pivotTran.position;
                nowTarget = patrolTran;
                isAttackDelay = true;//공격 대기모드
                attackTeme = attackDelay;
            }
            else
            {
                //패트롤
                patrolUpdate();
            }
        }
        //}
        return nowTarget;//플레이어가 기절하지 않으면 풀리지 않음
    }
    public override bool checkTargetState()
    {
        if (playerRoot != null && isChase && isAttackDelay)
        {
            if (playerRoot.stateCtrl.nowState == StateKind.Stun)
            {
                isChase = false;//일정 범위 벗어날때까지 추격 안함 <이후에 getTarget에서 패트롤로 빠짐
                isAttackDelay = false;
                isAttackOn = false;
                return true;
            }
            PathField findField = PathManager.Instence.getContainField(playerRoot.animaterCtrl.pivotCtrl.root.position);
            if (findField != null && playerField != findField)
            {
                playerField = findField;
                patrolTran.position = findField.getOffset() + findField.pivotTran.position;
            }
            return false;
        }
        else if (playerRoot != null && isChase && isAttackDelay == false && isAttackOn == false && playerRoot.stateCtrl.nowState != StateKind.Stun)
        {
            //nowTarget = null;
            //return true;
        }
        //패트롤
        return false;
    }
    public override void arriveTarget()
    {
        if (playerRoot != null && isAttackOn)
        {
            if (playerRoot.animaterCtrl.pivotCtrl.Body == nowTarget)
            {
                isArrive = false;
                rootCtrl.stateCtrl.changeState(StateKind.Attack);
            }
        }
        else if (isAttackDelay)
        {
            //대기
            //patrolTran.position = findField.getOffset() + findField.pivotTran.position;
        }
        else if (isAttackDelay == false && isAttackOn == false)
        {
            isChase = true;//플레이어를 기절시키고 목적지에 한번 도달하면 이후에 다시 추격 가능
                           //순찰중
            getTarget();
        }
        base.arriveTarget();
    }
    public void patrolUpdate()
    {
        Debug.Log("다시!!6");
        isArrive = false;
        PathField field = PathManager.Instence.fieldList[UnityEngine.Random.Range(0, PathManager.Instence.fieldList.Count)];
        patrolTran.position = field.getOffset() + field.pivotTran.position;
        nowTarget = patrolTran;
        pathInfo.Clear();
        PathManager.Instence.findPath(rootCtrl.animaterCtrl.pivotCtrl.root.position, nowTarget.position, ref pathInfo, true);
    }

    private float attackDelay = 5f;
    private float attackTeme;
    public override void targetUpdate()
    {

        if (playerRoot != null && isChase && isAttackDelay)
        {
            attackTeme -= Time.deltaTime;
            if (attackTeme <= 0f)
            {
                //공격모드
                isAttackDelay = false;
                isAttackOn = true;
                rootCtrl.moveCtrl.isRun = true;
                isArrive = false;
                nowTarget = playerRoot.animaterCtrl.pivotCtrl.root;
                pathInfo.Clear();
                PathManager.Instence.findPath(rootCtrl.animaterCtrl.pivotCtrl.root.position, nowTarget.position, ref pathInfo, true);
            }
        }
        else if (isChase && isAttackOn == false && isAttackDelay == false)
        {
            float dis = Vector2.Distance(playerRoot.animaterCtrl.pivotCtrl.root.position, rootCtrl.animaterCtrl.pivotCtrl.root.position);
            if (isChase && dis < attackRange)//추격 가능상 태임
            {
                //nowTarget = playerRoot.animaterCtrl.modelCtrl.Body;
                playerField = PathManager.Instence.getContainField(playerRoot.animaterCtrl.pivotCtrl.root.position);
                patrolTran.position = playerField.getOffset() + playerField.pivotTran.position;
                nowTarget = patrolTran;
                isAttackDelay = true;//공격 대기모드
                attackTeme = attackDelay;
            }
        }
        //else if (playerRoot != null && isAttackOn)
        //{
        //    if (playerRoot.animaterCtrl.modelCtrl.Body == nowTarget)
        //    {
        //        isArrive = false;
        //        rootCtrl.stateCtrl.changeState(StateKind.Attack);
        //    }
        //}
    }
}
