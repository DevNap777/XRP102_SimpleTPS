using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에트리뷰트 추가
[CreateAssetMenu(fileName = "HpPotion", menuName = "Scriptable Object/Hp Potion", order = 1)]

// 스크립터블 오브젝트를 상속 받도록 설정
public class HpPosion : ItemData
{
    public int Value;

    public override void Use(PlayerController2 controller2)
    {
        controller2.Recover(Value);
    }
}
