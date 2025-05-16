using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField][Range(0, 100)] private float _attackRange;
    [SerializeField] private int _shootDamage;
    [SerializeField] private float _shootDelay;
    [SerializeField] private AudioClip _shootSFX;

    private CinemachineImpulseSource _impulse;
    // 화면 기준으로 레이케스트를 쏠 거임
    private Camera _camera;

    // _currentCount가 0으로 떨어지면 true로 반영
    private bool _canShoot { get => _currentCount <= 0; }
    private float _currentCount;

    private void Awake() => Init();

    private void Update() => HandleCanShoot();


    private void Init()
    {
        _camera = Camera.main;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    // 아래의 모든 것들을 관리해줄 핸들러
    public bool Shoot()
    {
        if (!_canShoot) return false;

        PlayShootSound();
        PlayCameraEffect();
        PlayShootEffect();
        _currentCount = _shootDelay;

        // TODO : Ray 발사 -> 반환받은 대상에게 데미지 부여. 몬스터 구현시 같이 구현

        IDamagable target = RayShoot();

        if (target == null) return true;

        target.TakeDamage(_shootDamage);

        return true;
    }

    private IDamagable RayShoot()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
        {
            // ??? 이 부분을...? 어떻게 우회해야 하지...?
            return hit.transform.GetComponent<IDamagable>();
        }

        return null;
    }

    private void HandleCanShoot()
    {
        if (_canShoot) return;

        _currentCount -= Time.deltaTime;
    }

    // 시네머신을 사용해서 화면이 흔들리도록 하는 효과
    private void PlayShootSound()
    {
        // Instance로 싱글톤으로 호출
        SFXController sfx = GameManager.Instance.Audio.GetSFX();
        sfx.Play(_shootSFX);
    }

    private void PlayCameraEffect()
    {
        _impulse.GenerateImpulse();
    }

    private void PlayShootEffect()
    {
        // TODO : 총구 화염 효과. 파티클로 구현할 거임
    }
}
