using System;
using UnityEngine;

namespace TDScenes.Game.Grid
{
    
    public class Grid
    {
        private int _width;
        private int _height;
        private int _size;
        public static Action<bool> MoveableCamera;
        public Vector2[] GridPositions { get; private set; }
    
        public Grid(int width, int height, int size=1)
        {
            _width = width > 0 ? width : throw new ArgumentException("Width must be positive", nameof(width));
            _height = height > 0 ? height : throw new ArgumentException("Height must be positive", nameof(height));
            _size = size >=0 ? size : throw new ArgumentException("Size must be higher than 0", nameof(height));
            MoveableCamera.Invoke(width>27 || height>11);
         
            CreatePositions();
        }

        private void CreatePositions()
        {
            GridPositions = new Vector2[_width * _height];
            Vector2 position = new Vector2();

            for (int x = 0, i = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++, i++)
                {
                    position.x = x * _size;
                    position.y = y * _size;
                    GridPositions[i] = position;
                }
            }
        }
       
    }
}
