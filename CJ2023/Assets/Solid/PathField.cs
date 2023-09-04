using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathField : MonoBehaviour
{
    public static bool RangeInEquals(float start, float end, float value)
    {
        return start <= value && value <= end;
    }
    public static bool RangeInEquals(int start, int end, int value)
    {
        return start <= value && value <= end;
    }
    public bool isError;
    public Vector2Int grid;
    public Vector2 size = Vector2.one;
    public float range = 0.5f;
    public Transform pivotTran;
    public Transform[] pivotTrans;
    public List<PathField> naverField = new List<PathField>();
    private float checkDis;
    private Vector2 realSize;
    private void Awake()
    {
        List<PathField> copy = naverField.ConvertAll(x => x);
        foreach (PathField field in copy)
        {
            if (field.naverField.Contains(this) == false)
            {
                field.naverField.Add(this);
            }
        }
        pivotTran = this.transform;
        PathManager.Instence.setField(this);
        Vector2 realSize = (size * 0.5f) * pivotTran.localScale;
        checkDis = realSize.magnitude * range * (Mathf.Min(realSize.x, realSize.y) / Mathf.Max(realSize.x, realSize.y));
    }
    public bool containPos(Vector3 worldPos)
    {
        if (RangeInEquals(pivotTran.position.x - ((size.x * 0.5f) * pivotTran.localScale.x), pivotTran.position.x + ((size.x * 0.5f) * pivotTran.localScale.x), worldPos.x))
        {
            if (RangeInEquals(pivotTran.position.y - ((size.y * 0.5f) * pivotTran.localScale.y), pivotTran.position.y + ((size.y * 0.5f) * pivotTran.localScale.y), worldPos.y))
            {
                Debug.Log(this.name + "¹üÀ§ µé¾î¿È " + worldPos);
                return true;
            }
        }
        return false;
    }
    public bool checkArrive(Vector3 worldPos, Vector3 offset)
    {
        Transform target = getPivotTran(worldPos);
        worldPos.z = target.position.z;
        float dis = Vector2.Distance(target.position + offset, worldPos);
        if (dis < 1f)
        {
            //Debug.Log("µµÂøÀÓ");
            return true;
        }
        return false;
    }

    public Vector3 getOffset()
    {
        return new Vector3(UnityEngine.Random.Range(-checkDis, checkDis), UnityEngine.Random.Range(-checkDis, checkDis), 0f) * UnityEngine.Random.Range(0f, 0.9f);
    }

    private void OnDrawGizmosSelected()
    {
        if (this.pivotTran != null)
        {
            Gizmos.DrawWireSphere(this.pivotTran.position, checkDis);
        }
    }

    public Transform getPivotTran(Vector3 nowPos)
    {
        if (pivotTrans != null && pivotTrans.Length > 0)
        {
            float mindDis = 0f;
            Transform resolt = null;
            for (int i = 0; i < pivotTrans.Length; i++)
            {
                float dis = Vector2.Distance(nowPos, pivotTrans[i].position);
                if (resolt == null || mindDis > dis)
                {
                    resolt = pivotTrans[i];
                    mindDis = dis;
                }
            }
            return resolt;
        }
        else
        {
            return pivotTran;
        }
    }
}
