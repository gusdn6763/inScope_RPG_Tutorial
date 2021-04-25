using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 상인의 상점 버튼
/// </summary>
public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;        //아이콘

    [SerializeField] private Text title;        //이름

    [SerializeField] private Text price;        //가격

    [SerializeField] private Text quantity;     //수량

    private VendorItem vendorItem;              //상인이 파는 아이템의 정보

    /// <summary>
    /// 상인의 상점창에서 아이템 추가
    /// </summary>
    /// <param name="vendorItem">상인 아이템의 정보를 받음</param>
    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;

        //아이템의 수량이 1개 이상이거나 || 아이템 갯수 제한이 없을경우  
        if (vendorItem.MyQuantity > 0 || vendorItem.Unlimited)
        {
            icon.sprite = vendorItem.MyItem.MyIcon;
            title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[vendorItem.MyItem.MyQuality], vendorItem.MyItem.MyTitle);

            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.MyQuantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }
            if (vendorItem.MyItem.MyPrice > 0)
            {
                price.text = "Price: " + vendorItem.MyItem.MyPrice.ToString();
            }
            else
            {
                price.text = string.Empty;
            }
            gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Player.instance.MyGold >= vendorItem.MyItem.MyPrice) && InventoryScript.instance.AddItem(Instantiate(vendorItem.MyItem)))
        {
            SellItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.ShowTooltip(new Vector2(0, 1), transform.position, vendorItem.MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }

    /// <summary>
    /// 아이템을 샀을경우
    /// </summary>
    private void SellItem()
    {
        Player.instance.MyGold -= vendorItem.MyItem.MyPrice;

        if (!vendorItem.Unlimited)
        {
            vendorItem.MyQuantity--;
            quantity.text = vendorItem.MyQuantity.ToString();

            if (vendorItem.MyQuantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
