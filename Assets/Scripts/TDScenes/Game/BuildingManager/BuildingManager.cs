using System;
using System.Collections.Generic;
using System.Linq;
using TDScenes.Game.Grid;
using UnityEngine;

namespace ToweDefence.Scenes.Game.BuildingManager
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private BuildingTypeSO activeBuildingType;
        [SerializeField] private LayerMask gridLayerMask;
        [SerializeField] private GameManager gameManager;
        private Vector3 _targetPos;
        public static event Action<Vector3[]> BuildingCheck;
        
        public static event Action<int> UnitsPositionReguest;
        public static event Action<List<Vector2>> UnitsPositionResponse;
        private void OnEnable()
        {
           GridManager.OnBuildAreaChecked += HandleCellStatus;
           InventoryItem.RotateRequest += HandleRotateRequest;
           GridManager.UnitsPositionResponse += HandleEmptyCellRequest;
        }
        private void HandleEmptyCellRequest(List<Vector2> gridPositions)
        {
            UnitsPositionResponse?.Invoke(gridPositions);
        }

        private void OnDisable()
        {
            GridManager.OnBuildAreaChecked -= HandleCellStatus;
            InventoryItem.RotateRequest += HandleRotateRequest; 
        }

        private void HandleRotateRequest(BuildingTypeSO type)
        {

            type.ChangeDirection();
        }
        public static Action CountDecrease;
        private void HandleCellStatus(bool gridPosition)
        {
            if (gridPosition && activeBuildingType.count>0)
            {
                gameManager.AddToMyBuildings(activeBuildingType);
                Transform building = Instantiate(activeBuildingType.prefab, _targetPos, Quaternion.identity);
                building.GetChild(0).rotation = Quaternion.Euler(0, 0, activeBuildingType.GetDirectionAngle());
                CountDecrease.Invoke();
                AttackJob attackJob = activeBuildingType.attackJob;
                if(attackJob!=null)
                    UnitsPositionReguest.Invoke(attackJob.playerSoldiers.Count); 
            }
            
            _targetPos = Vector3.zero;
        }
        private void Update()
        {
            /*
            if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began && activeBuildingType != null)
        {
            // Touch pozisyonunu kullanarak Raycast yap
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero, gridLayerMask);

            if (hit)
            {
                Vector3 gridPosition = hit.transform.position;
                _targetPos = gridPosition;

                Vector3[] cells = activeBuildingType.cells.Select(cell => new Vector3(gridPosition.x + cell.x, gridPosition.y + cell.y, 0)).ToArray();
                
                BuildingCheck?.Invoke(cells);
            }
        }
    }
            */
            if (Input.GetMouseButtonDown(0) && activeBuildingType!=null)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,gridLayerMask);

                if (hit)
                {
                    Vector3 gridPosition = hit.transform.position;
                    _targetPos = gridPosition;

                    Vector3[] cells = activeBuildingType.cells.Select(cell => new Vector3(gridPosition.x + cell.x, gridPosition.y + cell.y, 0)).ToArray();
                    
                    BuildingCheck?.Invoke(cells);

                }
            }
           
        }
        public static BuildingManager Instance { get; private set; }
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
        }
        public static Action<BuildingTypeSO> ActiveBuildingTypeChanged;
        public void SetActiveBuildingType(BuildingTypeSO buildingTypeSo)
        {
            activeBuildingType = buildingTypeSo;
            ActiveBuildingTypeChanged.Invoke(buildingTypeSo);
        }

        public BuildingTypeSO GetActiveBuildingType()
        {
            return activeBuildingType;
        }


    }
}
