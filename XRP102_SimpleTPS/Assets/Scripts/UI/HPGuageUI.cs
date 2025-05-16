using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPGuageUI : MonoBehaviour
{
    // 1. LookAt으로 메인 카메라를 바라보게 한다. => 카메라 위치에 따라서 HP게이지가 기울어짐
    // 2. 현재 카메라의 방향으로 회전. 즉, 카메라의 방향 벡터를 적용
    // 3. 카메라의 반대 방향 벡터 적용

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
