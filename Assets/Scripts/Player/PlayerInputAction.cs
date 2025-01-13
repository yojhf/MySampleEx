using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MySampleEx
{
    // 플레이어 Input 관리 클래스
    public class PlayerInputAction : MonoBehaviour
    {
        // 이름 입력값
        public Vector2 Move { get; private set; }
        // 점프
        public bool IsJump { get; set; }
        // 공격 입력값
        public bool Attack { get; private set; }
        // 공격 코루틴
        private Coroutine m_AttackWaitCoroutine;

        #region NewInput SendMessage
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }
        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }
        public void OnAttack(InputValue value)
        {
            AttackInput(value.isPressed);
        }
        #endregion

        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }
        public void JumpInput(bool newJumpState)
        {
            IsJump = newJumpState;
        }
        public void AttackInput(bool newAttackState)
        {
            Attack = newAttackState;

            if (m_AttackWaitCoroutine != null)
            {
                StopCoroutine(m_AttackWaitCoroutine);
            }
            m_AttackWaitCoroutine = StartCoroutine(AttackWait());
        }

        IEnumerator AttackWait()
        {
            yield return new WaitForSeconds(0.03f);

            Attack = false;
        }
    }
}