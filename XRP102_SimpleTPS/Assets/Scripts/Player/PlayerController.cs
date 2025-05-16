using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;
    private Animator _animator;
    private Image _aimImage;

    [SerializeField] private CinemachineVirtualCamera _aimCamera;
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _aimAnimator;
    [SerializeField] private HPGuageUI _hpUI;

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

        // Test Code-----------------
        //_hpUI.SetImageFillAmount(1);
        //_status.CurrentHp.Value = _status.MaxHP;
    }   

    // 플레이어 조작 메인 함수
    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        HandleAiming();
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

    private void HandleShooting()
    {
        // Aim상태이고 키까지 눌리면 수행
        if (_status.IsAttacking.Value && Input.GetKey(_shootKey))
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
            Vector3 input = _movement.GetInputDirection();
            _animator.SetFloat("X", input.x);
            _animator.SetFloat("Z", input.z);
        }
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
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
    }

    public void UnsubscribeEvents()
    {
        _status.CurrentHp.Unsubscribe(SetHPUIGuage);
        _status.IsMoving.Unsubscribe(SetMoveAnimation);

        _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
        _status.IsAiming.Unsubscribe(SetAimAnimation);
        _status.IsAttacking.Unsubscribe(SetAttackAnimation);
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
