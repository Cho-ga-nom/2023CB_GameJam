using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoreData")]


public class StoreData : ScriptableObject
{

    [Tooltip("현재 보유중인 재화량")] public int NowCost;
    [Tooltip("기본제공 재화량")]      public int BaseCost;
    [Tooltip("전시품 파손시 손해값")] public int IteBreakCost;
    [Tooltip("전시품 도난시 손해값")] public int ItemMissingCost;

}
