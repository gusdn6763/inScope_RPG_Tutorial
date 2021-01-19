using System;
using UnityEngine;

[Serializable]
public class Spell : IUseable, IMoveable, IDescribable
{
    [SerializeField] private GameObject spellGameObject = null;
    [SerializeField] private Sprite icon = null;

    [SerializeField] private Color barColor = Color.white;

    [SerializeField] private string name = null;
    [SerializeField] private string description = null;
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

    public Sprite MyIcon
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

    public string MyName
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

    public string GetDescription()
    {
        return string.Format("{0}\n캐스팅 시간: {1}초\n피해량: {2} \n<color=#ffd111>설명: {3}</color>", name, castTime, damage, description);
    }

    public void Use()
    {
        Player.instance.CastSpell(MyName);
    }
}
