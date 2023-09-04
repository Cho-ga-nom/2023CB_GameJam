using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterCtrl : CtrlParent
{

    public override void initAwakeChild()
    {
    }
    public List<InterObj> interList = new List<InterObj>();
    public InterObj selectObj;
    public InterObj useObj;

    public bool useDown()
    {
        if (useObj != null)
        {
            useObj.useEnd(this);
        }
        else if (selectObj != null)
        {
            return selectObj.shortUse(this);
        }
        return false;
    }
    public bool longStay()
    {
        if (useObj != null)
        {
            //들고있을때는 아무것도 못함
        }
        else if (selectObj != null)
        {
            return selectObj.longUse(this);
        }
        return false;
    }
    public void longCancle()
    {
        if (useObj != null)
        {
            //들고있을때는 아무것도 못함
        }
        else if (selectObj != null)
        {
            selectObj.longCancle(this);
        }
    }

    void Update()
    {
        selectUpdate();
    }

    public void selectUpdate()
    {
        selectObj = null;
        if (interList != null && interList.Count > 0)
        {
            float minDis = 0f;
            interList.RemoveAll(x => x.gameObject.activeSelf == false || Vector2.Distance(x.transform.position, this.transform.position) > 10f);
            for (int i = 0; i < interList.Count; i++)
            {
                if (interList[i].isInterLock || interList[i].isNotFind || interList[i] == useObj) continue;

                float dis = Vector2.Distance(rootCtrl.animaterCtrl.pivotCtrl.root.position, interList[i].PivotTran.position);

                if (selectObj == null || minDis > dis)
                {
                    selectObj = interList[i];
                    minDis = dis;
                }
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        InterObj interObj = collision.GetComponent<InterObj>();
        if (interObj != null && interList.Contains(interObj) == false)
        {
            interList.Add(interObj);
            interObj.interEnable(this);
            selectUpdate();
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        InterObj interObj = interList.Find(x => x.collider == collision); // collision.GetComponent<InterObj>();
        if (interObj != null && interList.Contains(interObj) == true)
        {
            interList.Remove(interObj);
            interObj.interDisable(this);
            selectUpdate();
        }
    }
}
