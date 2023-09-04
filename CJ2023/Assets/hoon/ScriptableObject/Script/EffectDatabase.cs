using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EffectDatabase")]

public class EffectDatabase : ScriptableObject
{
    [Tooltip("����Ʈ ����Ʈ")] public List<EffectData> EffectList;
    [Tooltip("����Ʈ �±� ȣ���")] public Dictionary<string, EffectData> EffectDic;

    [Tooltip("EffectList�� EffectDic�� �߰�")]
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
