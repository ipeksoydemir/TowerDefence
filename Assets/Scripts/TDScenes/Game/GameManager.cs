using System;
using System.Collections;
using System.Collections.Generic;
using TDScenes.Game.Units;
using Tools.ObserverPattern;
using ToweDefence.Scenes.Game.BuildingManager;

using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using TDScenes.Game.Grid;
using Tools.HSM;

public class GameManager : MonoBehaviour, IObservable
{
    private int balance =1300; // levelden gelcek
    private int wellness = 200; // leveldan gelcek
    private int borderline = 4; // leveldan gelecek 'enemyside' 
    [SerializeField] private EnemyUnitSO enemy; // leveldan gelecek
    private int _currentTotalHealth=0;
    [SerializeField]
    private GameStateMachine gameStateMachine;

    public void IncreaseCurrentTotalHealth(int value)
    {
        Health += value;
    }
    public void DecreaseCurrentTotalHealth(int value)
    {
        Health -= value;

    }
    private List<BuildingTypeSO> _myBuildings = new List<BuildingTypeSO>();


    public static GameManager Instance { get; private set; }

    private Action<PlayerUnitSO> OnFight;
    private bool _willBeFight;
    private static List<PlayerUnitSO> _mySoldiers = new List<PlayerUnitSO>();
    private List<Transform> _soldiers = new List<Transform>();
    public void AddToMyBuildings(BuildingTypeSO buildingType)
    {
       /* _myBuildings.Add(buildingType);
        if(buildingType.attackJob != null)
        {
            _mySoldiers.AddRange(buildingType.attackJob.playerSoldiers);
            foreach (var soldier in buildingType.attackJob.playerSoldiers)
            {
                IncreaseCurrentTotalHealth(soldier.Health);
            }
        }*/
    }

    public static void AddToMyMilitary(List<PlayerUnitSO> soldiers)
    {
        _mySoldiers.AddRange(soldiers);

    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Balance = 1200;
        OnFight += HandleAttack;
        _currentTotalHealth = wellness;
    }

   

    void GetFightInfo(bool wantYouFight)
    {
        _willBeFight = wantYouFight;
    }


    async void HandleAttack(PlayerUnitSO warrior)
    {
        // sawaşçının attack değeri kadar enemy e hasar verir
        // defence değeri kadar enemyninin attack değerini soğurur
        // enemy'den gelen defence benim defencedan büyükse kalan kısmı 
        // current healthdan ve  DecreaseCurrentTotalHealth den düşer
        // current health health değeri 0 olunca asker ölür
        // _mySoldiers tan silinir
        // _mySoldiers count 0 sa oyunu kaybettim
        //-----------------------------------------
        // Enemy update de bakıcak benim bölgemde soldier var mı?
        // Yoksa attack değeri ile salidrcak ve playerın DecreaseCurrentTotalHealth den düşer
        // benim bölgemde soldier varsa defence kadar benim atağımı soğurmaya çalışçak


    }


    public  int Balance
    {
        get
        {
               return balance;
        }
        set
        {
            if (balance != value)
            {
                balance = value;
                Notify();
            }
        }

    }
    private void OnDead()
    {
        gameStateMachine.ChangeStateToLost();
    }
    public int Health
    {
        get
        {
            return _currentTotalHealth;
        }
        set
        {
            if (_currentTotalHealth != value)
            {
                _currentTotalHealth = value;
                Notify();
                if (_currentTotalHealth <= 0)
                    OnDead();

            }
        }

    }
    private static List<IObserver> _observers = new List<IObserver>();

    public void Detach(IObserver observer)
    {
        if (!_observers.Contains(observer))
            return;
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.HandleNotification();
        }
    }

    public void Attach(IObserver observer)
    {
        if (observer == null)
            return;
        _observers.Add(observer);
    }



}
