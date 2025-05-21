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
    /// �ʱ�ȭ�� �Լ�, ��ü ������ �ʿ��� �ʱ�ȭ �۾��� �ִٸ� ���⼭ �����Ѵ�.
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

    // �÷��̾� ���� ���� �Լ�
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
        // Aim�����̰� Ű���� ������ ����
        //if (_status.IsAttacking.Value && Input.GetKey(_shootKey))

        // _shootInputAction.WasPressedThisFrame() => �̹� �����ӿ� ���ȴ°�? (GetKeyDown)
        // _shootInputAction.WasReleaseddThisFrame() => �̹� �����ӿ� �������°�? (GetKeyUp)
        // _shootInputAction.IsPressed() => ���� �������°�? (GetKey)

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
        // moveDir�� Vector3�� ������ �ƴ϶�� IsMovint.Value�� �� ����
        _status.IsMoving.Value = moveDir != Vector3.zero;

        //��ü�� ȸ�� ��� ���� �� ȣ���ؾ� ��.
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
        // Aim ������ ����
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

        // ���� ���·� �����ϰ� �ʹٸ�?
        // 1. Key Down ��Ȳ�϶� => Ű �Է��� ���۵� �������� üũ
        // 2. Key Up ��Ȳ�϶� => Ű �Է��� ���۵� �������� üũ
        _status.IsAiming.Value = ctx.started;

        // ctx.started => Ű �Է��� ���۵ƴ��� �Ǻ�
        // ctx.performed => Ű �Է��� ���������� �Ǻ�
        // ctx.canceled => Ű �Է��� ��ҵƴ���(����������) �Ǻ�
    }

    public void TakeDamage(int value)
    {
        // ü���� ����߸���, ü���� 0 ���ϰ� �Ǹ� �÷��̾ �׵��� ó����
        _status.CurrentHp.Value -= value;

        if (_status.CurrentHp.Value <= 0)
        {
            Dead();
        }
    }

    public void RecoveryHp(int value)
    {
        // ü���� ȸ����Ű��, MaxHp �ʰ��� �Ǵ� ���� ���ƾ� ��.
        int hp = _status.CurrentHp.Value + value;

        _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHP);
    }

    public void Dead()
    {
        // TODO : �ָ� ���� ������, �������ͽ��� ��� ����
        Debug.Log("�÷��̾� ��� ó��");
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
