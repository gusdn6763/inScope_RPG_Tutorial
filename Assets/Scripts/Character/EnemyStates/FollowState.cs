using UnityEngine;

class FollowState : IState
{
    private Enemy parent;


    public void Enter(Enemy parent)
    {
        Player.instance.AddAttacker(parent);
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (parent.Target != null)
        {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;

            Vector3 targetPosition = parent.Target.position;
            Vector3 myPosition = parent.transform.position;
            parent.transform.position
                = Vector2.MoveTowards(myPosition, targetPosition, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if (distance <= parent.AttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        }
    }
}
