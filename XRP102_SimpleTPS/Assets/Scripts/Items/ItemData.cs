using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HP포션이 상속받을 수 있도록 abstract로 설정
public abstract class ItemData : ScriptableObject
{
    public string Name;

    // 텍스트가 여러줄로 나오게 해줌
    [TextArea] public string Description;

    // 인벤토리에 넣을 아이콘 넣게 해줌
    public Sprite Icon;

    public GameObject prefab;

    public abstract void Use(PlayerController2 controller2);
}
