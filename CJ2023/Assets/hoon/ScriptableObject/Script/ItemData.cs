using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData")]

[System.Serializable]
public class ItemData : ScriptableObject
{
     [Tooltip("상품 구매 레벨당 가격")] public int[] Cost;
     [Tooltip("상품 이름")]             public string Title;
     [Tooltip("상품설명")]              public string Content;
     [Tooltip("상품 이미지")]           public Sprite Icon;
}
