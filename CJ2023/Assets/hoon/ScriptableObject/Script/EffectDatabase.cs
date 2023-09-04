using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EffectDatabase")]

public class EffectDatabase : ScriptableObject
{
    [Tooltip("이펙트 리스트")] public List<EffectData> EffectList;
    [Tooltip("이펙트 태그 호출용")] public Dictionary<string, EffectData> EffectDic;

    [Tooltip("EffectList를 EffectDic에 추가")]
    public void InitData()
    {
        EffectDic = new Dictionary<string, EffectData>();
        for (int i = 0; i < EffectList.Count; i++)
        {
            EffectDic[EffectList[i].EffectTag] = EffectList[i];
            EffectList[i].Effect.gameObject.SetActive(false);
        }
    }
}
