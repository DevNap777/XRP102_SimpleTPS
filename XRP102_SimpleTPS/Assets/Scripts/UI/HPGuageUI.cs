using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPGuageUI : MonoBehaviour
{
    // 1. LookAt���� ���� ī�޶� �ٶ󺸰� �Ѵ�. => ī�޶� ��ġ�� ���� HP�������� ������
    // 2. ���� ī�޶��� �������� ȸ��. ��, ī�޶��� ���� ���͸� ����
    // 3. ī�޶��� �ݴ� ���� ���� ����

    private void LateUpdate()
    {
        SetUIRotate2();
    }

    // Test 2
    private void SetUIRotate2()
    {
        //transform.rotation = Camera.main.transform.rotation;
        transform.forward = Camera.main.transform.forward;
    }

}
