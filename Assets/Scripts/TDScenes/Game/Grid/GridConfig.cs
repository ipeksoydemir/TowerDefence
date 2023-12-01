

using System;
using System.Collections.Generic;
using System.IO;
using Configs;
using Tools.ObserverPattern;
using UnityEngine;
using Newtonsoft.Json;

namespace TDScenes.Game.Grid
{
    public class GridConfig : MonoBehaviour,IObservable
    {
        private void Awake()
        {
            string path = Path.Combine(Application.persistentDataPath, "Grid.json");
           
              

                string jsonData = "{\n  \"GridWidth\": 25,\n  \"GridHeight\": 10\n}";


                File.WriteAllText(path, jsonData);
          
           

                string jsonreadData = File.ReadAllText(path);
                GridData loadedData = JsonConvert.DeserializeObject<GridData>(jsonreadData);
                this.size = new Vector2Int(loadedData.GridWidth, loadedData.GridHeight);
                this.GridWidth = loadedData.GridWidth;
                this.GridHeight = loadedData.GridHeight;

          

            //  string jsonFilePath = "Assets/Resource/Grid.json";
            //  LoadFromJson(jsonFilePath);
        }

        private void LoadFromJson(string jsonFilePath)
        {
            string json = System.IO.File.ReadAllText(jsonFilePath);
            GridData gridSizeData = JsonUtility.FromJson<GridData>(json);

            this.size = new Vector2Int(gridSizeData.GridWidth, gridSizeData.GridHeight);
            this.GridWidth = gridSizeData.GridWidth;
            this.GridHeight = gridSizeData.GridHeight;
        }

        public Vector2Int size;

        private void OnValidate(){
            print(size.x +": x , y:" +size.y );
            GridWidth = size.x;
            GridHeight = size.y;
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
            if(observer==null)
                return;
            _observers.Add(observer);
        }

        #region GridSize


        private int _gridWidth;

        public int GridWidth
        {
            get => _gridWidth;
            set
            {
                if (_gridWidth != value)
                {
                    _gridWidth = value;
                    Notify();
                }
            }
        }

        private int _gridHeight;

        public int GridHeight
        {
            get => _gridHeight;
            set
            {
                if (_gridHeight != value)
                {
                    _gridHeight = value;
                    Notify();
                }
            }
        }

        #endregion

    }

}