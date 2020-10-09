﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject[] keybindButtons = null;
    [SerializeField] private Button[] actionButtons = null;
    [SerializeField] private GameObject targetFrame = null;
    [SerializeField] private Image portraitFrame = null;
    [SerializeField] private CanvasGroup keybindMenu =  null;

    private Stat heathStat;

    private KeyCode action1, action2, action3;

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
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }
        if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }

    private void ActionButtonOnClick(int btnIndex)
    {
        actionButtons[btnIndex].onClick.Invoke();
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

    public void OpenCloseMenu()
    {
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;
        keybindMenu.blocksRaycasts = (keybindMenu.blocksRaycasts) == true ? false : true;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }
}
