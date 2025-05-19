using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϳ��� ���� ������Ʈ���� �� �ϳ��� ���ι��̴��� ���� �� �ֵ��� ����
[DisallowMultipleComponent]
public class ReferenceProvider : MonoBehaviour
{
    // Provider�� � �Ϳ� ���� ������ ������ �־�� �ϴ°�?
    [SerializeField] private Component _component;

    private void Awake() => ReferenceRegistry.Register(this);

    private void OnDestroy() => ReferenceRegistry.Unregister(this);
    
    // �ڱⰡ ���ϴ� ���·� ��ȯ�ؼ� ��ȯ
    // ���� ������ T�� ������Ʈ�� ��ӹ޾ƾ� �Ѵ�.
    public T GetAs<T>() where T : Component
    {
        // ���⿡���� ����ó�� �ʿ�
        // ����ó�� ���� �����ϰ� ������ �̰� ��
        return _component as T;
    }
}
