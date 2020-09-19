using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{

    public SpriteRenderer MySpriteRenderer { get; set; }

    public int CompareTo(Obstacle other)
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }

        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }

        return 0;
    }


    private void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }
}