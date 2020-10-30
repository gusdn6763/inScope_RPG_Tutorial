using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    //아이템의 중복갯수
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField] private Image icon;
    [SerializeField] private Text stackSize;


    public BagScript MyBag { get; set; }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return Items.Peek();
            }
            return null;
        }
    }
    public Image MyIcon { get { return icon; } set { icon = value; } }

    public int MyCount { get { return Items.Count; } }
    // 빈 슬롯 여부
    public bool IsEmpty { get { return Items.Count == 0; } }
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

    public ObservableStack<Item> Items { get => items; }

    private void Awake()
    {
        Items.OnPop += new UpdateStackEvent(UpdateSlot);
        Items.OnPush += new UpdateStackEvent(UpdateSlot);
        Items.OnClear += new UpdateStackEvent(UpdateSlot);
    }
    // 슬롯에 아이템 추가.
    public bool AddItem(Item item)
    {
        Items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //처음에 슬롯의 아이템 선택시 ChoosedSlot값은 비어있음
            //슬롯의 아이템을 집을시 InventoryScript.instance.ChoosedSlot = this;값을 넣음  
            if (InventoryScript.instance.ChoosedSlot == null && !IsEmpty)
            {
                //집은 아이템이 존재하고 집은 아이템이 가방일시  
                if (HandScript.instance.Dragable != null && HandScript.instance.Dragable is Bag)
                {
                    if (MyItem is Bag)
                    {
                        InventoryScript.instance.SwapBags(HandScript.instance.Dragable as Bag, MyItem as Bag);
                    }
                }
                else
                {
                    //HandScript에게 자신의 아이템 정보값을 넘김 -> 집은 아이템 표시
                    HandScript.instance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.instance.ChoosedSlot = this;
                }

            }
            //만약 잡은게 없고, 선택한 슬롯이 비어있고, 가방을 선택했으면
            else if (InventoryScript.instance.ChoosedSlot == null && IsEmpty && HandScript.instance.Dragable is Bag)
            {
                //가방을 추가, 가방 삭제, 아이템을 놓음
                Bag bag = (Bag)HandScript.instance.Dragable;

                if (bag.MyBagScript != MyBag && InventoryScript.instance.MyEmptySlotCount - bag.Slots > 0)
                {
                    AddItem(bag);
                    bag.MyBagButton.RemoveBag();
                    HandScript.instance.Drop();
                }
            }
            //만약 무언가를 집은 상태라면
            else if (InventoryScript.instance.ChoosedSlot != null)
            {
                //아이템을 되돌려놓는지 확인하는 함수, 아이템을 확인 후 합치는 함수, 아이템의 위치를 바꾸는 함수
                //아이템을 빈 슬롯에 넣는지 확인하는 함수  
                if (PutItemBack() 
                   || MergeItems(InventoryScript.instance.ChoosedSlot) 
                   || SwapItems(InventoryScript.instance.ChoosedSlot) 
                   || AddItems(InventoryScript.instance.ChoosedSlot.Items))
                {
                    //아이템을 놓고, 초기화
                    HandScript.instance.Drop();
                    InventoryScript.instance.ChoosedSlot = null;
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    private bool PutItemBack()
    {
        if (InventoryScript.instance.ChoosedSlot == this)
        {
            InventoryScript.instance.ChoosedSlot.MyIcon.color = Color.white;
            return true;
        }
        return false;
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
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.Items);

            // 아이템을 옮기기 때문에 초기화
            from.Items.Clear();
            // from 슬롯의 아이템 리스트에 해당 슬롯의 아이템리스트 전달
            from.AddItems(Items);
            // 현재 슬롯의 아이템 리스트 초기화
            Items.Clear();
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
            Items.Pop();
        }
    }    

    public void RemoveItem(Item item)
    {
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            InventoryScript.instance.OnItemCountChanged(Items.Pop());

            //if (MyCount == 0)
            //{
            //    MyIcon.color = new Color(0, 0, 0, 0);
            //}

        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && Items.Count < MyItem.StackSize)
        {
            Items.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }
    private void UpdateSlot()
    {
        UIManager.instance.UpdateStackSize(this);
    }


    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        //클릭한from에 있는 아이템이 현재 슬롯의 아이템과 같고, 꽉 차있지 않으면
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //아이템의 갯수를 중첩의 갯수만큼 합칠 수 있도록 만든다.
            int free = MyItem.StackSize - MyCount;

            for(int i = 0; i < free; i++)
            {
                AddItem(from.Items.Pop());
            }
        }
        return true;
    }

    public void Clear()
    {
        if (Items.Count > 0)
        {
            InventoryScript.instance.OnItemCountChanged(Items.Pop());
            Items.Clear();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.instance.ShowTooltip(transform.position, MyItem);
        }
    }

    // 마우스 커서가 Slot 영역 안에서 밖으로 나가면 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }
}

