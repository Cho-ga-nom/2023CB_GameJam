using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolChild : MonoBehaviour, I_PoolChild
{
    public Action<I_PoolChild> disableAction;
    public void setDisableAction(Action<I_PoolChild> newDisableAction)
    {
        disableAction = newDisableAction;
    }
    private void OnEnable()
    {
        isDisalbe = false;
        enableCall();
    }
    private bool isDisalbe;
    public float autoTime = 0f;
    private float autoTemp;
    private void Update()
    {
        if (isDisalbe == false && autoTime > 0f)
        {
            autoTemp -= Time.deltaTime;
            if (autoTemp <= 0f)
            {
                disableCall();
            }
        }
    }
    public void enableCall()
    {
        autoTemp = autoTime;
    }
    public void disableCall()
    {
        if (isDisalbe == false)
        {
            isDisalbe = true;
            disableAction?.Invoke(this);
            this.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        disableCall();
    }
    public Transform getTran()
    {
        return this.transform;
    }
}
