using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    [SerializeField] private InputAction _testKey;

    private void TestMethod(InputAction.CallbackContext ctx)
    {
        Debug.Log("!!!!");
    }

    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;
    private Image _aimImage;

    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _aimAnimator;
    [SerializeField] private HPGuageUI _hpUI;
    private InputAction _aimInputAction;
    private InputAction _shootInputAction;

    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;

    private void Awake() => Init();
    private void OnEnable() => SubscribeEvents();
    private void Update() => HandlePlayerControl();
    private void OnDisable() => UnsubscribeEvents();

    /// <summary>
    /// 초기화용 함수, 객체 생성시 필요한 초기화 작업이 있다면 여기서 수행한다.
    /// </summary>
    private void Init()
    {
        _status = GetComponent<PlayerStatus>();
        _movement = GetComponent<PlayerMovement>();
        //_mainCamera = Camera.main.gameObject;
        _animator = GetComponent<Animator>();
        _aimImage = _aimAnimator.GetComponent<Image>();
        _aimInputAction = GetComponent<PlayerInput>().actions["Aim"];

        // Test Code-----------------
        //_hpUI.SetImageFillAmount(1);
        //_status.CurrentHp.Value = _status.MaxHP;
    }   

    // 플레이어 조작 메인 함수
    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        // HandleAiming();
        HandleShooting();

        // Test Code-----------------
        //if (Input.GetKey(KeyCode.Alpha1))
        //{
        //    _status.CurrentHp.Value--;
        //}
        //
        //if (Input.GetKey(KeyCode.Alpha2))
        //{
        //    _status.CurrentHp.Value++;
        //}
    }

    //private void HandleShooting()
    public void HandleShooting()
    {
        // Aim상태이고 키까지 눌리면 수행
        //if (_status.IsAttacking.Value && Input.GetKey(_shootKey))

        // _shootInputAction.WasPressedThisFrame() => 이번 프레임에 눌렸는가? (GetKeyDown)
        // _shootInputAction.WasReleaseddThisFrame() => 이번 프레임에 떼어졌는가? (GetKeyUp)
        // _shootInputAction.IsPressed() => 지금 눌려졌는가? (GetKey)

        if (_status.IsAttacking.Value && _shootInputAction.IsPressed())
        {
            _status.IsAttacking.Value = _gun.Shoot();
        }
        else
        {
            _status.IsAttacking.Value = false;
        }
    }

    private void HandleMovement()
    {
        Vector3 camRotateDir = _movement.SetAimRotation();

        float moveSpeed;
        if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
        else moveSpeed = _status.RunSpeed;

        Vector3 moveDir = _movement.SetMove(moveSpeed);
        // moveDir이 Vector3의 영점이 아니라면 IsMovint.Value에 값 대입
        _status.IsMoving.Value = moveDir != Vector3.zero;

        //몸체의 회전 기능 구현 후 호출해야 함.
        Vector3 avatarDir;
        if (_status.IsAiming.Value)
        {
            avatarDir = camRotateDir;
        }
        else
        {
            avatarDir = moveDir;
        }

        _movement.SetAvatarRotation(avatarDir);

        // SetAnimationParameter
        // Aim 상태일 때만
        if (_status.IsAiming.Value)
        {
            //Vector3 input = _movement.GetInputDirection();
            //_animator.SetFloat("X", input.x);
            //_animator.SetFloat("Z", input.z);

            _animator.SetFloat("X", _movement.InputDirection.x);
            _animator.SetFloat("Z", _movement.InputDirection.y);
        }
    }

    private void HandleAiming(InputAction.CallbackContext ctx)
    {
        // _status.IsAiming.Value = Input.GetKey(_aimKey);

        // 눌린 상태로 유지하고 싶다면?
        // 1. Key Down 상황일때 => 키 입력이 시작된 시점인지 체크
        // 2. Key Up 상황일때 => 키 입력이 시작된 시점인지 체크
        _status.IsAiming.Value = ctx.started;

        // ctx.started => 키 입력이 시작됐는지 판별
        // ctx.performed => 키 입력이 진행중인지 판별
        // ctx.canceled => 키 입력이 취소됐는지(떼어졌는지) 판별
    }

    public void TakeDamage(int value)
    {
        // 체력을 떨어뜨리되, 체력이 0 이하가 되면 플레이어가 죽도록 처리함
        _status.CurrentHp.Value -= value;

        if (_status.CurrentHp.Value <= 0)
        {
            Dead();
        }
    }

    public void RecoveryHp(int value)
    {
        // 체력을 회복시키되, MaxHp 초과가 되는 것을 막아야 함.
        int hp = _status.CurrentHp.Value + value;

        _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHP);
    }

    public void Dead()
    {
        // TODO : 주말 동안 만들어보기, 스테이터스는 어떻게 할지
        Debug.Log("플레이어 사망 처리");
    }

    public void SubscribeEvents()
    {
        _status.CurrentHp.Subscribe(SetHPUIGuage);
        _status.IsMoving.Subscribe(SetMoveAnimation);

        _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Subscribe(SetAimAnimation);
        _status.IsAttacking.Subscribe(SetAttackAnimation);

        // inputs ----
        _aimInputAction.Enable();
        _aimInputAction.started += HandleAiming;
        _aimInputAction.canceled += HandleAiming;
    }

    public void UnsubscribeEvents()
    {
        _status.CurrentHp.Unsubscribe(SetHPUIGuage);
        _status.IsMoving.Unsubscribe(SetMoveAnimation);

        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Unsubscribe(SetAimAnimation);
        _status.IsAttacking.Unsubscribe(SetAttackAnimation);

        // inputs ----
        _aimInputAction.Disable();
        _aimInputAction.started -= HandleAiming;
        _aimInputAction.canceled -= HandleAiming;
    }

    private void SetAimAnimation(bool value)
    {
        if (!_aimImage.enabled)
        {
            _aimImage.enabled = true;
        }
        _animator.SetBool("IsAim", value);
        _aimAnimator.SetBool("IsAim", value);

    }
    private void SetMoveAnimation(bool value) => _animator.SetBool("IsMove", value);
    private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);
   
    private void SetHPUIGuage(int currentHp)
    {
        float hp = currentHp / _status.MaxHP;
        _hpUI.SetImageFillAmount(hp);
    }
  
}
