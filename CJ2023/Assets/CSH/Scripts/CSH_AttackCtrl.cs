using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_AttackCtrl : CtrlParent
{
    public CSH_ClubCtrl ClubPrefab, FullChargeClubPrefab;
    public CSH_TaserCtrl TaserPrefab;
    public CSH_ChargeClubCtrl ChargeClubPrefab;
    public GameObject BulletPrefab;

    public bool isChaging;
    public bool isTaserOn = false;
    public bool isFire = false;
    public float BulletSpeed = 15.0f;

    private PoolSystem pool;

    GameObject leftArm => rootCtrl.animaterCtrl.pivotCtrl.Arm_L.gameObject;
    CSH_ClubCtrl clubArm, fullChargeClubArm;
    CSH_TaserCtrl taserArm;
    CSH_ChargeClubCtrl chargeClubArm;

    public override void initAwakeChild()
    {
        CoolTime.Instence.MeleeCharge(0f);

        pool = new PoolSystem() { createItem = () => { return Instantiate(BulletPrefab).GetComponent<I_PoolChild>(); } };

        clubArm = Instantiate(ClubPrefab, leftArm.transform.position, leftArm.transform.rotation);
        clubArm.transform.SetParent(rootCtrl.animaterCtrl.pivotCtrl.Body);
        clubArm.transform.SetPositionAndRotation(leftArm.transform.position, leftArm.transform.rotation);
        clubArm.transform.SetParent(rootCtrl.transform);
        clubArm.gameObject.SetActive(false);

        taserArm = Instantiate(TaserPrefab, leftArm.transform.position, leftArm.transform.rotation);
        taserArm.transform.SetParent(rootCtrl.animaterCtrl.pivotCtrl.Body);
        taserArm.transform.SetLocalPositionAndRotation(leftArm.transform.position, leftArm.transform.rotation);
        taserArm.RootCtrl = rootCtrl;
        taserArm.gameObject.SetActive(false);
        taserArm.transform.SetParent(rootCtrl.transform);
        taserArm.actionEnd = () => { rootCtrl.animaterCtrl.pivotCtrl.Arm_L.gameObject.SetActive(true); };

        chargeClubArm = Instantiate(ChargeClubPrefab, leftArm.transform.position, leftArm.transform.rotation);
        chargeClubArm.transform.SetParent(rootCtrl.animaterCtrl.pivotCtrl.Body);
        chargeClubArm.transform.SetLocalPositionAndRotation(leftArm.transform.position, leftArm.transform.rotation);
        chargeClubArm.gameObject.SetActive(false);

        fullChargeClubArm = Instantiate(FullChargeClubPrefab, leftArm.transform.position, leftArm.transform.rotation);
        fullChargeClubArm.transform.SetParent(rootCtrl.animaterCtrl.pivotCtrl.Body);
        fullChargeClubArm.transform.SetLocalPositionAndRotation(leftArm.transform.position, leftArm.transform.rotation);
        fullChargeClubArm.gameObject.SetActive(false);

    }

    public void AttackUpdate(Vector2 cursorPos)
    {
        leftArm.SetActive(false);

        clubArm.actionEnd = () => { rootCtrl.animaterCtrl.pivotCtrl.Arm_L.gameObject.SetActive(true); };
        clubArm.transform.position = leftArm.transform.position;
        clubArm.hitChecker.collider.enabled = false;
        clubArm.gameObject.SetActive(true);
        clubArm.hitChecker.rootCtrl = rootCtrl;
        DataManager.Instence.soundCall("ClubSwing", this.transform);

        Vector2 moveDic = (cursorPos - (Vector2)clubArm.transform.position).normalized;
        float angle = Vector3.Angle(Vector3.up, moveDic);
        clubArm.pivotTran.localScale = (moveDic.x < 0 ? MoveCtrl.left : MoveCtrl.right);
        clubArm.transform.eulerAngles = angle * Vector3.forward * (moveDic.x < 0 ? 1f : -1f);


    }

    public bool TaserUpdate(Vector2 cursorPos)
    {
        leftArm.SetActive(false);

        taserArm.transform.position = leftArm.transform.position;//, leftArm.transform.rotation);
        taserArm.gameObject.SetActive(true);

        //float angle = Mathf.Atan2(cursorPos.y - rootCtrl.transform.position.y, cursorPos.x - rootCtrl.transform.position.x) * Mathf.Rad2Deg;
        Vector2 moveDic = (cursorPos - (Vector2)taserArm.transform.position).normalized;
        float angle = Vector3.Angle(Vector3.up, moveDic);
        taserArm.pivotTran.localScale = (moveDic.x < 0 ? MoveCtrl.left : MoveCtrl.right);
        taserArm.transform.eulerAngles = angle * Vector3.forward * (moveDic.x < 0 ? 1f : -1f);
        //taserArm.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //taserArm.transform.eulerAngles = Vector3.forward * SolidAngle.getAngle(taserArm.transform.position, cursorPos, AxisType.TwoD);

        if (isTaserOn)
        {
            taserArm.Aim();

            if (isFire)
            {
                if (DataManager.Instence.NowPlayerData.BulietAmount > 0)
                {
                    DataManager.Instence.NowPlayerData.BulietAmount -= 1;
                    DataManager.Instence.soundCall("TaserGunShot", this.transform);
                    Transform effectTran = DataManager.Instence.effectCall("Electric");

                    // 총알 생성
                    Transform firePivot = taserArm.FirePivot;
                    effectTran.position = firePivot.position;
                    effectTran.gameObject.SetActive(true);
                    Vector2 startPos = new Vector2(firePivot.position.x, firePivot.position.y);

                    HitBullet bullet = (pool.newItem() as HitBullet); //Instantiate(BulletPrefab, startPos, transform.rotation);
                    bullet.rootCtrl = this.rootCtrl;
                    bullet.transform.position = startPos;
                    bullet.transform.rotation = transform.rotation;
                    bullet.gameObject.SetActive(true);

                    // 총알 발사
                    Vector2 shotDir = taserArm.FirePivot.up;
                    bullet.GetComponent<HitBullet>().setBulletInfo(shotDir, BulletSpeed);
                    isFire = false;

                    if (DataManager.Instence.NowPlayerData.BulietAmount <= 0)
                    {
                        TaserEnd();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public void TaserEnd()
    {
        taserArm.TaserEnd();
    }

    public void StartChargeAttack(Vector3 cursorPos)
    {
        isChaging = true;
        leftArm.SetActive(false);
        rootCtrl.animaterCtrl.pivotCtrl.Arm_L.gameObject.SetActive(false);

        DataManager.Instence.soundCall("Charging", this.transform);
        chargeClubArm.gameObject.SetActive(true);
        chargeClubArm.transform.SetPositionAndRotation(leftArm.transform.position, leftArm.transform.rotation);

        float angle = Mathf.Atan2(cursorPos.y - rootCtrl.transform.position.y, cursorPos.x - rootCtrl.transform.position.x) * Mathf.Rad2Deg;
        chargeClubArm.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void StayChargeAttack(float clickTime)
    {
        leftArm.SetActive(false);
        CoolTime.Instence.MeleeCharge(clickTime/0.8f);
        chargeClubArm.GetComponent<Animator>().SetFloat("Charging", Mathf.Min(clickTime / 0.8f, 1.0f));
    }

    public void EndChargeAttack(Vector2 cursorPos)
    {
        CoolTime.Instence.MeleeCharge(0f);

        isChaging = false;
        fullChargeClubArm.actionEnd = () => { rootCtrl.animaterCtrl.pivotCtrl.Arm_L.gameObject.SetActive(true); };
        fullChargeClubArm.transform.position = leftArm.transform.position;
        fullChargeClubArm.hitChecker.damage = 3 + DataManager.Instence.NowPlayerData.MeleaDamegeAdd;
        fullChargeClubArm.hitChecker.collider.enabled = false;
        fullChargeClubArm.gameObject.SetActive(true);
        fullChargeClubArm.hitChecker.rootCtrl = rootCtrl;
        DataManager.Instence.soundCall("ClubSwing", this.transform);

        chargeClubArm.gameObject.SetActive(false);

        Vector2 moveDic = (cursorPos - (Vector2)chargeClubArm.transform.position).normalized;
        float angle = Vector3.Angle(Vector3.up, moveDic);
        fullChargeClubArm.pivotTran.localScale = (moveDic.x < 0 ? MoveCtrl.left : MoveCtrl.right);
        fullChargeClubArm.transform.eulerAngles = angle * Vector3.forward * (moveDic.x < 0 ? 1f : -1f);




    }

    public void CancleChargeAttack()
    {
        CoolTime.Instence.MeleeCharge(0f);
        isChaging = false;
        chargeClubArm.gameObject.SetActive(false);
        rootCtrl.animaterCtrl.pivotCtrl.Arm_L.gameObject.SetActive(true);
    }
}
