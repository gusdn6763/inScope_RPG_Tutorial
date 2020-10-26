using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField] private int health;
    public void Use()
    {
        Remove();
        Player.instance.Health.MyCurrentValue += health;
    }
}
