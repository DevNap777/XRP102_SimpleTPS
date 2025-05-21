using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Vector2 InputDirection { get; private set; }
    public Vector2 MouseDirection { get; private set; }

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
        //Vector2 mouseDirection = GetMouseDirection();

        // x���� ����� ������ �� �ʿ� ����.
        _currentRotation.x += MouseDirection.x;

        // y���� ��쿣 ���� ������ �ɾ�� ��.
        _currentRotation.y = Mathf.Clamp(_currentRotation.y + MouseDirection.y, _minPitch, _maxPitch);

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

    public Vector3 GetMoveDirection()
    {
        //Vector2 input = InputDirection;
    
        Vector3 direction = 
            // �� ����
            (transform.right * InputDirection.x) + 
            // �� ����
            (transform.forward * InputDirection.y);
    
        // ����ȭ
        return direction.normalized;
    }
  
    // Input System ���� ������ �̸� Move �տ� On�� ���̸� ȣ���
    public void OnMove(InputValue value)
    {
        InputDirection = value.Get<Vector2>();
    }

    public void OnRotate(InputValue value)
    {
        Vector2 mouseDir = value.Get<Vector2>();
        mouseDir.y *= -1;
        MouseDirection = mouseDir * _mouseSensitivity;
    }


    //public Vector3 GetInputDirection()
    //{
    //    // �����е尡 ���� GetAxis�� ����
    //    float x = Input.GetAxisRaw("Horizontal");
    //    float z = Input.GetAxisRaw("Vertical");
    //
    //    // ���� ������ ������ y�� ���� �� ��
    //    return new Vector3(x, 0, z);
    //}

    // ���콺�� x, y�� �����Ƿ� Vector2 ���
    //private Vector2 GetMouseDirection()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
    //    // -�� �ϰ� ������� ���ϴ� �������� ������
    //    float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;
    //
    //    return new Vector2(mouseX, mouseY);
    //}
}
