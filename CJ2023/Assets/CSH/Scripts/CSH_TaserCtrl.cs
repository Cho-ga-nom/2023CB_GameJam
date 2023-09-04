using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_TaserCtrl : MonoBehaviour
{
    public RootCtrl RootCtrl;
    public bool isAiming = false;
    public float rayDist = 2f;
    public Action actionEnd;
    public Transform FirePivot;
    public Transform pivotTran;

    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Aim()
    {
        Vector3 startPos = FirePivot.position;
        startPos.z = 0;
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, FirePivot.up, rayDist);
        Collider2D hit = null;
        float minDis = 0;
        Vector3 vector3 = Vector3.zero;

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        for (int i = 0; i < hits.Length; i++)
        {
            RootCtrl rootCtrl = hits[i].transform.GetComponentInParent<RootCtrl>();
            if (rootCtrl != null && rootCtrl == RootCtrl)
            {
                continue;
            }

            float dis = Vector2.Distance(startPos, hits[i].point);
            if (hit == null || dis < minDis)
            {
                hit = hits[i].collider;
                minDis = dis;
                vector3 = hits[i].point;
            }
        }

        // 충돌한 경우
        if (hit != null)
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, vector3);
        }
        else
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, startPos + FirePivot.up * rayDist);
        }
    }

    public void TaserEnd()
    {
        actionEnd?.Invoke();
        this.gameObject.SetActive(false);
    }
}
