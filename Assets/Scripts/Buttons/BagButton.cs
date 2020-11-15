using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;

    [SerializeField] private Sprite full, empty;
    [SerializeField] private int bagIndex;

    public Bag MyBag { get { return bag; }
        set
        {
            // 백 버튼에 가방이 등록되어있는지 아닌지 체크
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }

            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    public int BagIndex { get => bagIndex; set => bagIndex = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //무언가를 선택한 상태이고, 아이템을 선택하고, 가방을 선택시
            if (InventoryScript.instance.ChoosedSlot != null && HandScript.instance.Dragable != null && HandScript.instance.Dragable is Bag)
            {
                if (MyBag != null)
                {
                    InventoryScript.instance.SwapBags(MyBag, HandScript.instance.Dragable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.instance.Dragable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.instance.Drop();
                    InventoryScript.instance.ChoosedSlot = null;
                }
            }
            if (Input.GetKey(KeyCode.LeftShift) && bag != null)
            {
                HandScript.instance.TakeMoveable(MyBag);
            }
        }
    }

    public void RemoveBag()
    {
        InventoryScript.instance.RemoveBag(MyBag);
        MyBag.MyBagButton = null;

        foreach(Item item in MyBag.MyBagScript.GetItem())
        {
            InventoryScript.instance.AddItem(item);
        }
        MyBag = null;
    }
}
