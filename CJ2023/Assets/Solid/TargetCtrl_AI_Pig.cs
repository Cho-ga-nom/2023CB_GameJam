using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl_AI_Pig : TargetCtrl
{
    //public Transform exitTran;//Ż���
    public InterObj_StatueTable nowStaute;
    private Transform patrolTran;
    public float bulletSpeed = 1f;
    public float attackTime = 2f;
    public float attackTemp;
    public HitBullet hitBullet;
    private float runTemp;
    private bool isExit;

    public override void initStart()
    {
        isExit = false;
        runTemp = 5f;
        attackTime = charaData.AttackDelay;
        hitBullet.gameObject.SetActive(false);
        poolSystem.createItem = () => { return Instantiate(hitBullet); };
        patrolTran = new GameObject().transform;
        patrolTran.SetParent(null);
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
        base.initStart();
    }

    public override Transform getTarget()
    {
        //����ǰ�� ������մϴ�.
        if (nowTarget == null)//Ÿ���� �����߸� ����ǰ�� �븲
        {
            List<InterObj_StatueTable> findList = GameManager.Instence.statueList.FindAll(statue => statue.isTargetOn);
            if (findList != null && findList.Count > 0)
            {
                nowStaute = findList[Random.Range(0, findList.Count)];
                nowTarget = nowStaute.interItem.transform;
            }
            else
            {
                //��Ʈ��
                patrolUpdate();
            }

        }
        return nowTarget;
    }
    public override bool checkTargetState()
    {
        if (nowStaute != null)
        {
            if (nowStaute.isTargetOn == false)
            {
                nowStaute = null;
                return true;
            }
            return false;
        }
        //��Ʈ��
        return false;
    }
    public override void arriveTarget()
    {
        if (isExit)
        {
            if (rootCtrl.interCtrl.useObj != null)
            {
                Debug.Log("Ż���̴�!!");
                GameManager.Instence.scoreDown();
                if (nowStaute != null)
                {
                    nowStaute.isTheft = true;//���� ����
                    nowStaute.interItem.gameObject.SetActive(false);
                    rootCtrl.carryCtrl.carryOff(nowStaute.interItem);
                }
            }
            rootCtrl.gameObject.SetActive(false);
        }
        else if (nowStaute != null)
        {
            isArrive = false;
            if (nowStaute.isItemOn)
            {
                nowStaute.shortUse(rootCtrl.interCtrl);
            }
            else
            {
                nowStaute.interItem.shortUse(rootCtrl.interCtrl);
            }
            nowStaute = null;
            patrolUpdate();
        }
        else
        {
            //������
            patrolUpdate();
        }
        base.arriveTarget();
    }
    public void patrolUpdate()
    {
        isArrive = false;
        Debug.Log("�ٽ�!!3");

        PathField field = PathManager.Instence.fieldList[UnityEngine.Random.Range(0, PathManager.Instence.fieldList.Count)];
        patrolTran.position = field.getOffset() + field.pivotTran.position;
        nowTarget = patrolTran;
        pathInfo.Clear();
        PathManager.Instence.findPath(rootCtrl.animaterCtrl.pivotCtrl.root.position, nowTarget.position, ref pathInfo, true);
    }
    public PoolSystem poolSystem = new PoolSystem();
    public override void targetUpdate()
    {
      
        if (isExit)
        {
            return;
        }
        if (rootCtrl.interCtrl.useObj != null && isExit == false)
        {
            runTemp -= Time.deltaTime;
            if (runTemp <= 0)
            {
                isExit = true;//������
                isArrive = false;
                pathInfo.Clear();
                patrolTran.position = SpawnManager.Instence.spawnPoints[UnityEngine.Random.Range(0, SpawnManager.Instence.spawnPoints.Count)].position;
                nowTarget = patrolTran;
                PathManager.Instence.findPath(rootCtrl.animaterCtrl.pivotCtrl.root.position, nowTarget.position, ref pathInfo, true);

            }
        }
        switch (rootCtrl.stateCtrl.nowState)
        {
            case StateKind.None:
            case StateKind.Idle:
            case StateKind.Move:
                if (rootCtrl.interCtrl.useObj == null)
                {
                    attackTemp -= Time.deltaTime;
                    if (attackTemp <= 0f)
                    {
                        attackTemp = attackTime;
                        HitBullet bullet = poolSystem.newItem() as HitBullet;
                        bullet.rootCtrl = this.rootCtrl;
                        bullet.setBulletInfo((GameManager.Instence.playerCtrl.animaterCtrl.pivotCtrl.Body.position - rootCtrl.animaterCtrl.pivotCtrl.Head.position).normalized, bulletSpeed);
                        bullet.gameObject.SetActive(true);
                        bullet.transform.position = rootCtrl.animaterCtrl.pivotCtrl.Head.position;
                    }
                }
                break;
        }
    }

}
