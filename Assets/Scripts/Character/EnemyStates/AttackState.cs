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
        //만약 공격 쿨타임이 보다 공격시간이 크고, 공격상태가 아니면
        if (parent.AttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.AttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        //타겟이 존재시
        if (parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            
            //어그로 범위 >= 공격 범위 + 플레이어가 공격한 추가 거리 && 공격상태가 아닐때 
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

        //EnemyController의 2번째 레이아웃(AttackLayer)의 현재 진행중인 애니메이션 즉 몹의 공격 애니메이션 시간
        yield return new WaitForSeconds(parent.Animator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }
}
