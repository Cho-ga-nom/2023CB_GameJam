using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public static LayerMask hitCheckLayer;

    public RootCtrl playerCtrl;

    [SerializeField]
    public List<InterObj_StatueTable> statueList = new List<InterObj_StatueTable>();


    protected override void awakeChild()
    {
        statueList = new List<InterObj_StatueTable>();
        roundTemp = roundTime;
        //isLife = true;
        hitCheckLayer = (1 << LayerMask.NameToLayer("HitZone")) + (1 << LayerMask.NameToLayer("Wall"));// + (1 << LayerMask.NameToLayer("Defualt"));
    }

    public float roundTime = 180f;
    public float roundTemp;

    [SerializeField]
    private Vector2 spawnTime = new Vector2(1f, 3f);
    private float spawnTemp;
    private float baseTemp;
    private float heroTemp = 30f;
    public bool isLife;
    public Result Result;
    private void Update()
    {
        if (isLife == false) return;
        if (roundTemp > 0f)
        {
            CoolTime.instence.time(1f - (roundTemp / roundTime));
            roundTemp -= Time.deltaTime;
            spawnTemp -= Time.deltaTime;
            baseTemp -= Time.deltaTime;
            heroTemp -= Time.deltaTime;
            if (heroTemp <= 0f)
            {
                heroTemp = UnityEngine.Random.Range(10f, 20f);
                SpawnManager.instence.swaponCallHero();
            }
            if (baseTemp <= 0f)
            {
                baseTemp = 10f;
                SpawnManager.Instence.swaponCall("Base");
            }
            if (roundTemp <= 0)
            {
                //Todo 라운드 엔드 결과창 호출
                playerCtrl.inputCtrl.isLock = true;
                int badCost = 0;
                for (int i = 0; i < statueList.Count; i++)
                {
                    if (statueList[i].isTheft)
                    {
                        badCost += 1000;
                    }
                    else if (statueList[i].isItemOn == false)
                    {
                        badCost += 500;
                    }
                }
                Result?.PrintResult(1000, addCost, badCost);
                return;
            }
            if (spawnTemp <= 0)
            {
                spawnTemp = UnityEngine.Random.Range(spawnTime.x, spawnTime.y);
                SpawnManager.Instence.swaponCall();
            }
        }
    }

    private int addCost;
    public void scoreUp(InterObj_Thief thief)
    {
        SpawnManager.instence.spawnNow -= 1;
        //스코어 업
        thief.rootCtrl.stateCtrl.isLock = true;
        addCost += thief.rootCtrl.targetCtrl.charaData.Cost;
    }
    public void scoreDown()
    {
        SpawnManager.instence.spawnNow -= 1;
    }
}
