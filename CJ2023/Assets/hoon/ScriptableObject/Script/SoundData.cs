using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundData
{
    [Tooltip("ȣ��� �±�")] public string AudioTag;
    [Tooltip("����")] public float Volume;
    [Tooltip("�������")] public float StartTime;
    [Tooltip("��ġ")] public Vector2 Pitch;
    [Tooltip("���� Ŭ��")] public AudioClip Clip;
    [Tooltip("0�� �ƴϸ� �ִ� ���� ����")] public int maxCount;

}
