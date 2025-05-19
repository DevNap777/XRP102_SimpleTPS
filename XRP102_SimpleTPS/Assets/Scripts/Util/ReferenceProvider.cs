using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 게임 오브젝트에는 딱 하나의 프로바이더만 가질 수 있도록 설정
[DisallowMultipleComponent]
public class ReferenceProvider : MonoBehaviour
{
    // Provider가 어떤 것에 대한 참조를 가지고 있어야 하는가?
    [SerializeField] private Component _component;

    private void Awake() => ReferenceRegistry.Register(this);

    private void OnDestroy() => ReferenceRegistry.Unregister(this);
    
    // 자기가 원하는 형태로 변환해서 반환
    // 여기 들어오는 T는 컴포넌트를 상속받아야 한다.
    public T GetAs<T>() where T : Component
    {
        // 여기에서도 예외처리 필요
        // 예외처리 없이 심플하게 적으면 이게 끝
        return _component as T;
    }
}
