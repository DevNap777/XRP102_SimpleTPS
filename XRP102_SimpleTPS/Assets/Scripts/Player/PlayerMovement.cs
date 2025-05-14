using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// � Ŭ��������
// �ܺο� ������ �͵��� � �͵��� �ִ���
// �̰��� �Ϲ����� ��Ű��ó ����
public class PlayerMovement : MonoBehaviour
{
    // �������ּ���
    [Header("References")]
    [SerializeField] private Transform _avatar;
    [SerializeField] private Transform _aim;

    private Rigidbody _rigidbody;
    private PlayerStatus _playerStatus;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float _minPitch;
    [SerializeField][Range(0, 90)] private float _maxPitch;

    // ���콺�� ȸ�������� ��, �󸶳� ���� �����̴���
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    private Vector2 _currentRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
    }

    // �̵�
    public Vector3 SetMove(float moveSpeed)
    {
        Vector3 moveDirection = GetMoveDirection();

        Vector3 velocity = _rigidbody.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        _rigidbody.velocity = velocity;

        return moveDirection;
    }

    // Aim ȸ��
    public Vector3 SetAimRotation()
    {
        Vector2 mouseDirection = GetMouseDirection();

        // x���� ����� ������ �� �ʿ� ����.
        _currentRotation.x += mouseDirection.x;

        // y���� ��쿣 ���� ������ �ɾ�� ��.
        _currentRotation.y = Mathf.Clamp(_currentRotation.y + mouseDirection.y, _minPitch, _maxPitch);

        // ĳ���� ������Ʈ�� ��쿡�� Y�� ȸ���� �ݿ�
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

        // ������ ��� ���� ȸ�� �ݿ�
        // ���� ������ �� �� �������ִ°� ��������
        Vector3 currentEuler = _aim.localEulerAngles;
        _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

        // ȸ�� ���� ���� ��ȯ
        Vector3 rotateDirVector = transform.forward;

        // y�࿡ ���� ȸ���� ������� �ʰ�
        rotateDirVector.y = 0;

        return rotateDirVector.normalized;
    }

    // �� ȸ��
    public void SetAvatarRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _avatar.rotation = Quaternion.Lerp(_avatar.rotation, targetRotation, _playerStatus.RotateSpeed * Time.deltaTime);
    }

    // ���콺�� x, y�� �����Ƿ� Vector2 ���
    private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        // -�� �ϰ� ������� ���ϴ� �������� ������
        float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

        return new Vector2(mouseX, mouseY);
    }

    public Vector3 GetMoveDirection()
    {
        Vector3 input = GetInputDirection();

        Vector3 direction = 
            // �� ����
            (transform.right * input.x) + 
            // �� ����
            (transform.forward * input.z);

        // ����ȭ
        return direction.normalized;
    }

    public Vector3 GetInputDirection()
    {
        // �����е尡 ���� GetAxis�� ����
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // ���� ������ ������ y�� ���� �� ��
        return new Vector3(x, 0, z);
    }
}
