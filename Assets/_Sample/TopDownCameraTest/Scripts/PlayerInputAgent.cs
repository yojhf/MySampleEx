using UnityEngine;
using UnityEngine.InputSystem;

namespace MySampleEx
{
    // 플레이어(Agent) Input 관리 클래스
    public class PlayerInputAgent : MonoBehaviour
    {
        public bool Click { get; set; }
        public Vector2 MousePos { get; private set; }

        #region NewInput SendMessage
        public void OnClick(InputValue value)
        {
            ClickInput(value.isPressed);
        }
        public void OnMousePosition(InputValue value)
        {
            MousePositionInput(value.Get<Vector2>());
        }
        #endregion

        public void ClickInput(bool newMoveDirection)
        {
            Click = newMoveDirection;
        }

        public void MousePositionInput(Vector2 newJumpState)
        {
            MousePos = newJumpState;
        }
    }
}