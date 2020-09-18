using System;
using UnityEngine;

[Serializable]
public class Spell
{

    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellGameObject;


    [SerializeField]
    private Color barColor;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public float CastTime
    {
        get
        {
            return castTime;
        }

    }

    public GameObject SpellGameObject
    {
        get
        {
            return spellGameObject;
        }
    }

    public Color BarColor
    {
        get
        {
            return barColor;
        }
    }
}