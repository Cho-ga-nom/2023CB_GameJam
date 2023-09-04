using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public TextMeshProUGUI Base;
    public TextMeshProUGUI Get;
    public TextMeshProUGUI Brake;
    public TextMeshProUGUI ResultMoney;
    public TextMeshProUGUI NowMoney;
    public StoreData storeData;
    public Button button;
    public int UpSpeed = 500;
    public float WaitTime = 0.5f;
    public bool isWait = false;


    public void PrintResult(int BasePay, int GetPay, int BrakePay)
    {
        isWait = true;
        this.gameObject.SetActive(true);
        StartCoroutine(cor_PrintResult(BasePay, GetPay, BrakePay));
    }

    IEnumerator cor_PrintResult(int BasePay, int GetPay, int BrakePay)
    {
        
        NowMoney.text = storeData.NowCost.ToString() + "¿ø";
        storeData.NowCost += GetPay + BasePay - BrakePay;
        if (storeData.NowCost < 0) button.enabled = false;
        Base.enabled = false;
        Get.enabled = false;
        Brake.enabled = false;
        ResultMoney.enabled = false;



        Base.enabled = true;
        float time = 0;
        while (BasePay > time)
        {
            Base.text = Mathf.Floor(time).ToString();
            time += Time.deltaTime * UpSpeed;
            yield return new WaitForFixedUpdate();
        }
        Base.text = BasePay.ToString();

        time = 0;
        while (WaitTime > time)
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }



        Get.enabled = true;
        time = 0;
        while (GetPay > time)
        {
            Get.text = "+" + Mathf.Floor(time).ToString();
            time += Time.deltaTime * UpSpeed;
            yield return new WaitForFixedUpdate();
        }
        Get.text = "+" + GetPay.ToString();

        time = 0;
        while (WaitTime > time)
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }



        Brake.enabled = true;
        time = 0;
        while (BrakePay > time)
        {
            Brake.text = "-" + Mathf.Floor(time).ToString();
            time += Time.deltaTime * UpSpeed;
            yield return new WaitForFixedUpdate();
        }
        Brake.text = "-" + BrakePay.ToString();

        time = 0;
        while (WaitTime > time)
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }


        ResultMoney.text = (GetPay + BasePay - BrakePay).ToString();
        ResultMoney.enabled = true;

        time = 0;
        while (WaitTime > time)
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        NowMoney.text = storeData.NowCost.ToString() + "¿ø";

        if(storeData.NowCost < 0)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(GoTitle);
            button.enabled = true;
        }

        isWait = false;

    }

    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }

}
