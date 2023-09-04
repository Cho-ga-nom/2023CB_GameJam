using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateData")]

public class StateData : ScriptableObject
{
    [Header("Inventory")]
    [Tooltip("���� ����")]            public int MeleeLv;
    [Tooltip("�̵��ӵ� ����")]          public int MoveLv;
    [Tooltip("ü�� ����")]              public int HpLv;
    [Tooltip("�����ġ ����")]          public int LockLv;
    [Tooltip("���ڼ� ����")]            public int CaptureLv;
    [Tooltip("�������� īƮ���� ����")] public int BulietAmount;
    [Tooltip("������ ����")]            public int TieAmount; 
    [Tooltip("���� ����")]              public int TrapAmount;

    [Header("Upgrade value")]
    [Tooltip("���� ���� ���� ������ ���ġ")]    public int MeleaDamegeAdd;
    [Tooltip("ȸ�� ���� ȸ�� �����ð� ���ġ")]    public float MoveSpeedAdd; 
    [Tooltip("ü�� ���� �ִ�ġ ���ġ")]           public int HpMaxAdd;
    [Tooltip("���ڼ� ���� ���ڽð� ���ġ")]       public float CaptureTimeAdd;
    [Tooltip("��� ���� ��� ���� ������ ����ġ")] public float LockDelayPer;
}
