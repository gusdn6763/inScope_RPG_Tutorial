using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AttackState : IState
{
    private Enemy parent;
    private Coroutine stopCoroutine;
    private float attackCooldown = 1;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.AttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.AttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        if (parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            
            if (distance >= parent.AttackRange + parent.ExtraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.Animator.SetTrigger("Attack");

        yield return new WaitForSeconds(parent.Animator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }
}
