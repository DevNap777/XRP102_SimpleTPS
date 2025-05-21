using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> _slots = new();
    private PlayerController2 _controller2;

    private void Awake() => Init();

    private void Init()
    {
        _controller2 = GetComponent<PlayerController2>();
    }


    public void GetItem(ItemData itemData)
    {
        _slots.Add(itemData);
    }

    public void UseItem(int index)
    {
        // ��������� �̷��Ե� �� �� ����
        //_slots[index] = null;

        _slots[index].Use(_controller2);
        // �����ۿ��� ��� ����� �־����
        _slots.RemoveAt(index);
    }
}
