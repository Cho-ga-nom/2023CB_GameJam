using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData")]

[System.Serializable]
public class ItemData : ScriptableObject
{
     [Tooltip("��ǰ ���� ������ ����")] public int[] Cost;
     [Tooltip("��ǰ �̸�")]             public string Title;
     [Tooltip("��ǰ����")]              public string Content;
     [Tooltip("��ǰ �̹���")]           public Sprite Icon;
}
