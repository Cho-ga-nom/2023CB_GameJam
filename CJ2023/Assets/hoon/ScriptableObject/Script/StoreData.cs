using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoreData")]


public class StoreData : ScriptableObject
{

    [Tooltip("���� �������� ��ȭ��")] public int NowCost;
    [Tooltip("�⺻���� ��ȭ��")]      public int BaseCost;
    [Tooltip("����ǰ �ļս� ���ذ�")] public int IteBreakCost;
    [Tooltip("����ǰ ������ ���ذ�")] public int ItemMissingCost;

}
