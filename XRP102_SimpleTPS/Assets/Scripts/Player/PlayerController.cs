using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : 참조 생성용 임시 네임스페이스 참조. 작업물 병합 시 삭제 예정
using PlayerMovement = B_Test.PlayerMovement;

public class PlayerController : MonoBehaviour
{
    public bool IsControlActivate { get; set; } = true;

    private PlayerStatus _status;
    private PlayerMovement _movement;

    [SerializeField] private GameObject _aimCamera;
    private GameObject _mainCamera;

    [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;

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
        _mainCamera = Camera.main.gameObject;
    }

    private void HandlePlayerControl()
    {
        if (!IsControlActivate) return;

        HandleMovement();
        HandleAiming();
    }

    private void HandleMovement()
    {
        // TODO: Movement 병합시 기능 추가예정
    }

    private void HandleAiming()
    {
        _status.IsAiming.Value = Input.GetKey(_aimKey);
    }

    public void SubscribeEvents()
    {
        _status.IsAiming.Subscribe(value => SetActivateAimCamera(value));
    }

    public void UnsubscribeEvents()
    {
        _status.IsAiming.Unsubscribe(value => SetActivateAimCamera(value));
    }

    private void SetActivateAimCamera(bool value)
    {
        _aimCamera.SetActive(value);
        _mainCamera.SetActive(!value);
    }
}
