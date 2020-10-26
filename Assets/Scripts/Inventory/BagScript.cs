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
    // 받은 수만큼 슬롯을 추가해줌  
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
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


    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}