using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;

    [SerializeField] private Vector3 offset = Vector3.zero;
    public IMoveable Dragable { get; set; }

    private Image icon;

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
        icon = GetComponent<Image>();
    }

    void Update()
    {
        icon.transform.position = Input.mousePosition + offset;

        DeleteItem();
    }

    //SpellButton 스크립트에서 실행 -> 스킬창에 있는 스킬버튼
    public void TakeMoveable(IMoveable moveable)
    {
        this.Dragable = moveable;
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = Dragable;

        Dragable = null;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }

    public void Drop()
    {
        Dragable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private void DeleteItem()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && instance.Dragable != null)
        {
            if (Dragable is Item && InventoryScript.instance.ChoosedSlot != null)
            {
                (Dragable as Item).MySlot.Clear();
            }
            Drop();
            InventoryScript.instance.ChoosedSlot = null;
        }
    }
}
