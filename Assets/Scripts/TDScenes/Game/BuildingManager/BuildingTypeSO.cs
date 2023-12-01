using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToweDefence.Scenes.Game.BuildingManager
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    [CreateAssetMenu()]
    public class BuildingTypeSO : ScriptableObject
    {
        public Transform prefab;
        public int price,count;
        public Sprite icon;
        public Vector3[] cells;
        public Direction direction;
        public string title;
        public AttackJob attackJob;
        public DefenceJob defenceJob;
        public int GetDirectionAngle() =>((int)direction * 90) % 360;

        public void ChangeDirection()
        {
            direction = (Direction)(((int)direction + 1) % Enum.GetValues(typeof(Direction)).Length);
            CalculateCells();
        }

        private void CalculateCells()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i]= new Vector3(cells[i].y* -1, cells[i].x, 0);
   
            }
        }


        void OnDisable()
        {
            count = 0;
            while (direction != Direction.Up)
            {
                ChangeDirection();
            }
        }
    }

}