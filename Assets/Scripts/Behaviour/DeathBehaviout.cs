using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehaviout : StateMachineBehaviour
{
    private float timePassed;

    //애니메이션 시작시
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.transform.GetChild(0).gameObject);
    }

    //애니메이션이 시작중일때
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timePassed += Time.deltaTime;
        //5초 후에 event CharacterRemoved변수를 실행
        //몹을 클릭시 UIManager의 ShowTargetFrame함수에서 함수들을 받음
        if (timePassed >= 5)
        {
            animator.GetComponent<Enemy>().OnCharacterRemoved();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
