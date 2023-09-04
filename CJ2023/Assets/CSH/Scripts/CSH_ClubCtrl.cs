using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_ClubCtrl : MonoBehaviour
{
    public Action actionEnd;
    public HitChecker hitChecker;
    public Transform pivotTran;

    public void SwingEnd()
    {
        actionEnd?.Invoke();
        this.gameObject.SetActive(false);
    }
}
