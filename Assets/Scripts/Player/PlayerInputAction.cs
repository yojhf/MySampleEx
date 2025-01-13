using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MySampleEx
{
    // �÷��̾� Input ���� Ŭ����
    public class PlayerInputAction : MonoBehaviour
    {
        // �̸� �Է°�
        public Vector2 Move { get; private set; }
        // ����
        public bool IsJump { get; set; }
        // ���� �Է°�
        public bool Attack { get; private set; }
        // ���� �ڷ�ƾ
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