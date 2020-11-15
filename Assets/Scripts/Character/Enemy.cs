using UnityEngine;

[RequireComponent(typeof(LootTable))]
public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup = null;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float extraRange = 0.2f;
    [SerializeField] float initAggroRange = 0.0f;

    private LootTable lootTable;
    private IState currentState;

    public Vector3 StartPosition { get; set; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ExtraRange { get => extraRange; set => extraRange = value; }
    public float AggroRange { get; set; }
    public bool InRange {get { return Vector2.Distance(transform.position, Target.position) < AggroRange; } }
    public float AttackTime { get; set; }

    protected override void Awake()
    {
        base.Awake();
        lootTable = GetComponent<LootTable>();
    }

    protected override void Start()
    {
        base.Start();
        AggroRange = initAggroRange;
        StartPosition = transform.position;
        ChangeState(new IdleState());
    }
    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
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

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            SetTarget(source);
            base.TakeDamage(damage, source);
            OnHealthChanged(health.MyCurrentValue);
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

    public void SetTarget(Transform target)
    {
        if (Target == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
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

    public override void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }
    public override void StopInteract()
    {
        LootWindow.instance.Close();
    }
}
