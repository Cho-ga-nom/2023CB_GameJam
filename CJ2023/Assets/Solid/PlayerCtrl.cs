using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : CtrlParent
{
    public Transform selectIcon;
    public int tieAmount;
    public float trapTime = 0.3f;
    public float trapTemp;
    private float footTemp;
    public override void initAwakeChild()
    {
        selectIcon.SetParent(null);
        selectIcon.gameObject.SetActive(false);
        tieAmount = DataManager.Instence.NowPlayerData.TieAmount;
        GameManager.Instence.playerCtrl = rootCtrl;
    }
    public override void initStart()
    {
        rootCtrl.moveCtrl.moveSpeed = DataManager.Instence.PlayerData.Speed;
        rootCtrl.hpCtrl.maxHp += DataManager.Instence.NowPlayerData.HpMaxAdd;
        base.initStart();
    }
    private void Update()
    {
        if (rootCtrl.interCtrl.selectObj != null && rootCtrl.interCtrl.selectObj.isInterLock == false)
        {
            if (rootCtrl.interCtrl.useObj == null || rootCtrl.interCtrl.useObj.checkInterKind(rootCtrl.interCtrl.selectObj))
            {
                selectIcon.gameObject.SetActive(true);
                selectIcon.position = rootCtrl.interCtrl.selectObj.PivotTran.position + (Vector3.up * 2f);
            }
            else
            {
                selectIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            selectIcon.gameObject.SetActive(false);
        }
        if (rootCtrl.moveCtrl.moveDis > 0.1f)
        {
            footTemp -= Time.deltaTime;
            if (footTemp > 0f)
            {
                return;
            }
            if (rootCtrl.moveCtrl.isRun)
            {
                footTemp = DataManager.Instence.soundCall("Run", this.transform).Clip.length;
            }
            else
            {
                footTemp = DataManager.Instence.soundCall("Walk", this.transform).Clip.length;
            }
        }
    }
    public void craftTrapStart()
    {
        trapTemp = trapTime;
    }
    public void creaftTrapUpdate()
    {
        if (trapTemp > 0f)
        {
            CoolTime.Instence.TrapCharge(1f - (trapTemp / trapTime));
            trapTemp -= Time.deltaTime;
            if (trapTemp <= 0f)
            {
                DataManager.Instence.NowPlayerData.TrapAmount--;
                //Æ®·¦ »ý¼º
                Transform trapTran = Instantiate(DataManager.Instence.trap);
                trapTran.position = this.rootCtrl.transform.position;
                trapTran.gameObject.SetActive(true);

            }
        }
    }
}
