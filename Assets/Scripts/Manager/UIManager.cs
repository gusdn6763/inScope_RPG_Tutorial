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
    [SerializeField] private GameObject targetFrame = null;
    [SerializeField] private Image portraitFrame = null;
    [SerializeField] private CanvasGroup keybindMenu =  null;
    [SerializeField] private CanvasGroup spellBook = null;

    private Stat heathStat;

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
    }
    void Start()
    {
        SetUseable(actionButtons[0], SpellBook.instance.GetSpell("Fireball"));
        SetUseable(actionButtons[1], SpellBook.instance.GetSpell("Frostbolt"));
        SetUseable(actionButtons[2], SpellBook.instance.GetSpell("Lightning"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keybindMenu);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(spellBook);
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
        Debug.Log(buttonName);
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void SetUseable(ActionButton btn, IUseable useable)
    {
        btn.MyIcon.sprite = useable.Icon;
        btn.MyIcon.color = Color.white;
        btn.MyUseable = useable;
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = (canvasGroup.blocksRaycasts) == true ? false : true;
    }
}
