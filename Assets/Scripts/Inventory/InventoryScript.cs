using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    [SerializeField] private Item[] items;
    [SerializeField] private BagButton[] bagButtons;
    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    public bool CanAddBag { get { return bags.Count < bagButtons.Count(); } }

    public SlotScript FromSlot
    {
        get { return fromSlot; }
        set
        {
            fromSlot = value;

            if (value != null)
            {
                fromSlot.MyIcon.color = Color.gray;
            }
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initalize(16);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initalize(16);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
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
                break;
            }
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
    public void AddItem(Item item)
    {
        if (item.StackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }
        PlaceInEmpty(item);
    }

    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slots in bag.MyBagScript.Slots)
            {
                if (slots.StackItem(item))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
    }
}
