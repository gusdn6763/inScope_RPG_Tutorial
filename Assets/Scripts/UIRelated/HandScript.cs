using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;

    [SerializeField]private Vector3 offset;
    public IMoveable MyMoveable { get; set; }

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
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }
}
