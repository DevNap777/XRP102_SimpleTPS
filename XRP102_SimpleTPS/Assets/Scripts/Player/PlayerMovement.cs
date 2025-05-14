using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 어떤 클래스에서
// 외부에 공개될 것들은 어떤 것들이 있는지
// 이것이 일반적인 아키텍처 설계
public class PlayerMovement : MonoBehaviour
{
    // 참조해주세요
    [Header("References")]
    [SerializeField] private Transform _avatar;
    [SerializeField] private Transform _aim;

    private Rigidbody _rigidbody;
    private PlayerStatus _playerStatus;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float _minPitch;
    [SerializeField][Range(0, 90)] private float _maxPitch;

    // 마우스를 회전시켰을 때, 얼마나 빨리 움직이느냐
    [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

    private Vector2 _currentRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStatus = GetComponent<PlayerStatus>();
    }

    // 이동
    public Vector3 SetMove(float moveSpeed)
    {
        Vector3 moveDirection = GetMoveDirection();

        Vector3 velocity = _rigidbody.velocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        _rigidbody.velocity = velocity;

        return moveDirection;
    }

    // Aim 회전
    public Vector3 SetAimRotation()
    {
        Vector2 mouseDirection = GetMouseDirection();

        // x축의 경우라면 제한을 걸 필요 없음.
        _currentRotation.x += mouseDirection.x;

        // y축의 경우엔 각도 제한을 걸어야 함.
        _currentRotation.y = Mathf.Clamp(_currentRotation.y + mouseDirection.y, _minPitch, _maxPitch);

        // 캐릭터 오브젝트의 경우에는 Y축 회전만 반영
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

        // 에임의 경우 상하 회전 반영
        // 현재 에임이 몇 도 기울어져있는가 가져오기
        Vector3 currentEuler = _aim.localEulerAngles;
        _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

        // 회전 방향 벡터 반환
        Vector3 rotateDirVector = transform.forward;

        // y축에 대한 회전은 적용되지 않게
        rotateDirVector.y = 0;

        return rotateDirVector.normalized;
    }

    // 몸 회전
    public void SetAvatarRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _avatar.rotation = Quaternion.Lerp(_avatar.rotation, targetRotation, _playerStatus.RotateSpeed * Time.deltaTime);
    }

    // 마우스는 x, y만 있으므로 Vector2 사용
    private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        // -로 하고 곱해줘야 원하는 방향으로 움직임
        float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

        return new Vector2(mouseX, mouseY);
    }

    public Vector3 GetMoveDirection()
    {
        Vector3 input = GetInputDirection();

        Vector3 direction = 
            // 옆 방향
            (transform.right * input.x) + 
            // 앞 방향
            (transform.forward * input.z);

        // 정규화
        return direction.normalized;
    }

    public Vector3 GetInputDirection()
    {
        // 게임패드가 들어가면 GetAxis로 변경
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 현재 점프가 없으니 y는 지정 안 함
        return new Vector3(x, 0, z);
    }
}
