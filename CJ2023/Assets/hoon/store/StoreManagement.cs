using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class StoreManagement : MonoBehaviour
{
    [Header("Melee")]
    public ItemData Melee;
    public store MeleeStore;
    [Header("Move")]
    public ItemData Move;
    public store MoveStore;
    [Header("Hp")]
    public ItemData Hp;
    public store HpStore;
    [Header("Lock")]
    public ItemData Lock;
    public store LockStore;
    [Header("Capture")]
    public ItemData Capture;
    public store CaptureStore;
    [Header("Bulit")]
    public ItemData Bulit;
    public store BulitStore;
    [Header("Tie")]
    public ItemData Tie;
    public store TieStore;
    [Header("Trap")]
    public ItemData Trap;
    public store TrapStore;
    [Header("Cost")]
    public TextMeshProUGUI Cost;
    public TextMeshProUGUI Message;
    public TextMeshProUGUI FullMessage;
    [Header("PlayerData")]
    public StateData NowPlayerData;
    public StateData MaxPlayerData;
    public StoreData StoreData;

    private void Start()
    {
        Load();
    }
    public void Load()
    {
        //UI����
        MeleeStore.SetStore(Melee, MaxPlayerData.MeleeLv, NowPlayerData.MeleeLv, true);
        MoveStore.SetStore(Move, MaxPlayerData.MoveLv, NowPlayerData.MoveLv, true);
        HpStore.SetStore(Hp, MaxPlayerData.HpLv, NowPlayerData.HpLv, true);
        LockStore.SetStore(Lock, MaxPlayerData.LockLv, NowPlayerData.LockLv, true);
        CaptureStore.SetStore(Capture, MaxPlayerData.CaptureLv, NowPlayerData.CaptureLv, true);
        BulitStore.SetStore(Bulit, MaxPlayerData.BulietAmount, NowPlayerData.BulietAmount, false);
        TieStore.SetStore(Tie, MaxPlayerData.TieAmount, NowPlayerData.TieAmount, false);
        TrapStore.SetStore(Trap, MaxPlayerData.TrapAmount, NowPlayerData.TrapAmount, false);
        Cost.text = StoreData.NowCost.ToString() + "��";
    }

    public void buy(int Slot)
    {
        if (Slot == 1) { Levelup(ref NowPlayerData.TieAmount, ref MaxPlayerData.TieAmount, Tie); } // ������
        if (Slot == 2) { Levelup(ref NowPlayerData.BulietAmount, ref MaxPlayerData.BulietAmount, Bulit); } // ��������
        if (Slot == 3) { Levelup(ref NowPlayerData.TrapAmount, ref MaxPlayerData.TrapAmount, Trap); } // ����

        //���� ����
        if (Slot == 4) { Levelup(ref NowPlayerData.LockLv, ref MaxPlayerData.LockLv, Lock); } // �����ġ
        if (Slot == 5) { Levelup(ref NowPlayerData.HpLv, ref MaxPlayerData.HpLv, Hp); } // HP
        if (Slot == 6) { Levelup(ref NowPlayerData.MoveLv, ref MaxPlayerData.MoveLv, Move); } // �̵��ӵ�
        if (Slot == 7) { Levelup(ref NowPlayerData.CaptureLv, ref MaxPlayerData.CaptureLv, Capture); } // ���ڽð�
        if (Slot == 8) { Levelup(ref NowPlayerData.MeleeLv, ref MaxPlayerData.MeleeLv, Melee); } // ��¡������

        //������ ����
        if (Slot == 4) { NowPlayerData.LockDelayPer = NowPlayerData.LockLv * MaxPlayerData.LockDelayPer; } // �����ġ
        if (Slot == 5) { NowPlayerData.HpMaxAdd = NowPlayerData.HpLv * MaxPlayerData.HpMaxAdd; } // HP
        if (Slot == 6) { NowPlayerData.MoveSpeedAdd = NowPlayerData.MoveLv * MaxPlayerData.MoveSpeedAdd; } // �̵��ӵ�
        if (Slot == 7) { NowPlayerData.CaptureTimeAdd = NowPlayerData.CaptureLv * MaxPlayerData.CaptureTimeAdd; } // ���ڽð�
        if (Slot == 8) { NowPlayerData.MeleaDamegeAdd = NowPlayerData.MeleeLv * MaxPlayerData.MeleaDamegeAdd; } // ��¡������
        Load();

    }

    public void Levelup(ref int nowlevel, ref int maxlevel, ItemData item)
    {
        if (nowlevel < maxlevel && nowlevel != -1)
        {
            if (StoreData.NowCost < item.Cost[nowlevel]) StartCoroutine(MessageFade());
            else
            {
                StoreData.NowCost -= item.Cost[nowlevel];
                nowlevel += 1;
            }
        }
        if (nowlevel < maxlevel && nowlevel == -1)
        {
            if (StoreData.NowCost < item.Cost[item.Cost.Length - 1]) StartCoroutine(MessageFade());
            else
            {
                StoreData.NowCost -= item.Cost[item.Cost.Length - 1];
                nowlevel += 4;
            }
        }

        if (nowlevel >= maxlevel) StartCoroutine(FullMessageFade());
        NowPlayerData.MeleaDamegeAdd = NowPlayerData.MeleeLv * MaxPlayerData.MeleaDamegeAdd;
    }


    private void OnLevelWasLoaded(int level)
    {
        Load();
    }
    [SerializeField]
    private string[] sceneNames;
    public void Next(string index)
    {
        index = sceneNames[Random.Range(0, sceneNames.Length)];
        DataManager.Instence.sceneChange();
        SceneManager.LoadScene(index);
    }

    IEnumerator MessageFade()
    {
        float fadeCount = 1;
        while (fadeCount > 0)
        {
            fadeCount -= 0.005f;
            yield return new WaitForSeconds(0.01f);
            Message.color = new Color(0.8584906f, 0.2543076f, 0.2543076f, fadeCount);
        }
    }

    IEnumerator FullMessageFade()
    {
        float fadeCount = 1;
        while (fadeCount > 0)
        {
            fadeCount -= 0.005f;
            yield return new WaitForSeconds(0.01f);
            FullMessage.color = new Color(1, 0.409434f, 0.9118285f, fadeCount);
        }
    }

}
