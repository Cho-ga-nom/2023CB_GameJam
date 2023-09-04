using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_InputCtrl : InputCtrl
{
    public CSH_AttackCtrl attackCtrl;
    public bool isAbleCharge;

    private Vector2 moveDic = new Vector2();
    private Vector3 cursorPos = new Vector3();
    private float clickTime;
    private bool isAttack;
    private bool isCapture;
    private bool isTrap;
    private float reloadTime = 2f;
    private float reloadTemp;

    void Awake()
    {
        attackCtrl = GetComponent<CSH_AttackCtrl>();
        clickTime = 0;
        isAttack = false;
        isCapture = false;
        isTrap = false;
        CoolTime.Instence.TaserCharge(0f);

    }

    public override void InputUpdate()
    {
        if (reloadTemp > 0f)
        {
            reloadTemp -= Time.deltaTime;
            CoolTime.Instence.TaserCharge((reloadTemp / reloadTime));
        }
        DetectMove();
        DetectCursor();

        if (rootCtrl.inputCtrl.isLock)              // ������ �ϸ鼭 ������ ����
        {
            return;
        }

        if (isCapture)                       // �������� �� ������ ����
        {
            if (Input.GetKey(KeyCode.F))
            {
                DetectCapture();
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                DetectCaptureCancle();
                isCapture = false;
            }

            return;
        }

        if (isTrap)                              // ���� ��ġ ���� �� ������ ����
        {
            if (Input.GetKey(KeyCode.Q))
            {
                DetectInstallTrap();
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                isTrap = false;
            }

            return;
        }

        MoveUpdate();

        if (isAttack)
        {
            if (isAbleCharge)
            {
                if (!Input.GetMouseButton(1))
                {
                    if (Input.GetMouseButton(0))
                    {
                        clickTime += Time.deltaTime;

                        if (clickTime > 0.2f && attackCtrl.isChaging == false)
                        {
                            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            attackCtrl.StartChargeAttack(cursorPos);
                        }

                        attackCtrl.StayChargeAttack(clickTime);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        if (clickTime >= 0.8f)
                        {
                            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            attackCtrl.EndChargeAttack(cursorPos);
                        }
                        else
                        {
                            attackCtrl.CancleChargeAttack();
                            DetectAttack();
                        }
                    }
                }

                isAttack = false;
            }

            if (DataManager.Instence.NowPlayerData.BulietAmount > 0 && reloadTemp <= 0f)
            {
                if (Input.GetMouseButton(1))           // �������� ����
                {
                    DetectTaser();
                    attackCtrl.isTaserOn = true;

                    if (Input.GetMouseButtonDown(0))    // �������� �߻�
                    {
                        attackCtrl.isFire = true;
                    }
                }
                if (Input.GetMouseButtonUp(1))       // �������� ���� ����
                {
                    attackCtrl.TaserEnd();
                    isAttack = false;
                }
            }
            else
            {
                attackCtrl.TaserEnd();
                isAttack = false;
            }


            return;
        }

        if (rootCtrl.interCtrl.useObj != null)
        {
            if (Input.GetKeyDown(KeyCode.E))     // ���, ����
            {
                DetectPut();
            }

            return;
        }

        if (isAbleCharge)                                // ��¡ ����
        {
            if (!Input.GetMouseButton(1))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clickTime = 0;
                }
                else if (Input.GetMouseButton(0))
                {
                    isAttack = true;
                    clickTime += Time.deltaTime;

                    if (clickTime > 0.2f && attackCtrl.isChaging == false)
                    {

                        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        attackCtrl.StartChargeAttack(cursorPos);
                    }

                    attackCtrl.StayChargeAttack(clickTime);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (clickTime >= 0.8f)
                    {
                        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        attackCtrl.EndChargeAttack(cursorPos);
                    }
                    else
                    {
                        attackCtrl.CancleChargeAttack();
                        DetectAttack();
                    }
                }
            }
        }
        else                                            // �Ϲ� ����
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                isAttack = true;
                DetectAttack();
                isAttack = false;
            }
        }

        if (DataManager.Instence.NowPlayerData.BulietAmount > 0 && reloadTemp <= 0f)
        {
            if (Input.GetMouseButton(1))           // �������� ����
            {
                isAttack = true;
                DetectTaser();
                attackCtrl.isTaserOn = true;

                if (Input.GetMouseButtonDown(0))    // �������� �߻�
                {
                    attackCtrl.isFire = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))       // �������� ���� ����
        {
            attackCtrl.TaserEnd();
        }

        if (Input.GetKeyDown(KeyCode.Space))   // ȸ��
        {
            DetectEvasion();
        }

        if (Input.GetKeyDown(KeyCode.E))     // ���, ����
        {
            DetectPut();
        }

        if (Input.GetKeyDown(KeyCode.F))    // ����
        {
            isCapture = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))    // ���� ��ġ
        {
            rootCtrl.playerCtrl.craftTrapStart();
            isTrap = true;
        }
    }

    void DetectMove()
    {
        moveDic.x = Input.GetAxisRaw("Horizontal");
        moveDic.y = Input.GetAxisRaw("Vertical");
        moveDic.Normalize();
    }

    void DetectCursor()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void MoveUpdate()
    {
        rootCtrl.moveCtrl.moveUpdate(moveDic);
        rootCtrl.moveCtrl.lookUpdate(cursorPos);
    }

    void DetectAttack()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackCtrl.AttackUpdate(cursorPos);
    }

    void DetectTaser()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (attackCtrl.TaserUpdate(cursorPos))
        {
            reloadTemp = reloadTime;
            isAttack = false;
            attackCtrl.TaserEnd();
            if (DataManager.Instence.NowPlayerData.BulietAmount <= 0)
            {
            }
        }
    }

    void DetectEvasion()
    {
        // Ű���� �Է� ���� or Ŀ�� ����
        if (moveDic.magnitude < 0.1f)
        {
            moveDic = (cursorPos - rootCtrl.animaterCtrl.pivotCtrl.root.position).normalized;
        }

        rootCtrl.moveCtrl.callRoll(moveDic);
    }

    void DetectPut()
    {
        if (rootCtrl.interCtrl.selectObj != null && rootCtrl.interCtrl.selectObj.myKind == InterKind.StatueTable)
        {
            if (rootCtrl.interCtrl.useObj != null && rootCtrl.interCtrl.useObj.myKind == InterKind.Statue)
            {
                rootCtrl.interCtrl.useDown();
            }
        }
        else if (rootCtrl.interCtrl.selectObj != null && rootCtrl.interCtrl.selectObj.myKind == InterKind.Prison)
        {
            if (rootCtrl.interCtrl.useObj != null && rootCtrl.interCtrl.useObj.myKind == InterKind.Thief)
            {
                rootCtrl.interCtrl.useDown();
            }
        }
        else
        {
            rootCtrl.interCtrl.useDown();
        }
    }

    void DetectCapture()
    {
        rootCtrl.interCtrl.longStay();
    }
    void DetectCaptureCancle()
    {
        rootCtrl.interCtrl.longStay();
    }
    void DetectInstallTrap()
    {
        if (DataManager.Instence.NowPlayerData.TrapAmount > 0)
        {
            rootCtrl.playerCtrl.creaftTrapUpdate();
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cursorPos, 0.1f);
    }
}
