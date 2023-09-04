using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : SingletonBase<CoolTime>
{
    protected override void awakeChild()
    {
    }

    [Header("Image")]
    public Image Time;
    public Image tieTime;
    public Image RollCoolTime;
    public Image Melee;
    public Image Trap;
    public Image Taser;
    public int angle;
    public void time(float Percent)
    {
        Time.fillAmount = Percent;
    }
    public void tieCool(float Percent)
    {
        tieTime.fillAmount = Percent;
    }
    public void RollCool(float Percent)
    {
        RollCoolTime.fillAmount = Percent;
    }

    public void MeleeCharge(float Percent)
    {
        Melee.fillAmount = Percent;
    }

    public void TrapCharge(float Percent)
    {
        Trap.fillAmount = Percent;
    }

    public void TaserCharge(float Percent)
    {
        Taser.fillAmount = Percent;
    }


}
