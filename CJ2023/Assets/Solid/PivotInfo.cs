using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotInfo : MonoBehaviour
{
    public Transform root;
    public Transform Head;
    public Transform Body;
    public Transform Arm_L;
    public Transform Arm_R;
    public Transform Foot_L;
    public Transform Foot_R;

    public Transform CarryPivot;

    private void Awake()
    {
        
        if (CarryPivot == null)
        {
            CarryPivot = Body;
        }
    }
    public System.Action<string> eventCall;
    public void callEvent(string eventTag)
    {
        eventCall?.Invoke(eventTag);
    }
}
