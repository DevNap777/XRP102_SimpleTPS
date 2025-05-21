using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ʈ����Ʈ �߰�
[CreateAssetMenu(fileName = "HpPotion", menuName = "Scriptable Object/Hp Potion", order = 1)]

// ��ũ���ͺ� ������Ʈ�� ��� �޵��� ����
public class HpPosion : ItemData
{
    public int Value;

    public override void Use(PlayerController2 controller2)
    {
        controller2.Recover(Value);
    }
}
