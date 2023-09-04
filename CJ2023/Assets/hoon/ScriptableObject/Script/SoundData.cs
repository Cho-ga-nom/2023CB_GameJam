using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundData
{
    [Tooltip("호출용 태그")] public string AudioTag;
    [Tooltip("볼륨")] public float Volume;
    [Tooltip("재생시작")] public float StartTime;
    [Tooltip("피치")] public Vector2 Pitch;
    [Tooltip("사운드 클립")] public AudioClip Clip;
    [Tooltip("0이 아니면 최대 사운드 개수")] public int maxCount;

}
