using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상인의 아이템의 정보클래스 -> 아이템과 갯수, 아이콘, 무한인지 아닌지 체크
/// </summary>
[System.Serializable]
public class VendorItem
{
    [SerializeField] private Item item;

    [SerializeField] private int quantity;

    [SerializeField] private bool unlimited;

    public Item MyItem { get { return item; } }

    public int MyQuantity { get { return quantity; } set{ quantity = value; } }

    public bool Unlimited { get { return unlimited; } } }
