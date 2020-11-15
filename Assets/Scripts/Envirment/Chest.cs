using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite openSprite, closedSprite;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private BagScript bag;

    private SpriteRenderer spriteRenderer;
    //상자안의 아이템들
    private List<Item> items = new List<Item>();

    private bool isOpen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

    }

    public void StopInteract()
    {
        StoreItems();
        bag.Clear();
        isOpen = false;
        spriteRenderer.sprite = closedSprite;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    public void AddItems()
    {
        if (items != null)
        {
            foreach (Item item in items)
            {
                item.MySlot.AddItem(item);
            }
        }
    }

    //아이템 저장
    public void StoreItems()
    {
        items = bag.GetItem();
    }
}
