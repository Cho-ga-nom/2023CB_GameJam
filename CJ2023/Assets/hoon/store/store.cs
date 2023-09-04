using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class store : MonoBehaviour
{
    [Header("UI setting")]
    public Image Icon;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Content;
    public TextMeshProUGUI Cost;
    public TextMeshProUGUI NowLv;
    public Image BuyButton;
    public Sprite OnButton;
    public Sprite OffButton;
    

    public void SetStore(ItemData Item,int maxLevel, int level, bool Islevel)
    {
        Icon.sprite = Item.Icon; //아이콘
        
        //아이템 이름
        if (level == -1)
        {
            Title.text = "테이저건 잠금해제";
        }
        else
        {
            Title.text = Item.Title;
        }

        //아이템 설명
        if (level == -1)
        {
            Content.text = "테이저건을 구매합니다.\n구매시 탄환 3개가 지급됩니다.";
        }
        else
        {
            Content.text = Item.Content; 
        }

        // 가격
        if (level == -1)
        {
            Cost.text = Item.Cost[Item.Cost.Length-1].ToString() + "\n구매하기";
        }
        else
        {
            Cost.text = Item.Cost[level].ToString() + "\n구매하기";
        }

        // 개수 or 레벨
        if(level == -1)
        {
            NowLv.text = "잠겨있음";
        }
        else
        {
            string Unit = " 개";
            if (Islevel)
            {
                Unit = "LV";
            }
            NowLv.text = level.ToString() + Unit;
        }


        // 버튼 on/off
        if (level!=maxLevel)
        {
            BuyButton.sprite = OnButton;
        }
        else
        {
            BuyButton.sprite = OffButton;
        }
    }

}
