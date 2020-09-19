using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup = null;

    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }


    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChanged(health.MyCurrentValue);
    }
}
