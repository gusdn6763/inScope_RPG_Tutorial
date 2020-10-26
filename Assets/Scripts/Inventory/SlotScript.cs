using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    //아이템의 중복갯수
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField] private Image icon;
    [SerializeField] private Text stackSize;

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return items.Peek();
            }
            return null;
        }
    }
    public Image MyIcon { get { return icon; } set { icon = value; } }

    public int MyCount { get { return items.Count; } }
    // 빈 슬롯 여부
    public bool IsEmpty { get { return items.Count == 0; } }
    // 해당 슬롯이 가득 찼는지 확인
    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.StackSize)
            {
                return false;
            }

            else return true;
        }
    }

    public Text StackText { get { return stackSize; } }

    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }
    // 슬롯에 아이템 추가.
    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.instance.FromSlot == null && !IsEmpty)
            {
                HandScript.instance.TakeMoveable(MyItem as IMoveable);
                InventoryScript.instance.FromSlot = this;
            }
            else if (InventoryScript.instance.FromSlot != null)
            {
                if (PutItemBack() || SwapItems(InventoryScript.instance.FromSlot) || AddItems(InventoryScript.instance.FromSlot.items))
                {
                    HandScript.instance.Drop();
                    InventoryScript.instance.FromSlot = null;
                }
            }
        }
    }

    private bool SwapItems(SlotScript from)
    {
        // 슬롯이 비어있다면
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.StackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

            // from 슬롯의 아이템 리스트를 초기화
            from.items.Clear();
            // from 슬롯의 아이템 리스트에 해당 슬롯의 아이템리스트 전달
            from.AddItems(items);
            // 현재 슬롯의 아이템 리스트 초기화
            items.Clear();
            // 현재 슬롯의 아이템 리스트를 tmpFrom 으로 변경
            AddItems(tmpFrom);
            return true;
        }
        return false;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        // 해당 슬롯이 비어있거나 또는
        // newItems에 있는 아이템과 현재 슬롯의 아이템이 같다면
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                // 슬롯이 가득 찼는지 확인
                if (IsFull)
                {
                    return false;
                }

                // 아이템을 추가하고 newItems의 리스트에서 삭제합니다.
                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;

    }

    private bool PutItemBack()
    {
        if (InventoryScript.instance.FromSlot == this)
        {
            InventoryScript.instance.FromSlot.MyIcon.color = Color.white;
            return true;
        }
        return false;
    }

    public void UseItem()
    {
        // 해당 아이템 IUseable 인터페이스를 상속받았다면
        if (MyItem is IUseable)
        {
            // 해당 아이템을 사용한다.
            (MyItem as IUseable).Use();
        }
    }


    public void RemoveItem()
    {
        if (!IsEmpty)
        {
            items.Pop();
        }
    }    

    public void RemoveItem(Item item)
    {
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            // Items 의 제일 마지막 아이템을 꺼냅니다.
            items.Pop();

            //if (MyCount == 0)
            //{
            //    MyIcon.color = new Color(0, 0, 0, 0);
            //}

        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.StackSize)
        {
            items.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }
    private void UpdateSlot()
    {
        UIManager.instance.UpdateStackSize(this);
    }

}

