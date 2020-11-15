using System.Collections.Generic;
using UnityEngine;
 
public class BagScript : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;

    private List<SlotScript> slots = new List<SlotScript>();
    private CanvasGroup canvasGroup;


    public bool IsOpen { get { return canvasGroup.alpha > 0; } }

    public List<SlotScript> Slots { get => slots; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Item> GetItem()
    {
        List<Item> items = new List<Item>();
        foreach (SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach(Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }
    // 받은 수만큼 슬롯을 추가해줌  
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyBag = this;
            Slots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in Slots)
        {
            // 빈 슬롯이 있으면
            if (slot.IsEmpty)
            {
                // 해당 슬롯에 아이템을 추가한다.
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (SlotScript slot in Slots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public void Clear()
    {
        foreach (SlotScript slot in slots)
        {
            slot.Clear();
        }
    }
}