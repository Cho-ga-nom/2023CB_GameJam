using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : SingletonBase<DataManager>
{
    protected override void awakeChild()
    {
#if UNITY_EDITOR
        Time.timeScale = 5f;
#endif
        DontDestroyOnLoad(this.gameObject);
        trap.gameObject.SetActive(false);
        ropePrefab.gameObject.SetActive(false);
        trapPrefab.gameObject.SetActive(false);
        SoundDatabase.InitData();
        EffectDatabase.InitData();
        stateInit();
    }
    public void stateInit()
    {
        NowPlayerData.HpLv = 0;
        NowPlayerData.CaptureLv = 0;
        NowPlayerData.LockLv = 0;
        NowPlayerData.MeleeLv = 0;
        NowPlayerData.MoveLv = 0;

        NowPlayerData.BulietAmount = -1;
        NowPlayerData.TrapAmount = 0;
        NowPlayerData.TieAmount = 3;

        NowPlayerData.HpMaxAdd = 0;
        NowPlayerData.MeleaDamegeAdd = 0;
        NowPlayerData.MoveSpeedAdd = 0;
        NowPlayerData.LockDelayPer = 0;

        StoreData.NowCost = StoreData.BaseCost;
    }
    public void stateMax()
    {
        NowPlayerData.HpLv = MaxStateData.HpLv;
        NowPlayerData.CaptureLv = MaxStateData.CaptureLv;
        NowPlayerData.LockLv = MaxStateData.LockLv;
        NowPlayerData.MeleeLv = MaxStateData.MeleeLv;
        NowPlayerData.MoveLv = MaxStateData.MoveLv;

        NowPlayerData.BulietAmount = MaxStateData.BulietAmount;
        NowPlayerData.TrapAmount = MaxStateData.TrapAmount;
        NowPlayerData.TieAmount = MaxStateData.TieAmount;


        NowPlayerData.HpMaxAdd = MaxStateData.HpMaxAdd * MaxStateData.HpLv;
        NowPlayerData.MeleaDamegeAdd = MaxStateData.MeleaDamegeAdd * MaxStateData.MeleeLv;
        NowPlayerData.MoveSpeedAdd = MaxStateData.MoveSpeedAdd * MaxStateData.MoveLv;
        NowPlayerData.LockDelayPer = MaxStateData.LockDelayPer * MaxStateData.LockLv;
    }
    private bool isOn = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isOn = !isOn;
        }
    }
    private void OnGUI()
    {
        if (isOn)
        {
            if (GUILayout.Button("LvInit"))
            {
                stateInit();
                isOn = false;
                sceneChange();
                SceneManager.LoadScene("store");
            }
            if (GUILayout.Button("MaxLv"))
            {
                stateMax();
                isOn = false;
                sceneChange();
                SceneManager.LoadScene("store");
            }
            if (GUILayout.Button("Store"))
            {
                isOn = false;
                sceneChange();
                SceneManager.LoadScene("store");
            }
        }
    }

    public CharacterData PlayerData;
    public StateData NowPlayerData;
    public StateData MaxStateData;
    public StoreData StoreData;
    public ItemData[] ItemData;
    public CharacterData[] MobData;

    public SoundDatabase SoundDatabase;
    public EffectDatabase EffectDatabase;
    public InterObj_Item ropePrefab;
    public InterObj_Item trapPrefab;
    public Transform trap;
    public Dictionary<string, PoolSystem> effectPool = new Dictionary<string, PoolSystem>();
    public Dictionary<string, PoolSystem> soundPool = new Dictionary<string, PoolSystem>();
    public Dictionary<string, PoolSystem> itemPool = new Dictionary<string, PoolSystem>();

    public void sceneChange()
    {
        itemPool.Clear();
        effectPool.Clear();
        soundPool.Clear();
    }

    public Transform effectCall(string effecTag)
    {
        if (EffectDatabase.EffectDic.ContainsKey(effecTag) == false)
        {
            Debug.Log("없는 이팩트 테그임" + effecTag);
            return null;
        }
        if (effectPool.ContainsKey(effecTag) == false)
        {
            effectPool[effecTag] = new PoolSystem();
            effectPool[effecTag].createItem = () => { return Instantiate(EffectDatabase.EffectDic[effecTag].Effect).GetComponent<I_PoolChild>(); };
        }

        return effectPool[effecTag].newItem().getTran();
    }
    public SoundData soundCall(string effecTag, Transform audioTran)
    {
        if (SoundDatabase.AudioDic.ContainsKey(effecTag) == false)
        {
            Debug.Log("없는 사운드 테그임" + effecTag);
            return null;
        }
        if (soundPool.ContainsKey(effecTag) == false)
        {
            soundPool[effecTag] = new PoolSystem();
            soundPool[effecTag].createItem = () => { return new GameObject().AddComponent<AudioSource>().gameObject.AddComponent<PoolChild>(); };
        }
        //
        if (SoundDatabase.AudioDic[effecTag].maxCount > 0)
        {
            if (soundPool[effecTag].activeCount > SoundDatabase.AudioDic[effecTag].maxCount)
            {//최대치 넘김
                return null;
            }
        }
        PoolChild poolChild = soundPool[effecTag].newItem().getTran().GetComponent<PoolChild>();
        AudioSource audioSource = poolChild.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = SoundDatabase.AudioDic[effecTag].Clip;
        audioSource.volume = SoundDatabase.AudioDic[effecTag].Volume;
        float time = audioSource.clip.length * SoundDatabase.AudioDic[effecTag].StartTime;

        poolChild.autoTime = time * 1.1f;
        audioSource.transform.SetParent(audioTran);
        audioSource.transform.localPosition = Vector3.zero;
        audioSource.gameObject.SetActive(true);

        audioSource.Stop();
        audioSource.time = time;
        audioSource.pitch = SoundDatabase.AudioDic[effecTag].Pitch.y <= 0f ? SoundDatabase.AudioDic[effecTag].Pitch.x : UnityEngine.Random.Range(SoundDatabase.AudioDic[effecTag].Pitch.x, SoundDatabase.AudioDic[effecTag].Pitch.y);
        audioSource.Play();
        return SoundDatabase.AudioDic[effecTag];
    }

}
