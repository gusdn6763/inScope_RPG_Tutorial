using UnityEngine;

public class Enemy : NPC
{
    [SerializeField] private CanvasGroup healthGroup = null;
    private Transform target;
    private IState currentState;
    public Transform Target { get => target; set => target = value; }


    protected override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }
    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
    }

    protected override void Update()
    {
        currentState.Update();

        base.Update();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChanged(health.MyCurrentValue);
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            Direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
        }
        else
        {
            Direction = Vector2.zero;
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

}
