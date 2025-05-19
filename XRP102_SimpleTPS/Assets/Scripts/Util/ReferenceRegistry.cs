using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceRegistry
{
    private static Dictionary<GameObject, ReferenceProvider> _providers = new();

    public static void Register(ReferenceProvider referenceProvider)
    {
        if (_providers.ContainsKey(referenceProvider.gameObject)) return;

        //                           Ű                     ��
        _providers.Add(referenceProvider.gameObject, referenceProvider);
    }

    public static void Unregister(ReferenceProvider referenceProvider)
    {
        if (!_providers.ContainsKey(referenceProvider.gameObject)) return;

        _providers.Remove(referenceProvider.gameObject);
    }
    
    public static void Clear()
    {
        _providers.Clear();
    }

    public static ReferenceProvider GetProvider(GameObject gameObject)
    {
        // �Է¹��� ���� ������Ʈ�� Ű�� ������ �ʴٸ�
        // null�� ��ȯ
        if (!_providers.ContainsKey(gameObject)) return null;

        // �ִٸ� Dictionary�� ���� ������Ʈ�� �߰��ؼ� ��ȯ
        return _providers[gameObject];
    }
}
