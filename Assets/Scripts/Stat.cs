﻿using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;
    [SerializeField] private Text statValue = null ;

    [SerializeField] private float lerpSpeed = 1f;
    private float currentFill;
    public float MyMaxValue;


    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue) 
                currentValue = MyMaxValue;
            else if (value < 0) 
                currentValue = 0;
            else 
                currentValue = value;

            currentFill = currentValue / MyMaxValue;

            if (statValue != null)
            {
                statValue.text = currentValue + " / " + MyMaxValue;
            }
        }
    }

    private float currentValue;

    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / maxValue;
    }
}