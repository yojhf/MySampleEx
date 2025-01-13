using Unity.VisualScripting;
using UnityEngine;

namespace MySampleEx
{
    public class RandomStateSMB : StateMachineBehaviour
    {
        // ���� ������ ����
        public int numbersOfState = 3;
        // �Ϲ� ������ �ּ� ��� �ð�
        public float minNormalTime = 0f;
        // �Ϲ� ������ �ִ� ��� �ð�
        public float maxNormalTime = 5f;

        // �Ϲ� ������ ��� �ð�
        protected float m_RandomNormalTime;

        readonly int m_HashRandomIdle = Animator.StringToHash("RandomIdle");

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // ��� �ð� ���ϱ�
            m_RandomNormalTime = Random.Range(minNormalTime, maxNormalTime);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
            {
                animator.SetInteger(m_HashRandomIdle, -1);                
            }

            // Ÿ�̸�
            if(stateInfo.normalizedTime > m_RandomNormalTime && !animator.IsInTransition(0))
            {
                int randomNum = Random.Range(0, numbersOfState);

                animator.SetInteger(m_HashRandomIdle, randomNum);
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
}