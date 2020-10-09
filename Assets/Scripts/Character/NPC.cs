using UnityEngine;


public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class NPC : Character
{
    [SerializeField] private Sprite Portrait = null;
    public event HealthChanged healthChanged;
    public event CharacterRemoved characterRemoved;

    public Sprite MyPortrait
    {
        get
        {
            return Portrait;
        }
    }

    public virtual void DeSelect()
    {
        healthChanged -= new HealthChanged(UIManager.instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.instance.HideTargetFrame);
    }

    public virtual Transform Select()
    {
        return hitBox;
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
        //   UIManager.instance.UpdateTargetFrame(health);
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }
        Destroy(this.gameObject);
    }
}