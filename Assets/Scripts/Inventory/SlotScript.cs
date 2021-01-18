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
                return MyItems.Peek();
            }
            return null;
        }
    }
    public ObservableStack<Item> MyItems { get => items; }


    public Image MyIcon { get { return icon; } set { icon = value; } }

    public int MyCount { get { return MyItems.Count; } }
    // 빈 슬롯 여부
    public bool IsEmpty { get { return MyItems.Count == 0; } }
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
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }
    // 슬롯에 아이템 추가.
    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //왼쪽 클릭시 -> 아이템을 집고있는상태와 아이템을 집고있지 않는 상태로 나눌 수 있음  
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //처음에 슬롯의 아이템 선택시 ChoosedSlot값과 Dragable값은 비어있음
            //아직 선택한 슬롯이 없고, 드래그한 아이템도 없으므포
            //선택중일 슬롯도 없고, 슬롯에 무언가가 존재할시
            if (InventoryScript.instance.ChoosedSlot == null && !IsEmpty)
            {
                //무언가를 드래그한것이 있을시
                if (HandScript.instance.Dragable != null )
                {
                    //집은 아이템이 가방일시
                    if (HandScript.instance.Dragable is Bag)
                    {
                        if (MyItem is Bag)
                        {
                            InventoryScript.instance.SwapBags(HandScript.instance.Dragable as Bag, MyItem as Bag);
                        }
                    }
                    //집은 아이템이 장비일시
                    else if (HandScript.instance.Dragable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).ArmorType == (HandScript.instance.Dragable as Armor).ArmorType)
                        {
                            (MyItem as Armor).Equip();
                            HandScript.instance.Drop();
                        }
                    }
                }
                else
                {
                    //HandScript에게 자신의 아이템 정보값을 넘김 -> 집은 아이템 표시
                    HandScript.instance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.instance.ChoosedSlot = this;
                }

            }
            //선택한 슬롯이 없고, 드래그로 선택한 슬롯이 비어있을시 -> 
            //인벤토리의 슬롯에만 제한되는것이아닌 장비창 또는 스킬창에서 드래그도 가능하다
            else if (InventoryScript.instance.ChoosedSlot == null && IsEmpty)
            {
                if (HandScript.instance.Dragable is Bag)
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
                //장비를 다시 슬롯에 내려놓음
                else if (HandScript.instance.Dragable is Armor)
                {
                    Armor armor = (Armor)HandScript.instance.Dragable;
                    CharacterPanel.instance.MyCharButton.DequipArmor();
                    AddItem(armor);
                    HandScript.instance.Drop();
                }
            }
            //만약 무언가를 집은 상태라면
            else if (InventoryScript.instance.ChoosedSlot != null)
            {
                //아이템을 되돌려놓는지 확인하는 함수, 아이템을 확인 후 합치는 함수, 아이템의 위치를 바꾸는 함수
                //아이템을 빈 슬롯에 넣는지 확인하는 함수  
                if (PutItemBack()  || MergeItems(InventoryScript.instance.ChoosedSlot) || SwapItems(InventoryScript.instance.ChoosedSlot)  || AddItems(InventoryScript.instance.ChoosedSlot.MyItems))
                {
                    MyIcon.color = Color.white;
                    HandScript.instance.Drop();
                    //아이템을 놓고, 초기화
                    InventoryScript.instance.ChoosedSlot = null;
                    
                    
                }
            }
        }
        //오른쪽 버튼을 클릭하고, 드래그한것이 없을때
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.instance.Dragable == null)
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
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            // 아이템을 옮기기 때문에 초기화
            from.MyItems.Clear();
            // from 슬롯의 아이템 리스트에 해당 슬롯의 아이템리스트 전달
            from.AddItems(MyItems);
            // 현재 슬롯의 아이템 리스트 초기화
            MyItems.Clear();
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
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
    }


    public void RemoveItem()
    {
        if (!IsEmpty)
        {
            MyItems.Pop();
        }
    }    

    public void RemoveItem(Item item)
    {
        // 자기 자신이 빈슬롯이 아니라면
        if (!IsEmpty)
        {
            InventoryScript.instance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.StackSize)
        {
            MyItems.Push(item);
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

        Debug.Log(from.MyItem.GetType() +""+ MyItem.GetType());
    
        //클릭한from에 있는 아이템이 현재 슬롯의 아이템과 같고, 꽉 차있지 않으면
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //아이템의 갯수를 중첩의 갯수만큼 합칠 수 있도록 만든다.
            int free = MyItem.StackSize - MyCount;

            for(int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }
        }
        from.MyIcon.color = Color.white;
        MyItem.MySlot.MyIcon.color = Color.white;
        return true;
    }

    public void Clear()
    {
        int intitCount = MyItems.Count;

        if (intitCount > 0)
        {
            for (int i = 0; i < intitCount; i++)
            {
                InventoryScript.instance.OnItemCountChanged(MyItems.Pop());
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.instance.ShowTooltip(new Vector2(1, 0), transform.position, MyItem);
        }
    }

    // 마우스 커서가 Slot 영역 안에서 밖으로 나가면 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }
}

