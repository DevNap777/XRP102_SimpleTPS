using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 협업시 충돌을 최소화 할 수 있는 꼼수
// Test용으로 만들고 싶을때, 이렇게 namespace로 감싸서 충돌을 방지할 수 있음
// 단, 미리 약속을 하고 들어가는 것이 중요
namespace SJO_Test
{
    /// <summary>
    /// Movement 테스트용으로 구현한 클래스입니다.
    /// Controller 구현하시는 분께서 Movement 호출관련 메서드 정리 끝나시면
    /// 해당 파일은 삭제하셔도 됩니다.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement _movement;
        public PlayerStatus _status;

        private void Update()
        {
            MoveTest();

            // IsAiming 변경용 테스트 코드
            _status.ISAiming.Value = Input.GetKey(KeyCode.Mouse1);
        }
        
        /// <summary>
        /// 아래 메서드에 적힌 소스코드와 같은 방식으로 사용합니다.
        /// </summary>
        public void MoveTest()
        {
            // (회전 수행 후) 좌우회전에 대한 벡터 반환
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.ISAiming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            // moveDir이 Vector3의 영점이 아니라면 IsMovint.Value에 값 대입
            _status.ISMoving.Value = moveDir != Vector3.zero;

            //몸체의 회전 기능 구현 후 호출해야 함.
            Vector3 avatarDir;
            if (_status.ISAiming.Value)
            {
                avatarDir = camRotateDir;
            }
            else
            {
                avatarDir = moveDir;
            }

            _movement.SetAvatarRotation(avatarDir);
        }
    }
}

