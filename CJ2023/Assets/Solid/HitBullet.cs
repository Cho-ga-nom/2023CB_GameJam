using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBullet : HitChecker, I_PoolChild
{
    public float radius = 0.1f;
    public Vector3 moveDic;
    public float moveSpeed;
    public Transform model;
    private RaycastHit2D hit;
    private TrailRenderer[] trailRender;
    private void OnEnable()
    {
        if (trailRender == null)
        {
            trailRender = GetComponentsInChildren<TrailRenderer>();
        }
        else
        {
            foreach (TrailRenderer item in trailRender)
            {
                item.enabled = true;
                item.Clear();
            }
        }
    }

    public void setBulletInfo(Vector2 moveDic, float moveSpeed)
    {
        this.moveDic = moveDic;
        this.moveSpeed = moveSpeed;
    }
    Action<I_PoolChild> disableAction;
    public void setDisableAction(Action<I_PoolChild> newDisableAction)
    {
        disableAction = newDisableAction;
    }

    private void Update()
    {
        //이동에 따라 레이 체크
        float angle = Vector3.Angle(Vector3.up, moveDic.normalized);
        model.eulerAngles = angle * Vector3.forward * (moveDic.x < 0 ? 1f : -1f);
        Vector3 realMoveDelta = moveDic * moveSpeed * Time.deltaTime;
        this.transform.Translate(realMoveDelta);
        hit = Physics2D.CircleCast(this.transform.position - (realMoveDelta), radius, moveDic, realMoveDelta.magnitude, GameManager.hitCheckLayer);
        if (hit)
        {
            if (checkTarget(hit.collider.transform, hit.point))
            {
                if (trailRender != null)
                {
                    foreach (TrailRenderer item in trailRender)
                    {
                        item.enabled = false;
                        item.Clear();
                    }
                }
                disableAction?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }

    public Transform getTran()
    {
        return this.transform;
    }
}
