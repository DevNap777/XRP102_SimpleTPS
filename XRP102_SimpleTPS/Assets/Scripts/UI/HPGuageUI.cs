using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGuageUI : MonoBehaviour
{
    // 1. LookAt���� ���� ī�޶� �ٶ󺸰� �Ѵ�. => ī�޶� ��ġ�� ���� HP�������� ������
    // 2. ���� ī�޶��� �������� ȸ��. ��, ī�޶��� ���� ���͸� ����
    // 3. ī�޶��� �ݴ� ���� ���� ����

    // �ʵ�� ������ ��� �����̸� ����
    [SerializeField] private Image _image;

    private Transform _cameraTransform;

    private void Awake() => Init();

    private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);

    private void Init()
    {
        _cameraTransform = Camera.main.transform;
    }

    // UI �������� FillAmount�� ǥ�� ����� HP�� ����
    // ���� ��ġ / �ִ� ��ġ
    public void SetImageFillAmount(float value)
    {
        _image.fillAmount = value;
    }

    public void SetUIForwardVector(Vector3 target)
    {
        transform.forward = target;
    }

}
