using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundDatabase")]

public class SoundDatabase : ScriptableObject
{
    [Tooltip("���� ����Ʈ")] public List<SoundData> AudioList;
    [Tooltip("���� �±� ȣ���")] public Dictionary<string, SoundData> AudioDic;

    [Tooltip("AudioList�� AudioDic�� �߰�")]
    public void InitData()
    {
        AudioDic = new Dictionary<string, SoundData>();
        for (int i = 0; i < AudioList.Count; i++)
        {
            AudioDic[AudioList[i].AudioTag] = AudioList[i];
        }
    }
}
