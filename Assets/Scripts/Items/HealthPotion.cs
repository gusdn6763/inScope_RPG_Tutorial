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

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: 체력을 {0} 회복시켜준다</color>", health);
    }
}
