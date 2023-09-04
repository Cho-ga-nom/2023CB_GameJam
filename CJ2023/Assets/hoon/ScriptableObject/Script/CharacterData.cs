using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData")]

[System.Serializable]
public class CharacterData : ScriptableObject
{
    [Header("Base")]
    [Tooltip("ü��,0�� �ɽ� ���")] public int MaxHP;
    [Tooltip("x=�ȱ�,y=�޸���")] public Vector2 Speed;
    [Tooltip("���������ð�")] public float StunTime;
    [Tooltip("��������,(�÷��̾�=���� �ൿ�ð� 0.2s)")] public float TieTime;
    [Tooltip("�������ݱ��� �Է� ���ð�,(�����=������� �ð�)")] public float AttackDelay;

    [Header("Player")]
    [Tooltip("ȸ�� �Է� ���ð�")] public float RollDelay;
    [Tooltip("ȸ�ǽ� �̵��Ÿ�")] public float RollRange;
    [Tooltip("ȸ�� ���� �ð�")] public float RollTime;
    [Tooltip("��¡ �ð�")] public float ChargeingTime;

    [Header("Mob")]
    [Tooltip("ü������")] public int Cost;
}
