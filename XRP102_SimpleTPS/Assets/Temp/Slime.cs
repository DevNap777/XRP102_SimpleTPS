using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public DataManager Data;
    public MonsterType Type;

    [SerializeField] private string _name;
    [SerializeField] private int _atk;
    [SerializeField] private int _dfe;
    [SerializeField] private int _spd;
    [SerializeField] private string _dsc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Init(1);
    }

    private void Init(int type)
    {
        _name = Data.MonsterDic.GetData(name, "이름");
        _atk = int.Parse(Data.MonsterDic.GetData(name, "공격력"));
        _atk = int.Parse(Data.MonsterDic.GetData(name, "방어력"));
        _atk = int.Parse(Data.MonsterDic.GetData(name, "이동속도"));
        _name = Data.MonsterDic.GetData(name, "설명");
        //_name = Data.MonsterCSV.GetData((int)type, (int)MonsterData.Name);
        //_atk = int.Parse(Data.MonsterCSV.GetData((int)type, (int)MonsterData.Atk));
        //_dfe = int.Parse(Data.MonsterCSV.GetData((int)type, (int)MonsterData.Dfe));
        //_spd = int.Parse(Data.MonsterCSV.GetData((int)type, (int)MonsterData.Spd));
        //_dsc = Data.MonsterCSV.GetData((int)type, (int)MonsterData.Dsc);
    }
}

public enum MonsterType
{
    Slime = 1,
    Trallla
}