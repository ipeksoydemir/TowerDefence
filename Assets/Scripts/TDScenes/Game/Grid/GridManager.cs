using System;
using System.Collections.Generic;
using Tools.ObjectPooling;
using Tools.ObserverPattern;
using ToweDefence.Scenes.Game.BuildingManager;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TDScenes.Game.Grid
{
    public enum CellContent
    {
        Empty, Building, Soldier, Enemy
    }
    public class GridManager:MonoBehaviour,IObserver
    {
    
        [SerializeField] private AssetReference cell,enemy;
        [SerializeField] private Transform gameBoard;
        [SerializeField] private GridConfig configFile;
        private int _enemySide = 5;//leveldan gelecek
        private Grid _grid;
        private Dictionary<CellContent, List<Vector2>> _gridDict=new Dictionary<CellContent, List<Vector2>>();
        public static event Action<List<Vector2>> UnitsPositionResponse;
        private GameObject _cellPrefab,_enemyPrefab;
        public static Action<Vector2> EnemyPosition;
        public void HandleNotification()
        {
            ClearGrid();
            _grid = new Grid(configFile.GridWidth, configFile.GridHeight);
            //Rebuild
            BuildGrid();
        }
        private void OnEnable()
        {
            BuildingManager.BuildingCheck += CanBuild;
            BuildingManager.UnitsPositionReguest += GetEmptyPosition;
        }

        private void OnDisable()
        {
            BuildingManager.BuildingCheck -= CanBuild;
        }

        private async void Start()
        {
            configFile.Attach(this);

             _cellPrefab = await cell.LoadAssetAsync<GameObject>().Task;
             _enemyPrefab = await enemy.LoadAssetAsync<GameObject>().Task;
            if (_cellPrefab == null || _enemyPrefab==null)
            {
                Debug.LogError("Failed to load cell prefab.");
                return;
            }

            HandleNotification();
        }

        private void ClearGrid()
        {
            foreach (Transform child in gameBoard)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void BuildGrid()
        {
            ClearGrid();
            _grid = new Grid(configFile.GridWidth, configFile.GridHeight);
            List<Vector2> emptyGridPositions = new List<Vector2>();
            List<Vector2> enemyGridPositions = new List<Vector2>();
            if(_cellPrefab!=null || _enemyPrefab!=null)
            {
                foreach (var position in _grid.GridPositions)
                {
                    if (position.x > configFile.GridWidth - _enemySide)
                    {
                        Instantiate(_enemyPrefab, position, Quaternion.identity, gameBoard);
                        enemyGridPositions.Add(position);
                    }
                    else
                    {
                        emptyGridPositions.Add(position);
                        Instantiate(_cellPrefab, position, Quaternion.identity, gameBoard);
                    }

                }
            }
            _gridDict[CellContent.Empty] = emptyGridPositions;
            _gridDict[CellContent.Building] = new List<Vector2>();
            _gridDict[CellContent.Soldier] = new List<Vector2>();
            _gridDict[CellContent.Enemy] = enemyGridPositions;
          
            EnemyPosition.Invoke(new Vector2(configFile.GridWidth - 1, 0));
        }



        /// <summary>
        ///  Tüm hücreler uygun olduğunda tetiklenecek olay
        /// </summary>
        public static event Action<bool> OnBuildAreaChecked;

        public static event Action<Vector2[]> MoveToSoldier;

       
        private void CanBuild(Vector3[] gridPos)
        {
            List<Vector2> _buildingGrids = _gridDict[CellContent.Building];

            if (_buildingGrids != null)
            {
                foreach (var item in gridPos)
                {
                    if (_gridDict[CellContent.Enemy].Contains(item)||_buildingGrids.Contains(item) || item.x > configFile.GridWidth-1 || item.y> configFile.GridHeight-1 || item.x<0 || item.y<0)
                    {
                        OnBuildAreaChecked?.Invoke(false);
                        return;
                    }

                    if (_gridDict[CellContent.Soldier].Contains(item))
                    {
                        Vector2[] info = new Vector2[]
                        {
                            item,GetEmptyPosition()
                        };
                        print("MOVEE"+ info[0] + info[1]);
                        UpdateCellContent(info[0],CellContent.Building);
                        UpdateCellContent(info[1],CellContent.Soldier);
                        MoveToSoldier?.Invoke(info);
                    }
                    
                    // asker varsa event tetiklencek
                }
            }
            RecordBuild(gridPos);
            OnBuildAreaChecked?.Invoke(true);
           
            return ;
        }
       
        private void RecordBuild(Vector3[] gridPos)
        {
            foreach(var position in gridPos)
            {
                _gridDict[CellContent.Building].Add(position);
                _gridDict[CellContent.Empty].Remove(position);
            }
        }

        private Vector2 GetEmptyPosition()
        {
                Vector2 cell = _gridDict[CellContent.Empty][0];
               
                _gridDict[CellContent.Empty].Remove(cell);
                _gridDict[CellContent.Soldier].Add(cell);
           
            return cell;
        }

        private void GetEmptyPosition(int count)
        {
            List<Vector2> emptyPositions = new List<Vector2>();
            for (int i = 0; i < count; i++)
            {
                Vector2 cell = _gridDict[CellContent.Empty][0];
                emptyPositions.Add(cell);
                _gridDict[CellContent.Empty].Remove(cell);
                _gridDict[CellContent.Soldier].Add(cell);
                
            }
            UnitsPositionResponse?.Invoke(emptyPositions);
            
        }
        public List<Vector2> GetWalkableGrids()
        {
            var walkableTiles = new List<Vector2>();

            if (_gridDict.ContainsKey(CellContent.Empty))
            {
                walkableTiles.AddRange(_gridDict[CellContent.Empty]);
            }
            if (_gridDict.ContainsKey(CellContent.Soldier))
            {
                walkableTiles.AddRange(_gridDict[CellContent.Soldier]);
            }

            return walkableTiles;
        }

        public bool IsCellType(Vector2 position, CellContent content)
        {
            if(_gridDict[content]==null)
                return false;
            return _gridDict[content].Contains(position);
        }

        public void UpdateCellContent(Vector2 cell, CellContent content)
        {
            foreach (var key in Enum.GetValues(typeof(CellContent)))
            {
                if(_gridDict[(CellContent)key]!=null)
                    _gridDict[(CellContent)key].Remove(cell);
            }

            if (!_gridDict[content].Contains(cell))
            {
                _gridDict[content].Add(cell);
            }

        }
    }
}