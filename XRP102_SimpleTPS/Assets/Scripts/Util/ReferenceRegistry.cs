using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceRegistry
{
    private static Dictionary<GameObject, ReferenceProvider> _providers = new();

    public static void Register(ReferenceProvider referenceProvider)
    {
        if (_providers.ContainsKey(referenceProvider.gameObject)) return;

        //                           키                     값
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
        // 입력받은 게임 오브젝트가 키로 들어가있지 않다면
        // null을 반환
        if (!_providers.ContainsKey(gameObject)) return null;

        // 있다면 Dictionary에 게임 오브젝트를 추가해서 반환
        return _providers[gameObject];
    }
}
