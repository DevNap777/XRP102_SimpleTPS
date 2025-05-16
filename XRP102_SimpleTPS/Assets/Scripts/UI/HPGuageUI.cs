using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGuageUI : MonoBehaviour
{
    // 1. LookAt으로 메인 카메라를 바라보게 한다. => 카메라 위치에 따라서 HP게이지가 기울어짐
    // 2. 현재 카메라의 방향으로 회전. 즉, 카메라의 방향 벡터를 적용
    // 3. 카메라의 반대 방향 벡터 적용

    // 필드로 설정한 대상 이지미를 설정
    [SerializeField] private Image _image;

    private Transform _cameraTransform;

    private void Awake() => Init();

    private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);

    private void Init()
    {
        _cameraTransform = Camera.main.transform;
    }

    // UI 게이지의 FillAmount를 표시 대상의 HP로 설정
    // 현재 수치 / 최대 수치
    public void SetImageFillAmount(float value)
    {
        _image.fillAmount = value;
    }

    public void SetUIForwardVector(Vector3 target)
    {
        transform.forward = target;
    }

}
