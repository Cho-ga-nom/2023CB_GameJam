using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueSet : MonoBehaviour
{
    public TextMeshProUGUI bulit;
    public TextMeshProUGUI trap;
    public TextMeshProUGUI tie;
    public StateData NowPlayerData;
    public StateData MaxPlayerData;

    private void Update()
    {
        SetValue();
    }

    public void SetValue()
    {

        //bulit

        if (NowPlayerData.BulietAmount == -1)
        {
            bulit.text = "잠겨있음";
        }
        else
        {
            bulit.text = NowPlayerData.BulietAmount.ToString() + "/" + MaxPlayerData.BulietAmount.ToString();
        }

        //trap
        trap.text = NowPlayerData.TrapAmount.ToString() + "/" + MaxPlayerData.TrapAmount.ToString();

        //tie
        tie.text = GameManager.Instence.playerCtrl.playerCtrl.tieAmount.ToString() + "/" + MaxPlayerData.TieAmount.ToString();

    }


}
