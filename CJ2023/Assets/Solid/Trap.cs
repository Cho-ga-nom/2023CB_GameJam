using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int playerIndex = 0;
    public float range = 2.5f;
    public LayerMask layerMask;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RootCtrl rootCtrl = collision.GetComponentInParent<RootCtrl>();
        if (rootCtrl != null)
        {
            if (rootCtrl.TeamIndex != playerIndex)
            {
                PoolChild effecTran = DataManager.Instence.effectCall("trap").GetComponent<PoolChild>();
                effecTran.autoTime = 1f;
                effecTran.transform.position = this.transform.position;
                effecTran.gameObject.SetActive(true);
                Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, range, layerMask);
                for (int i = 0; i < hits.Length; i++)
                {
                    RootCtrl findCtrl = hits[i].GetComponentInParent<RootCtrl>();
                    if (findCtrl != null && findCtrl.TeamIndex != playerIndex)
                    {
                        findCtrl.stateCtrl.changeState(StateKind.Rope);
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }
}
