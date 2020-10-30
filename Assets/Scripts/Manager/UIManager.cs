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
    [SerializeField] private GameObject tooltip = null;
    [SerializeField] private GameObject targetFrame = null;
    [SerializeField] private Image portraitFrame = null;
    [SerializeField] private CanvasGroup keybindMenu = null;
    [SerializeField] private CanvasGroup spellBook = null;
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
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);
        heathStat.Initialize(target.Health.MyCurrentValue, target.Health.MyMaxValue);
        portraitFrame.sprite = target.MyPortrait;
        target.healthChanged += new HealthChanged(UpdateTargetFrame);
        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
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
    public void ShowTooltip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltip.GetComponentInChildren<Text>().text = description.GetDescription();
    }

    // 튤팁UI 비활성화
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
