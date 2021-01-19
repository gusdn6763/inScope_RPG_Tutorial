using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 유니티 메뉴에 UI추가  
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    [SerializeField] protected GameObject bagPrefab;
    [SerializeField] private int slots;

    public BagScript MyBagScript { get; set; }

    public BagButton MyBagButton { get; set; }
    public int MySlotCount { get { return slots; } }

    public void Initialize(int slots)
    {
        // Bag의 슬롯갯수 설정
        this.slots = slots;
    }

    // 아이템 사용
    public void Use()
    {
        if (InventoryScript.instance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.instance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            // 해당 가방이 등록된 가방퀵슬롯이 없다면
            if (MyBagButton == null)
            {
                InventoryScript.instance.AddBag(this);
            }
            // 해당 가방에 등록된 가방퀵슬롯이 있다면
            else
            {
                // 인벤토리에 가방을 추가한다. (가방 교체)
                InventoryScript.instance.AddBag(this, MyBagButton);
            }
            MyBagScript.MyBagIndex = MyBagButton.BagIndex;
        }
    }

    //가방창UI 생성
    public void SetupScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.instance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0}칸짜리 슬롯 가방", slots);
    }
}
