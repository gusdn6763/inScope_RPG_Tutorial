using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable
{
    private SlotScript slot;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int stackSize = 0;


    public SlotScript MySlot { get { return slot; } set { slot = value; } }
    public Sprite Icon { get { return icon; } }
    public int StackSize { get { return stackSize; } }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}