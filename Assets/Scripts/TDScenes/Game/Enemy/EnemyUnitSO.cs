using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyUnitSO : ScriptableObject, IUnit
{
    public GameObject enemyBuilding;
    private int _attack, _health, _defencePower;
    public int CurrentHealth;
    private int _speed;
    public int Attack { get => _attack; set => _attack = value; }
    public int Speed { get => _speed; set => _speed = value; }
    public int Health { get => _health; set => _health = value; }
    public int DefencePower { get => _defencePower; set => _defencePower = value; }

  
}
