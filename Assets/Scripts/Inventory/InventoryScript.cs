using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    public event ItemCountChanged itemCountChangedEvent;

    [SerializeField] private Item[] items;
    [SerializeField] private BagButton[] bagButtons;
    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    public bool CanAddBag { get { return bags.Count < bagButtons.Count(); } }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.Slots.Count;
            }
            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    public SlotScript ChoosedSlot
    {
        get { return fromSlot; }
        set
        {
            fromSlot = value;

            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }
    public List<Bag> MyBags
    {
        get
        {
            return bags;
        }
    }



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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Bag bag = (Bag)Instantiate(items[9]);
            bag.Initialize(16);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Bag bag = (Bag)Instantiate(items[9]);
            bag.Initialize(8);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddItem((HealthPotion)Instantiate(items[0]));
            AddItem((Armor)Instantiate(items[1]));
            AddItem((Armor)Instantiate(items[2]));
            AddItem((Armor)Instantiate(items[3]));
            AddItem((Armor)Instantiate(items[4]));
            AddItem((Armor)Instantiate(items[5]));
            AddItem((Armor)Instantiate(items[6]));
            AddItem((Armor)Instantiate(items[7]));
            AddItem((Armor)Instantiate(items[8]));
        }
    }

    public void AddBag(Bag bag)
    {
        foreach(BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton;
                bag.MyBagScript.transform.SetSiblingIndex(bagButton.BagIndex);
                break;
            }
        }
    }
    
    //가방 교체를 위한 오버로딩 함수
    public void AddBag(Bag bag, BagButton bagButton)
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(bagButton.BagIndex);
    }

    //불러오기를 통해 가방이 몇번째에 있었는지 파악하기위한 함수
    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetupScript();
        bags.Add(bag);
        bag.MyBagScript.MyBagIndex = bagIndex;
        bag.MyBagButton = bagButtons[bagIndex];
        bagButtons[bagIndex].MyBag = bag;
    }

    public void RemoveBag(Bag bag)
    {
        bag.SetupScript();
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        //변경될 모든 슬롯의 갯수 = (현재 모든 슬롯의 갯수 - 기존 가방의 슬롯갯수) + 바꿀려는 가방의 슬롯갯수
        int newSlotCount = (MyTotalSlotCount - oldBag.MySlotCount) + newBag.MySlotCount;

        //만약 원래 16칸 짜리 가방을 8칸짜리 가방으로 바꿀때, 아이템이 9칸이상 차지하고 있을시 가방 교체가 불가능하다.  
        if (newSlotCount - MyFullSlotCount >= 0)
        {
            //기존 가방의 아이템들을 임시 저장
            List<Item> bagItems = oldBag.MyBagScript.GetItem();
            //기존 가방 삭제  
            RemoveBag(oldBag);
            //어떤 퀵슬롯에 있었는지 알기 위해 값을 넘겨줌  
            newBag.MyBagButton = oldBag.MyBagButton;

            //새 가방 사용
            newBag.Use();
            foreach(Item item in bagItems)
            {
                if (item != newBag)
                {
                    //기존 가방의 임시 저장된 아이템들을 사용중인 가방에 생성
                    AddItem(item);
                }
            }
            //가방을 교체했으니 오래된 가방도 사용중인 가방에 생성
            AddItem(oldBag);
            HandScript.instance.Drop();
            instance.ChoosedSlot = null;
        }
    }    

    public void OpenClose()
    {
        bool closeBag = bags.Find(x => !x.MyBagScript.IsOpen);

        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.IsOpen != closeBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }
    public bool AddItem(Item item)
    {
        if (item.StackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        return PlaceInEmpty(item);
    }

    //갯수 쌓기
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slots in bag.MyBagScript.Slots)
            {
                if (slots.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }
        return false;
    }

    //액션바에 저장
    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        // 가방퀵슬롯에 등록된 모든 가방을 검사.
        foreach (Bag bag in bags)
        {
            // 가방의 모든 슬롯을 검사
            foreach (SlotScript slot in bag.MyBagScript.Slots)
            {
                // 빈슬롯이 아니고
                // 퀵슬롯에 등록된 아이템이 type의 아이템과 같은 종류의 아이템이라면
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        // useables 에 담는다.
                        useables.Push(item as IUseable);
                    }
                }
            }
        }
        return useables;
    }

    public IUseable GetUseables(string type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        // 가방퀵슬롯에 등록된 모든 가방을 검사.
        foreach (Bag bag in bags)
        {
            // 가방의 모든 슬롯을 검사
            foreach (SlotScript slot in bag.MyBagScript.Slots)
            {
                // 빈슬롯이 아니고
                // 슬롯에 저장된 아이템이 불러온 아이템(type)이름과 같으면
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    return (slot.MyItem as IUseable);
                }
            }
        }
        return null;
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;

        //가방 검색  
        foreach (Bag bag in bags)
        {
            //가방의 슬롯 검색  
            foreach (SlotScript slot in bag.MyBagScript.Slots)
            {
                //비어있지 않고 이름이 같으면  
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    itemCount += slot.MyItems.Count;
                }
            }
        }
        return itemCount;
    }

    //스택에 찾고자 하는 아이템과 갯수를 저장
    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();

        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.Slots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item);

                        if (items.Count == count)
                        {
                            return items;
                        }
                    }
                }
            }
        }
        return items;
    }

    //인벤토리의 아이템을 전부 불러온다 -> 저장하거나 불러올때 유용하다. 
    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.Slots)
            {
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
        }
        return slots;
    }

    //몇번째 가방의 몇번째 슬롯에 아이템을 추가한다.
    public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].MyBagScript.Slots[slotIndex].AddItem(item);
    }

    public void OnItemCountChanged(Item item)
    {
        // 이벤트에 등록된 델리게이트에 있다면
        if (itemCountChangedEvent != null)
        {
            // 이벤트에 등록된 모든 델리게이트호출 
            itemCountChangedEvent.Invoke(item);
        }
    }

}
