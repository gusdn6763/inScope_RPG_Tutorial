﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField] private Spell[] spells;
    [SerializeField] private Image castingBar;
    [SerializeField] private Image spellIcon;
    [SerializeField] private Text spellName;
    [SerializeField] private Text spellCastingTime;
    [SerializeField] private CanvasGroup canvasGroup;
    private Coroutine spellCoroutine;
    private Coroutine spellStopCoroutine;

    public Spell CastSpell(int index)
    {
        castingBar.fillAmount = 0f;
        castingBar.color = spells[index].BarColor;
        spellIcon.sprite = spells[index].Icon;
        spellName.text = spells[index].Name;
        spellCastingTime.text = spells[index].CastTime.ToString();
        spellCoroutine = StartCoroutine(Progress(index));
        spellStopCoroutine = StartCoroutine(FadeBar());
        return spells[index];
    }

    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / spells[index].CastTime;

        float progress = 0.0f;

        while(progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            spellCastingTime.text = (spells[index].CastTime - timePassed).ToString("F1");

            if ( spells[index].CastTime - timePassed < 0)
            {
                spellCastingTime.text = "0.0";
            }
            yield return null;
        }
        StopCasting();
    }

    private IEnumerator FadeBar()
    {

        float rate = 1.0f / 0.50f;

        float progress = 0.0f;

        while (progress <= 1.0)//As long as we are not done fading
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    public void StopCasting()
    {
        if (spellStopCoroutine != null)
        {
            StopCoroutine(spellStopCoroutine);
            canvasGroup.alpha = 0;
            spellStopCoroutine = null;

        }
    }
}
