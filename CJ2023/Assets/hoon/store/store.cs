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
        Icon.sprite = Item.Icon; //������
        
        //������ �̸�
        if (level == -1)
        {
            Title.text = "�������� �������";
        }
        else
        {
            Title.text = Item.Title;
        }

        //������ ����
        if (level == -1)
        {
            Content.text = "���������� �����մϴ�.\n���Ž� źȯ 3���� ���޵˴ϴ�.";
        }
        else
        {
            Content.text = Item.Content; 
        }

        // ����
        if (level == -1)
        {
            Cost.text = Item.Cost[Item.Cost.Length-1].ToString() + "\n�����ϱ�";
        }
        else
        {
            Cost.text = Item.Cost[level].ToString() + "\n�����ϱ�";
        }

        // ���� or ����
        if(level == -1)
        {
            NowLv.text = "�������";
        }
        else
        {
            string Unit = " ��";
            if (Islevel)
            {
                Unit = "LV";
            }
            NowLv.text = level.ToString() + Unit;
        }


        // ��ư on/off
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
