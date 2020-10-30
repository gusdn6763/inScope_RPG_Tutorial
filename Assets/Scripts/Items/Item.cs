using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    private SlotScript slot;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private Quality quality;
    [SerializeField] private int stackSize = 0;
    [SerializeField] private string title = null;

    public SlotScript MySlot { get { return slot; } set { slot = value; } }
    public Sprite Icon { get { return icon; } }
    public int StackSize { get { return stackSize; } }

    public Quality Quality { get => quality; }
    public string Title { get => title; }

    public virtual string GetDescription()
    {
        string color = string.Empty;
        return string.Format("<color={0}>{1}</color>", QualityColor.MyColors[Quality], Title);
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}