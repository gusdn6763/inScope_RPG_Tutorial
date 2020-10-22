using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    private Image icon;
    public IUseable MyUseable { get; set; }
    public Button MyButton { get; private set; }
    public Image MyIcon { get => icon; set => icon = value; }
    private void Awake()
    {
        MyButton = GetComponent<Button>();
        icon = GetComponentInChildren<Image>();
    }
    private void Start()
    {
        // 클릭 이벤트를 MyButton 에 등록한다.
        MyButton.onClick.AddListener(OnClick);
    }

    // 클릭 발생하면 실행
    public void OnClick()
    {
        //첫 시작시 스킬이 할당되어있지 않으므로 시작하지 않음
        if (MyUseable != null)
        {
            MyUseable.Use();
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
            if (HandScript.instance.MyMoveable != null) //&& HandScript.instance.MyMoveable is IUseable)
            {
                //스킬UI에 이미지와 색깔을 넣고 useable의 정보를 넣음
                SetUseable(HandScript.instance.MyMoveable as IUseable);
            }
        }
    }

    public void SetUseable(IUseable useable)
    {
        this.MyUseable = useable;

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        MyIcon.sprite = HandScript.instance.Put().Icon;
        MyIcon.color = Color.white;
    }
}
