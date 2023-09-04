using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitChecker : CtrlParent
{
    public string soundTag = "ClubSwing";
    public int damage = 1;
    public Collider2D collider;

    public override void initAwakeChild()
    {
    }


    public bool checkTarget(Transform targetTran, Vector3 pos)
    {
        HitZone hitZone = targetTran.GetComponent<HitZone>();
        if (hitZone != null)
        {
            if (hitZone.rootCtrl.TeamIndex != rootCtrl.TeamIndex && hitZone.rootCtrl.moveCtrl.isRoll == false)
            {
                DataManager.Instence.soundCall(soundTag, this.transform);
                hitZone.rootCtrl.hpCtrl.sendDamage(damage, pos);
                return true;
            }
            return false;
        }
        return true;
    }

}
