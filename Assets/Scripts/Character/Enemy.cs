using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup = null;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float extraRange = 0.2f;
    [SerializeField] float initAggroRange;
    private IState currentState;
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ExtraRange { get => extraRange; set => extraRange = value; }
    public float AggroRange { get; set; }
    public bool InRange {get { return Vector2.Distance(transform.position, Target.position) < AggroRange; } }

    public float AttackTime { get; set; }

    protected override void Start()
    {
        base.Start();
        AggroRange = initAggroRange;
        //임시코드
        AttackRange = 3;
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
        SetTarget(source);
        base.TakeDamage(damage, source);
        OnHealthChanged(health.MyCurrentValue);
    }

    private void FollowTarget()
    {
        if (Target != null)
        {
            Direction = (Target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
        }
        else
        {
            Direction = Vector2.zero;
        }
    }
    public void ChangeState(IState newState)
    {
        Debug.Log(newState);
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (Target == null)
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
}
