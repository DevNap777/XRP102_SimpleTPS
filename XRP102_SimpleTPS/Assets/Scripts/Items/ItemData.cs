using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HP������ ��ӹ��� �� �ֵ��� abstract�� ����
public abstract class ItemData : ScriptableObject
{
    public string Name;

    // �ؽ�Ʈ�� �����ٷ� ������ ����
    [TextArea] public string Description;

    // �κ��丮�� ���� ������ �ְ� ����
    public Sprite Icon;

    public GameObject prefab;

    public abstract void Use(PlayerController2 controller2);
}
