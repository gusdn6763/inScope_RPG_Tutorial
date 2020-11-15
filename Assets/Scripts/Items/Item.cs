using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    private SlotScript slot;
    private CharButton charButton;

    [SerializeField] private Sprite icon = null;
    [SerializeField] private Quality quality;

    [SerializeField] private string title = null;
    [SerializeField] private int stackSize = 0;
    [SerializeField] private int price;

    public SlotScript MySlot { get { return slot; } set { slot = value; } }
    public Sprite MyIcon { get { return icon; } }
    public CharButton MyCharButton { get { return charButton; }
        set
        {
            MySlot = null;
            charButton = value;
        }
    }

    public Quality MyQuality { get => quality; }
    public string MyTitle { get => title; }
    public int StackSize { get { return stackSize; } }
    public int MyPrice { get { return price; } }

    public virtual string GetDescription()
    {
        string color = string.Empty;
        return string.Format("<color={0}>{1}</color>", QualityColor.MyColors[MyQuality], MyTitle);
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}