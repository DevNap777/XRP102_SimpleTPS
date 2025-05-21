using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private int _hp;
    private Inventory _inventory;

    private void Awake() => Init();


    private void OnTriggerEnter(Collider other)
    {
        // �κ��丮���� ������ �߰� ����� �־�� ��.
        // �浹�Ѱ� �������� �´����� �翬�� üũ�ؾ���. ���⿡�� �����Ǿ� ���� ����.
        _inventory.GetItem(other.GetComponent<ItemObject>().Data);
        other.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inventory.UseItem(0);
        }
    }

    private void Init()
    {
        _inventory = GetComponent<Inventory>();
    }

    public void Recover(int value)
    {
        _hp += value;
    }
}
