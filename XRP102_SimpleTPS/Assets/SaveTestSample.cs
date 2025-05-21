using UnityEngine;
using CustomUtility.IO;

public class PlayerSaveDataExample : SaveData
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int Score { get; set; }

    public PlayerSaveDataExample()
    {
    }

    public PlayerSaveDataExample(string name, int hp, int score)
    {
        Name = name;
        Hp = hp;
        Score = score;
    }
}

public class SaveTestSample : MonoBehaviour
{
    private PlayerSaveDataExample _jsonSave;
    private PlayerSaveDataExample _jsonLoad;
    private PlayerSaveDataExample _binarySave;
    private PlayerSaveDataExample _binaryLoad;

    private void Start()
    {
        //SaveJson();
        //LoadJson();
        
        SaveBinary();
        LoadBinary();
    }

    private void SaveJson()
    {
        _jsonSave = new("ƒºƒºƒº", 55, 10);
        
        DataSaveController.Save(_jsonSave, SaveType.JSON);
    }

    private void LoadJson()
    {
        _jsonLoad = new("", 0, 0);

        DataSaveController.Load(ref _jsonLoad, SaveType.JSON);
        Debug.Log(_jsonLoad.Name);
        Debug.Log(_jsonLoad.Hp);
        Debug.Log(_jsonLoad.Score);
    }

    private void SaveBinary()
    {
        _jsonSave = new("πè", 60, 76);
        _jsonSave.Hp = 76;

        DataSaveController.Save(_jsonSave, SaveType.BINARY);
    }

    private void LoadBinary()
    {
        _jsonLoad = new("", 0, 0);

        DataSaveController.Load(ref _jsonLoad, SaveType.BINARY);
        Debug.Log(_jsonLoad.Name);
        Debug.Log(_jsonLoad.Hp);
        Debug.Log(_jsonLoad.Score);
    }
}
