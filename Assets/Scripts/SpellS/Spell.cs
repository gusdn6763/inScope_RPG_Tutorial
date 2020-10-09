using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable
{
    [SerializeField] private GameObject spellGameObject = null;
    [SerializeField] private Sprite icon = null;

    [SerializeField] private Color barColor = Color.white;

    [SerializeField] private string name = null;
    [SerializeField] private float castTime = 0f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private int damage = 0;

    public GameObject SpellGameObject
    {
        get
        {
            return spellGameObject;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public Color BarColor
    {
        get
        {
            return barColor;
        }
    }

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

    public void Use()
    {
        Player.instance.CastSpell(Name);
    }
}