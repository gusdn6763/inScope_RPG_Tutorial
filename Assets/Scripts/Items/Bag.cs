using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 유니티 메뉴에 UI추가  
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    [SerializeField] protected GameObject bagPrefab;
    private int slots;

    public BagScript MyBagScript { get; set; }
    public int Slots { get { return slots; } }

    public void Initalize(int slots)
    {
        // Bag의 슬롯갯수 설정
        this.slots = slots;
    }

    // 아이템 사용
    public void Use()
    {
        Remove();
        if (InventoryScript.instance.CanAddBag)
        {
            MyBagScript = Instantiate(bagPrefab, InventoryScript.instance.transform).GetComponent<BagScript>();

            //slot 아이템을 Bag 안에 추가한다.
            MyBagScript.AddSlots(slots);
            InventoryScript.instance.AddBag(this);
        }
    }
}
