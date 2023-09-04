using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundDatabase")]

public class SoundDatabase : ScriptableObject
{
    [Tooltip("사운드 리스트")] public List<SoundData> AudioList;
    [Tooltip("사운드 태그 호출용")] public Dictionary<string, SoundData> AudioDic;

    [Tooltip("AudioList를 AudioDic에 추가")]
    public void InitData()
    {
        AudioDic = new Dictionary<string, SoundData>();
        for (int i = 0; i < AudioList.Count; i++)
        {
            AudioDic[AudioList[i].AudioTag] = AudioList[i];
        }
    }
}
