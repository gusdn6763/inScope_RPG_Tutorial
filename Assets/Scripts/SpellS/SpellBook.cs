using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    public static SpellBook instance;

    [SerializeField] private Spell[] spells = null;
    [SerializeField] private Image castingBar = null;
    [SerializeField] private Image spellIcon = null;
    [SerializeField] private Text currentSpell = null;
    [SerializeField] private Text spellCastingTime = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    private Coroutine spellCoroutine;
    private Coroutine spellStopCoroutine;

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
    }
    public Spell CastSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);

        castingBar.fillAmount = 0f;
        castingBar.color = spell.BarColor;
        spellIcon.sprite = spell.Icon;
        currentSpell.text = spell.Name;
        spellCastingTime.text = spell.CastTime.ToString();
        spellCoroutine = StartCoroutine(Progress(spell));
        spellStopCoroutine = StartCoroutine(FadeBar());
        return spell;
    }

    private IEnumerator Progress(Spell spell)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / spell.CastTime;

        float progress = 0.0f;

        while(progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            spellCastingTime.text = (spell.CastTime - timePassed).ToString("F1");

            if ( spell.CastTime - timePassed < 0)
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

        while (progress <= 1.0)
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
        if (spellCoroutine != null)
        {
            StopCoroutine(spellCoroutine);
            spellCoroutine = null;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);
        return spell;
    }
}
