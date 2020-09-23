using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{

    public SpriteRenderer spriteRenderer;

    private Color defaultColor;
    private Color fadeedColor;
    
    public int CompareTo(Obstacle other)
    {
        if (spriteRenderer.sortingOrder > other.spriteRenderer.sortingOrder)
        {
            return 1;
        }

        else if (spriteRenderer.sortingOrder < other.spriteRenderer.sortingOrder)
        {
            return -1;
        }

        return 0;
    }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        fadeedColor = defaultColor;
        fadeedColor.a = 0.7f;
    }

    public void FadeOut()
    {
        spriteRenderer.color = fadeedColor;
    }

    public void FadeIn()
    {
        spriteRenderer.color = defaultColor;
    }
}