using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData")]

[System.Serializable]
public class CharacterData : ScriptableObject
{
    [Header("Base")]
    [Tooltip("체력,0이 될시 사망")] public int MaxHP;
    [Tooltip("x=걷기,y=달리기")] public Vector2 Speed;
    [Tooltip("기절유지시간")] public float StunTime;
    [Tooltip("포박유지,(플레이어=포박 행동시간 0.2s)")] public float TieTime;
    [Tooltip("다음공격까지 입력 대기시간,(기술형=잠금해제 시간)")] public float AttackDelay;

    [Header("Player")]
    [Tooltip("회피 입력 대기시간")] public float RollDelay;
    [Tooltip("회피시 이동거리")] public float RollRange;
    [Tooltip("회피 유지 시간")] public float RollTime;
    [Tooltip("차징 시간")] public float ChargeingTime;

    [Header("Mob")]
    [Tooltip("체포수당")] public int Cost;
}
