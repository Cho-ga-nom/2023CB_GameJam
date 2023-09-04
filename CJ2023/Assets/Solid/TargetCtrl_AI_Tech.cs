using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl_AI_Tech : TargetCtrl
{
    //public Transform exitTran;//Ż���
    public InterObj_StatueTable nowStaute;
    private Transform patrolTran;
    private float runTemp;
    private bool isExit;
    public float startWait;
    IEnumerator cor_wait()
    {
        yield return new WaitForSeconds(startWait);
        startWait = 0f;
    }
    public override void initStart()
    {
        startWait = 1.3f;
        StartCoroutine(cor_wait());
        rootCtrl.animaterCtrl.modelAni.SetTrigger("Start");
        isExit = false;
        runTemp = 5f;
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
        if (startWait > 0f) return null;
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
        if (startWait > 0f) return true;
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
        Debug.Log("�ٽ�!!2");

        PathField field = PathManager.Instence.fieldList[UnityEngine.Random.Range(0, PathManager.Instence.fieldList.Count)];
        patrolTran.position = field.getOffset() + field.pivotTran.position;
        nowTarget = patrolTran;
        pathInfo.Clear();
        PathManager.Instence.findPath(rootCtrl.animaterCtrl.pivotCtrl.root.position, nowTarget.position, ref pathInfo, true);
    }
    public override void targetUpdate()
    {

        if (startWait > 0f)
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
        base.targetUpdate();
    }
}
