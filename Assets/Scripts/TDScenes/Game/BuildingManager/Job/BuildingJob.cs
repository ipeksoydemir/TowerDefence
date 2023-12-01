using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDScenes.Game.Units;
using ToweDefence.Scenes.Game.BuildingManager;
using UnityEngine;

public class BuildingJob : MonoBehaviour
{
    [SerializeField] private BuildingTypeSO type;
    private List<Transform> _soldiers = new List<Transform>();
    private Transform soldierPrefab;
    private bool _rent=false;

    private void Awake()
    {   // uygun gridlerdde gorevini yap = Listedeki unitleri(askerleri) instantiate et

        if (type.attackJob != null)
        {
            ScriptableObject.CreateInstance<PlayerUnitSO>();
            BuildingManager.UnitsPositionResponse += InstantiateSoldiers;
            PathFinder.PathCreated += HandlePath;
            //  PathFinder.WantYouFight += GetFightInfo;
            // GridManager.MoveToSoldier += FindEmptyGrid;
            //OnFight += HandleAttack;
            soldierPrefab = type.attackJob.playerSoldiers[0].prefab;
        }
        

    }

 
    async void FindEmptyGrid(Vector2[] grids)
    {
        Vector2 currentGrid = grids[0];
        
        //PlayerUnitSO soldierUnit= FindSoldier(currentGrid);
        GameObject soldier = FindSoldier(currentGrid);

        if (soldier != null)
        {
            await Move(soldier, grids.ToList(),0);
        }
    }
   /* PlayerUnitSO FindSoldier(Vector2 currentPosition)
    {
        Vector3 position = currentPosition;
        foreach (var soldier in _mySoldiers)
        {
            if (soldier.currentPosition.position == position)
                return soldier;
        }

        return null;
    }*/
   GameObject FindSoldier(Vector2 c)
   {
       Vector3 current = c;
       GameObject soldier = new GameObject();

       foreach (var s in _soldiers)
       {
           if (s.position == current)
               soldier = s.gameObject;
       }

       return soldier;
   }
    private async void HandlePath(List<Vector2> path)
    {

      //  PlayerUnitSO soldierUnit = FindSoldier(path[0]);
        //GameObject soldier = soldierUnit?.prefab.gameObject;
        

        GameObject soldier = FindSoldier(path[0]);
        if (soldier != null)
        {
            await Move(soldier, path,1);

        }
       /* if (_willBeFight)
        {
            PlayerUnitSO warrior = soldierUnit;
            OnFight.Invoke(warrior);
        }
        else
        {
            //savaş bitti
        }*/
    }
    
    public async Task Move(GameObject soldier, List<Vector2> path,int speed)
    {

        foreach (var point in path)
        {
            // Askerin pozisyonunu güncelle
            soldier.transform.position = point;

            //todo:speed 
            await Task.Delay((int)400); //await Task.Delay(Speed);
        }

    }



    private async void Update()
    {
        if (!_rent && type.defenceJob != null)
        {
            _rent = true;
            await Rent();

        }
    }

    public async Task Rent()
    {
        GameManager.Instance.Balance += 10;

        await Task.Delay((int)1400);
        _rent = false;
    }

    private void InstantiateSoldiers(List<Vector2> positions)
    {
        BuildingManager.UnitsPositionResponse -= InstantiateSoldiers;
        int index = 0; 
        foreach (var position in positions)
        {
            _soldiers.Add(Instantiate(soldierPrefab, position, Quaternion.identity));
            type.attackJob.playerSoldiers[index].currentPosition.position = position;
            type.attackJob.playerSoldiers[index].CurrentHealth = type.attackJob.playerSoldiers[index].Health;
            index++;
            GameManager.AddToMyMilitary(type.attackJob.playerSoldiers);
            foreach (var soldier in type.attackJob.playerSoldiers)
            {

                GameManager.Instance.Health += soldier.Health;
            }
        }
    }
  

}
