﻿using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

[RequireComponent(typeof(LootTable))]
public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;
    public event CharacterRemoved characterRemoved;

    [SerializeField] private CanvasGroup healthGroup = null;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float extraRange = 0.2f;
    [SerializeField] float initAggroRange = 0.0f;

    private LootTable lootTable;
    private IState currentState;
    [SerializeField] private Sprite Portrait = null;

    public Sprite MyPortrait
    {
        get
        {
            return Portrait;
        }
    }

    public Vector3 StartPosition { get; set; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ExtraRange { get => extraRange; set => extraRange = value; }
    public float AggroRange { get; set; }
    public bool InRange {get { return Vector2.Distance(transform.position, Target.transform.position) < AggroRange; } }
    public float AttackTime { get; set; }

    protected override void Awake()
    {
        base.Awake();
        lootTable = GetComponent<LootTable>();
    }

    protected void Start()
    {
        health.Initialize(initHealth, initHealth);
        AggroRange = initAggroRange;
        StartPosition = transform.position;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                AttackTime += Time.deltaTime;
            }

            currentState.Update();
        }
        base.Update();
    }


    public void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UIManager.instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.instance.HideTargetFrame);
    }

    public override void TakeDamage(float damage, Character source)
    {
        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);
                base.TakeDamage(damage, source);
                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Player.instance.MyAttackers.Remove(this);
                    Player.instance.GainXP(XPManager.CalculateXP((this as Enemy)));
                }
            }
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Character target)
    {
        if (Target == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            AggroRange = initAggroRange;
            AggroRange += distance;
            Target = target;
        }
    }

    public void Reset()
    {
        Target = null;
        this.AggroRange = initAggroRange;
        this.Health.MyCurrentValue = this.Health.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public Character Select()
    {
        healthGroup.alpha = 1;

        return this;
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach (IInteractable interactable in Player.instance.MyInteractable)
            {
                if (interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }
            LootWindow.instance.CreatePages(drops);
        }
    }
    public void StopInteract()
    {
        LootWindow.instance.Close();
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
