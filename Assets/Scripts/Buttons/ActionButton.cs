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
        if (MyUseable != null)
        {
            MyUseable.Use();
        }
    }

    // 클릭이 발생했는지 감지. 
    // IPointerClickHandler 에 명시된 함수이다.
    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
