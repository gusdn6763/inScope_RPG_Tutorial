using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private LootWindow lootWindow;
    [SerializeField] private Image icon;

    [SerializeField] private Text title;

    public Image MyIcon { get { return icon; } }

    public Text MyTitle { get { return title; } }

    public Item MyLoot { get ;  set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //아이템을 인벤토리에 얻을시
        if (InventoryScript.instance.AddItem(MyLoot))
        {
            //게임상에서만 오브젝트가 비활성화 되고, 코드상에서는 리스트에서 빼줘야함
            gameObject.SetActive(false);
            //루팅UI에서 아이템 삭제
            lootWindow.TakeLoot(MyLoot);
            UIManager.instance.HideTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.ShowTooltip(new Vector2(1, 0), transform.position, MyLoot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }
}
