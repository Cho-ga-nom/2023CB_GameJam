using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateData")]

public class StateData : ScriptableObject
{
    [Header("Inventory")]
    [Tooltip("경비봉 레벨")]            public int MeleeLv;
    [Tooltip("이동속도 레벨")]          public int MoveLv;
    [Tooltip("체력 레벨")]              public int HpLv;
    [Tooltip("잠금장치 레벨")]          public int LockLv;
    [Tooltip("보박술 레벨")]            public int CaptureLv;
    [Tooltip("테이저건 카트리지 개수")] public int BulietAmount;
    [Tooltip("포승줄 개수")]            public int TieAmount; 
    [Tooltip("함정 개수")]              public int TrapAmount;

    [Header("Upgrade value")]
    [Tooltip("경비봉 업글 차지 데미지 상승치")]    public int MeleaDamegeAdd;
    [Tooltip("회피 업글 회피 무적시간 상승치")]    public float MoveSpeedAdd; 
    [Tooltip("체젹 업글 최대치 상승치")]           public int HpMaxAdd;
    [Tooltip("포박술 업글 포박시간 상승치")]       public float CaptureTimeAdd;
    [Tooltip("잠금 업글 잠금 해제 딜레이 적용치")] public float LockDelayPer;
}
