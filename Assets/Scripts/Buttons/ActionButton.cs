using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text stackSize;

    private Stack<IUseable> useables = new Stack<IUseable>();
    [SerializeField] private Image icon;
    private int count;

    public IUseable MyUseable { get; set; }
    public Button MyButton { get; private set; }
    public Image MyIcon { get { return icon; } set { icon = value; } }
    public int MyCount { get { return count; } }
    public Text StackText { get { return stackSize; } }

    private void Awake()
    {
        MyButton = GetComponent<Button>();
    }
    private void Start()
    {
        // 클릭 이벤트를 MyButton 에 등록한다.
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // 클릭 발생하면 실행
    public void OnClick()
    {
        if (HandScript.instance.Dragable == null)
        {
            // 액션퀵슬롯에 등록된 것이 스킬이라면
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            // 액션퀵슬롯에 사용가능한 아이템이 등록되었고
            // 등록된 아이템의 개수가 1개 이상이라면
            if (useables != null && useables.Count > 0)
            {
                // useables 배열에서 개체 하나를 삭제하고 사용
                useables.Peek().Use();
            }
        }
    }

    // 클릭이 발생했는지 감지. 
    // IPointerClickHandler 에 명시된 함수이다.
    public void OnPointerClick(PointerEventData eventData)
    {
        //왼쪽 마우스로 스킬UI를 클릭시
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //클릭해서 HandScript의 값이 존재할시
            if (HandScript.instance.Dragable != null)
            {
                SetUseable(HandScript.instance.Dragable as IUseable);
            }
        }
    }

    public void SetUseable(IUseable useable)
    {
        // 액션 퀵슬롯에 등록되려는 것이 아이템이라면
        if (useable is Item)
        {
            // 해당 아이템과 같은 종류의 아이템을 가진 리스트를 저장하고
            useables = InventoryScript.instance.GetUseables(useable);
            // 개수 저장
            count = useables.Count;

            //  이동모드 상태 해제
            InventoryScript.instance.ChoosedSlot.MyIcon.color = Color.white;
            InventoryScript.instance.ChoosedSlot = null;
        }
        else
        {
            this.MyUseable = useable;
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {

        MyIcon.sprite = HandScript.instance.Put().Icon;
        MyIcon.color = Color.white;

        if (count > 1)
        {
            UIManager.instance.UpdateStackSize(this);
        }
    }
    public void UpdateItemCount(Item item)
    {
        // 아이템이 IUseable(인터페이스)을 상속받았으며
        // useables 배열의 아이템개수가 1개 이상이면
        if (item is IUseable && useables.Count > 0)
        {
            // useables 에 등록된 아이템과 item 이 같은 타입이라면
            if (useables.Peek().GetType() == item.GetType())
            {
                useables = InventoryScript.instance.GetUseables(item as IUseable);

                count = useables.Count;

                UIManager.instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        // 액션 버튼에 등록된 것이 스킬이라면
        //아이템도 MyUseable인터페이스가 존재하지만 SetUseable함수에서 MyUseable변수를 할당해주는것은 스킬뿐
        Debug.Log(MyUseable);
        if (MyUseable != null && MyUseable is IDescribable)
        {
            tmp = (IDescribable)MyUseable;
        }
        // 액션 버튼에 등록된 것이 아이템이라면
        else if (useables.Count > 0)
        {
            //#13.5 기준 액션퀵슬롯에 아이템이 들어갈시 설명창UI는 표현하지 않는다
         //   UIManager.instance.ShowTooltip(transform.position, tmp);
        }
        if (tmp != null)
        {
            UIManager.instance.ShowTooltip(transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }
}
