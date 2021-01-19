using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;

    [SerializeField] private Vector3 offset = Vector3.zero;
    public IMoveable Dragable { get; set; }

    private Image icon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        icon = GetComponent<Image>();
    }


    private void Update()
    {
        // 마우스를 따라 아이콘이 이동한다.
        icon.transform.position = Input.mousePosition + offset;

        // 마우스 왼쪽클릭이 되었다. 그리고
        // 왼쪽클릭한 대상이 UI 가 아니다. 그리고
        // MyMoveable 이 null 이 아니다.
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && Dragable != null)
        {
            DeleteItem();
        }
    }
    

    //SpellButton 스크립트에서 실행 -> 스킬창에 있는 스킬버튼
    public void TakeMoveable(IMoveable moveable)
    {
        this.Dragable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = Dragable;

        Dragable = null;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }

    public void Drop()
    {
        Dragable = null;
        icon.color = new Color(0, 0, 0, 0);
        InventoryScript.instance.ChoosedSlot = null;
    }

    public void DeleteItem()
    {
        // Dragable인터페이스를 포함한 아이템이고, 선택가능한 슬롯이면
        if (Dragable is Item && InventoryScript.instance.ChoosedSlot != null)
        {
            Item item = (Item)Dragable;

            //아이템이 어떠한 슬롯에 있는 상태라면
            if (item.MySlot != null)
            {
                // 모두 만족하면 해당 슬롯의 모든 아이템을 삭제한다.
                (Dragable as Item).MySlot.Clear();
            }
            else if (item.MyCharButton != null)
            {
                item.MyCharButton.DequipArmor();
            }
        }
        // HandScript 의 아이콘 정보 초기화
        Drop();
        InventoryScript.instance.ChoosedSlot = null;
    }
}
