using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonBase<SpawnManager>
{
    [SerializeField]
    public List<Transform> spawnPoints = new List<Transform>();

    [SerializeField]
    public List<CustomDic<string, PoolCtrl>> mobPrefabDic;
    private Dictionary<string, PoolSystem> poolDic = new Dictionary<string, PoolSystem>();
    public int spawnNow;
    public int spawnMax = 10;
    public bool IsMax => spawnNow >= spawnMax;
    public int heroCount = 1;
    public string hero;
    protected override void awakeChild()
    {
        if (spawnPoints.Count <= 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                spawnPoints.Add(this.transform.GetChild(i));
            }
        }
        foreach (CustomDic<string, PoolCtrl> prefab in mobPrefabDic)
        {
            if (poolDic.ContainsKey(prefab.key) == false)
            {

                poolDic[prefab.key] = new PoolSystem() { createItem = () => { return Instantiate(prefab.value, this.transform).GetComponent<I_PoolChild>(); } };
            }
            prefab.value.gameObject.SetActive(false);
        }
        hero = mobPrefabDic[Random.Range(0, mobPrefabDic.Count)].key;
    }
    public void swaponCall()
    {
        CustomDic<string, PoolCtrl> select = mobPrefabDic[Random.Range(0, mobPrefabDic.Count)];
        swaponCall(select.key);
        //prefab =
    }
    public void swaponCallHero()
    {
        PoolSystem pool = poolDic[hero];
        PoolCtrl newItem = pool.newItem() as PoolCtrl;
        if (hero.Equals("Tech"))
        {
            Transform statueTran = GameManager.instence.statueList[Random.Range(0, GameManager.instence.statueList.Count)].transform;
            PathField findField = PathManager.instence.getContainField(statueTran.position);
            newItem.setSpawnPivot(findField.getOffset() + findField.pivotTran.position);
        }
        else
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            newItem.setSpawnPivot(spawnPoint.position);
        }
        ++heroCount;
        RootCtrl rootCtrl = newItem.GetComponentInParent<RootCtrl>(true);
        rootCtrl.init();
        rootCtrl.hpCtrl.maxHp += (heroCount * rootCtrl.hpCtrl.maxHp) / 2;
        rootCtrl.hpCtrl.repairHp();
        rootCtrl.transform.localScale = Vector3.one * 1.7f;
        newItem.gameObject.SetActive(true);
    }
    public void swaponCall(string v)
    {
        if (IsMax) return;

        ++spawnNow;
        PoolSystem pool = poolDic[v];
        PoolCtrl newItem = pool.newItem() as PoolCtrl;
        if (v.Equals("Tech"))
        {
            Transform statueTran = GameManager.instence.statueList[Random.Range(0, GameManager.instence.statueList.Count)].transform;
            PathField findField = PathManager.instence.getContainField(statueTran.position);
            newItem.setSpawnPivot(findField.getOffset() + findField.pivotTran.position);
        }
        else
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            newItem.setSpawnPivot(spawnPoint.position);
        }
        newItem.transform.localScale = Vector3.one;
        newItem.gameObject.SetActive(true);
        //prefab =
    }
}
