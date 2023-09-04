using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl_AI_Attack : TargetCtrl
{
    //public Transform exitTran;//Ż���
    //public InterObj_StatueTable nowStaute;
    public RootCtrl playerRoot => GameManager.Instence.playerCtrl;
    public float attackRange = 5f;//�þ߹������ ��
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
                    //���� ������ �ൿ
                    //�⺻ - ����
                    //���� - ����
                    //��� - ����
                    //�Ժ� - �絵��
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
        //�÷��̾ ������մϴ�.
        //if (nowTarget == null)//��Ʈ�� ���ٰ��� �÷��̾ ������ �ν��ؾ���
        //{
        if (playerRoot != null && isAttackDelay == false)//���� ���, ���� �� �ƴ�
        {
            float dis = Vector2.Distance(playerRoot.animaterCtrl.pivotCtrl.root.position, rootCtrl.animaterCtrl.pivotCtrl.root.position);
            if (isChase && dis < attackRange)//�߰� ���ɻ� ����
            {
                //nowTarget = playerRoot.animaterCtrl.modelCtrl.Body;
                playerField = PathManager.Instence.getContainField(playerRoot.animaterCtrl.pivotCtrl.root.position);
                patrolTran.position = playerField.getOffset() + playerField.pivotTran.position;
                nowTarget = patrolTran;
                isAttackDelay = true;//���� �����
                attackTeme = attackDelay;
            }
            else
            {
                //��Ʈ��
                patrolUpdate();
            }
        }
        //}
        return nowTarget;//�÷��̾ �������� ������ Ǯ���� ����
    }
    public override bool checkTargetState()
    {
        if (playerRoot != null && isChase && isAttackDelay)
        {
            if (playerRoot.stateCtrl.nowState == StateKind.Stun)
            {
                isChase = false;//���� ���� ��������� �߰� ���� <���Ŀ� getTarget���� ��Ʈ�ѷ� ����
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
        //��Ʈ��
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
            //���
            //patrolTran.position = findField.getOffset() + findField.pivotTran.position;
        }
        else if (isAttackDelay == false && isAttackOn == false)
        {
            isChase = true;//�÷��̾ ������Ű�� �������� �ѹ� �����ϸ� ���Ŀ� �ٽ� �߰� ����
                           //������
            getTarget();
        }
        base.arriveTarget();
    }
    public void patrolUpdate()
    {
        Debug.Log("�ٽ�!!6");
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
                //���ݸ��
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
            if (isChase && dis < attackRange)//�߰� ���ɻ� ����
            {
                //nowTarget = playerRoot.animaterCtrl.modelCtrl.Body;
                playerField = PathManager.Instence.getContainField(playerRoot.animaterCtrl.pivotCtrl.root.position);
                patrolTran.position = playerField.getOffset() + playerField.pivotTran.position;
                nowTarget = patrolTran;
                isAttackDelay = true;//���� �����
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
