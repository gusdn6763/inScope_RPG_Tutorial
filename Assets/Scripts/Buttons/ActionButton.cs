using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 플레이어 액션바 관련 클래스, OnPointerClick함수 먼저 보는것을 추천
/// </summary>
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
    public Text MyStackText { get { return stackSize; } }
    public Stack<IUseable> MyUseables  
    { get  { return useables; }
        set
        {
            if (value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }

            useables = value;
        }
    }

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
            // 액션퀵슬롯에 등록된 것이 사용가능한것이면 --> Iuseable을 상속받았다면
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            // 등록된 아이템의 개수가 1개 이상이라면 --> 소비형 아이템일경우
            else if(MyUseables != null && MyUseables.Count > 0)
            {
                // useables 배열에서 개체 하나를 삭제하고 사용
                MyUseables.Peek().Use();
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
            if (HandScript.instance.Dragable != null && HandScript.instance.Dragable is IUseable)
            {
                SetUseable(HandScript.instance.Dragable as IUseable);
            }
        }
    }

    /// <summary>
    /// 액션바에 놓을시Dragable에 있는 값을 이용해 액션바에 값을 저장   
    /// </summary>
    /// <param name="useable">IUseable인터페이스를 포함하는 아이템, 스펠을 받음</param>
    public void SetUseable(IUseable useable)
    {
        // 액션 퀵슬롯에 등록되려는 것이 아이템이라면
        if (useable is Item)
        {
            // 해당 아이템과 같은 종류의 아이템을 가진 리스트를 저장하고
            MyUseables = InventoryScript.instance.GetUseables(useable);

            //불러오기시에 ChoosedSlot값이 null이기 대문에 조건설정
            if (InventoryScript.instance.ChoosedSlot != null)
            {
                //  이동모드 상태 해제
                InventoryScript.instance.ChoosedSlot.MyIcon.color = Color.white;
                InventoryScript.instance.ChoosedSlot = null;
            }
        }
        else
        {
            MyUseables.Clear();
            this.MyUseable = useable;
        }

        count = MyUseables.Count;
        UpdateVisual(useable as IMoveable);
        UIManager.instance.RefreshTooltip(MyUseable as IDescribable);
    }

    /// <summary>
    /// 드래그한 오브젝트를 액션바에 놓을시 드래그 이미지를 지움 
    /// </summary>
    /// <param name="moveable">액션바의 이미지를 바꿈</param>
    public void UpdateVisual(IMoveable moveable)
    {
        //액션바에 스킬을 놓을시 드래그 오브젝트의 값을 비움
        if (HandScript.instance.Dragable != null)
        {
            HandScript.instance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;
        MyIcon.color = Color.white;

        if (count > 1)
        {
            UIManager.instance.UpdateStackSize(this);
        }
        else if (MyUseable is Spell)
        {
            UIManager.instance.ClearStackCount(this);
        }
    }

    /// <summary>
    /// 인벤토리에서 아이템이 증가시 액션바에서도 아이템을 증가시킴, OnItemCountChanged이벤트 함수를 이용
    /// </summary>
    /// <param name="item">액션바에서 받은 아이템갯수를 증가시킴</param>
    public void UpdateItemCount(Item item)
    {
        // 아이템이 IUseable(인터페이스)을 상속받았으며
        // useables 배열의 아이템개수가 1개 이상이면
        if (item is IUseable && MyUseables.Count > 0)
        {
            // useables 에 등록된 아이템과 item 이 같은 타입이라면
            if (MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.instance.GetUseables(item as IUseable);

                count = MyUseables.Count;

                UIManager.instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        // 액션 버튼에 등록된 것이 스킬이라면
        //아이템도 MyUseable인터페이스가 존재하지만 SetUseable함수에서 MyUseable변수를 할당해주는것은 스킬뿐
        if (MyUseable != null && MyUseable is IDescribable)
        {
            tmp = (IDescribable)MyUseable;
        }
        // 액션 버튼에 등록된 것이 아이템이라면
        else if (MyUseables.Count > 0)
        {
            //#13.5 기준 액션퀵슬롯에 아이템이 들어갈시 설명창UI는 표현하지 않는다
         //   UIManager.instance.ShowTooltip(transform.position, tmp);
        }
        if (tmp != null)
        {
            UIManager.instance.ShowTooltip(new Vector2(1, 0), transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideTooltip();
    }
}
