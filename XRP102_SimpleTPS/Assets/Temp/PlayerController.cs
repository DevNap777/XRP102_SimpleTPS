using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �浹�� �ּ�ȭ �� �� �ִ� �ļ�
// Test������ ����� ������, �̷��� namespace�� ���μ� �浹�� ������ �� ����
// ��, �̸� ����� �ϰ� ���� ���� �߿�
namespace SJO_Test
{
    /// <summary>
    /// Movement �׽�Ʈ������ ������ Ŭ�����Դϴ�.
    /// Controller �����Ͻô� �в��� Movement ȣ����� �޼��� ���� �����ø�
    /// �ش� ������ �����ϼŵ� �˴ϴ�.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement _movement;
        public PlayerStatus _status;

        private void Update()
        {
            MoveTest();

            // IsAiming ����� �׽�Ʈ �ڵ�
            _status.ISAiming.Value = Input.GetKey(KeyCode.Mouse1);
        }
        
        /// <summary>
        /// �Ʒ� �޼��忡 ���� �ҽ��ڵ�� ���� ������� ����մϴ�.
        /// </summary>
        public void MoveTest()
        {
            // (ȸ�� ���� ��) �¿�ȸ���� ���� ���� ��ȯ
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.ISAiming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            // moveDir�� Vector3�� ������ �ƴ϶�� IsMovint.Value�� �� ����
            _status.ISMoving.Value = moveDir != Vector3.zero;

            //��ü�� ȸ�� ��� ���� �� ȣ���ؾ� ��.
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

