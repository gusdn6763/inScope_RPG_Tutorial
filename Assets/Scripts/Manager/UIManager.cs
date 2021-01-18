using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject[] keybindButtons = null;
    [SerializeField] private ActionButton[] actionButtons = null;
    [SerializeField] private CharacterPanel characterPanel = null;
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private GameObject tooltip = null;
    [SerializeField] private GameObject targetFrame = null;
    [SerializeField] private Image portraitFrame = null;
    [SerializeField] private CanvasGroup keybindMenu = null;
    [SerializeField] private CanvasGroup spellBook = null;
    [SerializeField] private Text levelText;

    private Stat heathStat;
    private Text tooltipText;

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
        tooltipRect = tooltip.GetComponent<RectTransform>();
        heathStat = targetFrame.GetComponentInChildren<Stat>();
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keybindMenu);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenClose(spellBook);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryScript.instance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            characterPanel.OpenClose();
        }
    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        heathStat.Initialize(target.Health.MyCurrentValue, target.Health.MyMaxValue);
        portraitFrame.sprite = target.MyPortrait;
        levelText.text = target.MyLevel.ToString();

        //적의 체력이 변경시에 실행하는 함수
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        //적이 죽거나 다른 몹으로 타겟팅시 실행하는 함수
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        //적 레벨이 5이상 높을경우
        if (target.MyLevel >= Player.instance.MyLevel + 5)
        {
            levelText.color = Color.red;
        }
        //적 레벨이 3, 4이상 높을경우
        else if (target.MyLevel == Player.instance.MyLevel + 3 || target.MyLevel == Player.instance.MyLevel + 4)
        {
            levelText.color = new Color32(255, 124, 0, 255);
        }
        // 적 레벨과 플레이어레벨이 -2 ~ + 2 인경우
        else if (target.MyLevel >= Player.instance.MyLevel - 2 && target.MyLevel <= Player.instance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }
        // 적 레벨이 플레이어보다 3이상 낮은경우 또는 플레이어 레벨이 몬스터 레벨보다 너무 낮지 않은 경우
        else if (target.MyLevel <= Player.instance.MyLevel - 3 && target.MyLevel > XPManager.CalculateGrayLevel())
        {
            levelText.color = Color.green;
        }
        //더 낮은경우
        else
        {
            levelText.color = Color.grey;
        }
    }
    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }
    public void UpdateTargetFrame(float health)
    {
        heathStat.MyCurrentValue = health;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }


    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.StackText.text = clickable.MyCount.ToString();
            clickable.StackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.StackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.StackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.StackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;
    }

    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)
    {
        //피벗을 따로 선언해주는 이유 : 장비창에서 왼쪽의 장비한 아이템이 화면을 넘어가서 반대로 설명창 UI를 표현하기 위해
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltip.GetComponentInChildren<Text>().text = description.GetDescription();
    }

    // 튤팁UI 비활성화
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
