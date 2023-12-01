using Configs;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerUnitType
{
    HorseSoldier, //atlısüvari
    FootSoldier //piyade
}
[CreateAssetMenu()]
public class PlayerUnitSO : ScriptableObject, IUnit
{

    private int _attack,_health,_defencePower;
    public int CurrentHealth;
    private int _speed;
    public int Attack { get => _attack; set => _attack=value; }
    public int Speed { get => _speed; set => _speed = value; }
    public int Health { get => _health; set => _health = value; }
    public int DefencePower { get => _defencePower; set => _defencePower = value; }

    public PlayerUnitType type;
    public Transform currentPosition,prefab;
    private void Awake()
    {
       
        string path = Path.Combine(Application.persistentDataPath, type.ToString() + ".json");
        string path1 = Path.Combine(Application.persistentDataPath,  "FootSoldier.json");



        string jsonData1 = "{\n  \"Attack\": 17,\n  \"Speed\": 400,\n  \"Health\": 30,\n  \"DefencePower\": 5\n}";


        File.WriteAllText(path1, jsonData1);
        
        string path2 = Path.Combine(Application.persistentDataPath, "HorseSoldier.json");



        string jsonData = "{\n  \"Attack\": 25,\n  \"Speed\": 80,\n  \"Health\": 10,\n  \"DefencePower\": 5\n}";


        File.WriteAllText(path2, jsonData);
       

        string jsonreadData = File.ReadAllText(path);
        UnitData loadedData = JsonConvert.DeserializeObject<UnitData>(jsonreadData);
        Attack = loadedData.Attack;
        Speed = loadedData.Speed;
        Health = loadedData.Health;
        DefencePower = loadedData.DefencePower;
        // string jsonFilePath = "Assets/Resource/" + type.ToString() +".json";
        // LoadFromJson(jsonFilePath);
    }
    private void LoadFromJson(string jsonFilePath)
    {
        string json = System.IO.File.ReadAllText(jsonFilePath);
        UnitData unitData = JsonUtility.FromJson<UnitData>(json);

        Attack = unitData.Attack;
        Speed = unitData.Speed;
        Health = unitData.Health;
        DefencePower = unitData.DefencePower;
    }

}
