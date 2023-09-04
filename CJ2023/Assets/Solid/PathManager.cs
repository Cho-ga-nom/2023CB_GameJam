using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : SingletonBase<PathManager>
{
    public List<PathField> fieldList = new List<PathField>();
    protected override void awakeChild()
    {
    }
    private void Start()
    {
        if (instence == this)
        {
            foreach (PathField field in fieldList)
            {
                PathField fieldUp = findGrid(field.grid.x, field.grid.y + 1);
                PathField fieldDown = findGrid(field.grid.x, field.grid.y - 1);
                PathField fieldRight = findGrid(field.grid.x + 1, field.grid.y);
                PathField fieldLeft = findGrid(field.grid.x - 1, field.grid.y);
                if (fieldUp != null && field.naverField.Contains(fieldUp) == false)
                {
                    field.naverField.Add(fieldUp);
                }
                if (fieldDown != null && field.naverField.Contains(fieldDown) == false)
                {
                    field.naverField.Add(fieldDown);
                }
                if (fieldRight != null && field.naverField.Contains(fieldRight) == false)
                {
                    field.naverField.Add(fieldRight);
                }
                if (fieldLeft != null && field.naverField.Contains(fieldLeft) == false)
                {
                    field.naverField.Add(fieldLeft);
                }
            }
        }
    }
    public PathField findGrid(int X, int Y)
    {
        return fieldList.Find(field => field.grid.x == X && field.grid.y == Y);
    }

    public void setField(PathField pathField)
    {
        if (fieldList.Contains(pathField) == false)
        {
            fieldList.Add(pathField);
        }
    }
    public void findPath(Vector3 worldPos, Vector3 targetPos, ref PathInfo pathInfo, bool isForce = false)
    {
        PathField startField = getContainField(worldPos);
        PathField endField = getContainField(targetPos);
        if (startField != null && endField != null && (pathInfo.lastField != endField || pathInfo.isError || isForce))
        {
            PathInfo pathTemp = new PathInfo();
            pathTemp.startField = startField;
            pathTemp.lastField = endField;
            getPath(startField, endField, ref pathTemp);
            bool isUpdate = false;
            foreach (PathField field in pathTemp.path)
            {
                if (pathInfo.path.Find(x => x == field) == null)
                {
                    isUpdate = true;
                    break;
                }
            }
            if (pathInfo.isError || isUpdate || pathInfo.startField != startField)
            {
                pathInfo.Clear();
                pathInfo.startField = startField;
                pathInfo.lastField = endField;
                pathInfo.path = pathTemp.path;
                if (pathInfo.path.Count <= 0)
                {
                    pathInfo.isError = true;
                    pathInfo.path.Add(endField);
                }
                pathInfo.offset = pathInfo.path[0].getOffset();
            }
            //else
            //{
            //    pathInfo.path.Add(startField);
            //    pathInfo.path.Add(endField);
            //}
        }
    }
    public PathField getContainField(Vector3 worldPos)
    {
        PathField find = null;
        float dis = 0f;
        for (int i = 0; i < fieldList.Count; i++)
        {
            float checkDis = Vector2.Distance(worldPos, fieldList[i].pivotTran.position);
            if (find == null || dis > checkDis)
            {
                dis = checkDis;
                find = fieldList[i];
            }
        }
        return find;
        //return fieldList.Find(field => field.containPos(worldPos));
    }
    public PathField getPath(PathField startField, PathField endField, ref PathInfo pathInfo)
    {
        PathField findField = null;
        if (startField == endField)
        {
            if (pathInfo.path.Count > 0)
            {
                pathInfo.path.Insert(0, startField);
            }
            else
            {
                pathInfo.path.Add(startField);
            }
            return endField;
        }
        if (endField.grid.x - startField.grid.x > 0)
        {//오른쪽
            findField = startField.naverField.Find(field => field.grid == (startField.grid + Vector2Int.right));
        }
        else if (endField.grid.x - startField.grid.x < 0)
        {//왼쪽
            findField = startField.naverField.Find(field => field.grid == (startField.grid - Vector2Int.right));
        }
        if (findField != null)
        {
            findField = getPath(findField, endField, ref pathInfo);
            if (findField != null)
            {
                pathInfo.path.Insert(0, startField);
                return startField;
            }
        }
        if (endField.grid.y - startField.grid.y > 0)
        {//위
            findField = startField.naverField.Find(field => field.grid == (startField.grid + Vector2Int.up));
        }
        else if (endField.grid.y - startField.grid.y < 0)
        {//아래
            findField = startField.naverField.Find(field => field.grid == (startField.grid - Vector2Int.up));
        }
        if (findField != null)
        {
            findField = getPath(findField, endField, ref pathInfo);
            if (findField != null)
            {
                pathInfo.path.Insert(0, startField);
                return startField;
            }
        }

        if (findField == null)
        {
            float disMin = 0f;
            for (int i = 0; i < startField.naverField.Count; i++)
            {
                if (startField.isError == false)
                {
                    if (PathField.RangeInEquals(startField.grid.x - 1, startField.grid.x + 1, startField.naverField[i].grid.x)
                        && PathField.RangeInEquals(startField.grid.y - 1, startField.grid.y + 1, startField.naverField[i].grid.y))
                    {
                        continue;
                    }
                }
                float dis = Vector2.Distance(startField.naverField[i].pivotTran.position, endField.pivotTran.position);
                if (findField == null || disMin > dis)
                {
                    disMin = dis;
                    findField = startField.naverField[i];
                }
            }
        }
        return findField;
    }

}
[System.Serializable]
public class PathInfo
{
    public bool isError;
    public PathField startField;
    public PathField lastField;
    public float pathDis;
    public List<PathField> path = new List<PathField>();

    public int nowPathIndex;
    public Vector3 waypointDic;
    public float waypointDis;
    public bool isArrive;
    public Vector3 offset;

    public Vector3 nextPos(Vector3 nowPos)
    {
        if (nowPathIndex < path.Count)
        {
            Transform pivotTran = path[nowPathIndex].getPivotTran(nowPos);
            Debug.DrawLine(pivotTran.position, pivotTran.position + offset, Color.red);
            nowPos.z = pivotTran.position.z;
            waypointDis = Vector3.Distance(pivotTran.position, nowPos);
            if (path[nowPathIndex].checkArrive(nowPos, offset))
            {
                offset = path[nowPathIndex].getOffset();
                ++nowPathIndex;
                if (nowPathIndex < path.Count)
                {
                    nowPos.z = pivotTran.position.z;
                    waypointDic = (pivotTran.position - nowPos).normalized;
                    waypointDis = Vector3.Distance(pivotTran.position, nowPos);
                    return pivotTran.position + offset;
                }
                else
                {
                    isArrive = true;
                }
            }
            else
            {
                waypointDic = (pivotTran.position - nowPos).normalized;
                return pivotTran.position + offset;
            }
        }
        waypointDic = Vector3.zero;
        waypointDis = 0f;
        return nowPos;
    }
    public void Clear()
    {
        isArrive = false;
        offset = waypointDic = Vector3.zero;
        waypointDis = 0f;
        startField = lastField = null;
        pathDis = nowPathIndex = 0;
        path.Clear();
    }
}