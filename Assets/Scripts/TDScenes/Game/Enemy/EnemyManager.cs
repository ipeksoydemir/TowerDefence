using Configs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TDScenes.Game.Grid;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] private EnemyUnitSO enemy;
    [SerializeField] private GameObject parent;
    private bool _attack = false;
    private Action Attack;


    private void LoadFromJson(string jsonFilePath)
    {
        string json = System.IO.File.ReadAllText(jsonFilePath);
        UnitData unitData = JsonUtility.FromJson<UnitData>(json);

        enemy.Attack = unitData.Attack;
        enemy.Speed = unitData.Speed;
        enemy.Health = unitData.Health;
        enemy.DefencePower = unitData.DefencePower;
    }
    private void Awake()
    {
        //JSONDAN OKUNCAK
        string path = Path.Combine(Application.persistentDataPath, "Enemy.json");
       
        

            string jsonData = "{\n  \"Attack\": 4,\n  \"Speed\": 1200,\n  \"Health\": 400,\n  \"DefencePower\": 10\n}";


            File.WriteAllText(path, jsonData);
      
            string jsonreadData = File.ReadAllText(path);
            UnitData loadedData = JsonUtility.FromJson<UnitData>(jsonreadData);
            enemy.Attack = loadedData.Attack;
            enemy.Speed = loadedData.Speed;
            enemy.Health = loadedData.Health;
            enemy.DefencePower = loadedData.DefencePower;
            GridManager.EnemyPosition += InstantiateEnemy;
       
        /*
        string jsonFilePath = "Assets/Resource/Enemy.json";
        LoadFromJson(jsonFilePath);
        GridManager.EnemyPosition += InstantiateEnemy;
        ScriptableObject.CreateInstance<EnemyUnitSO>();
        */
        // PathFinder'dan gelen  WantYouFight true ise sacunmadayım false ise ataktayım

    }
    void InstantiateEnemy(Vector2 pos)
    {
        Instantiate(enemy.enemyBuilding,pos, Quaternion.identity,parent.transform);
    }

    private async void Update()
    {
        if(!_attack)
        {
            _attack = true;
            await Atack();

        }
    }

    public async Task Atack()
    {

        GameManager.Instance.Health -= enemy.Attack;

        await Task.Delay((int)1400);
        _attack = false;
    }
}