using System.Collections;

namespace TDScenes.Game.Units
{
    using System.Collections.Generic;
    using Unity.Mathematics;
    using UnityEngine;
    using TDScenes.Game.Grid;
    using Tools.AStar;
    using System;

    public class PathFinder : MonoBehaviour
    {
      
        [SerializeField] private LayerMask soldierLayerMask, gridLayer;
        private AStar _aStar;
        [SerializeField] private GridManager gridManager;
        public GameObject target;
        private Dictionary<GameObject, List<Vector2>> _activePaths;
        private bool _soldierSetted = false;
        GameObject selectedSoldier;
        public static Action<List<Vector2>> PathCreated;
        public static Action<bool> WantYouFight;
        private void Start()
        {
            _activePaths = new Dictionary<GameObject, List<Vector2>>();
            GridManager.MoveToSoldier += CreatePath;
        }

        private void CreatePath(Vector2[] positions)
        {
            _aStar = new AStar(gridManager.GetWalkableGrids());
            // Hedef belirlendi, askerin yolu hesaplanır
            Vector2 start = positions[0];
            Vector2 end = positions[1];
            List<Vector2> path = _aStar.FindPath(start, end);
            PathCreated.Invoke(path);
        }
        
        private void Update()
        {
            /*
             
             
             
              if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && !_soldierSetted)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, soldierLayerMask);

            if (hit.collider != null)
            {
                if (gridManager.IsCellType(hit.transform.position, CellContent.Soldier))
                {
                    selectedSoldier = hit.transform.gameObject;
                    _soldierSetted = true;
                }
            }
        }
        else if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && _soldierSetted)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, soldierLayerMask);

            if (hit.collider != null)
            {
                if (gridManager.IsCellType(hit.transform.position, CellContent.Empty) || gridManager.IsCellType(hit.transform.position, CellContent.Enemy))
                {
                    Instantiate(target, hit.transform.position, hit.transform.rotation);
                    _soldierSetted = false;
                    _aStar = new AStar(gridManager.GetWalkableGrids());

                    Vector2 start = selectedSoldier.transform.position;
                    Vector2 end = hit.transform.position;
                    if(gridManager.IsCellType(end, CellContent.Enemy))
                    {
                        WantYouFight.Invoke(true);
                    }
                    else if(gridManager.IsCellType(start, CellContent.Enemy) && !gridManager.IsCellType(end, CellContent.Enemy))
                        WantYouFight.Invoke(false);

                    List<Vector2> path = _aStar.FindPath(start, end);
                    gridManager.UpdateCellContent(start,CellContent.Empty);
                    gridManager.UpdateCellContent(end,CellContent.Soldier);
                    PathCreated.Invoke(path);
                    _activePaths[selectedSoldier] = path;
                }
                else
                {
                    _soldierSetted = false;
                    selectedSoldier = null;
                }
            }
        }
    }
             
             
             */
            if (Input.GetMouseButtonDown(0) && !_soldierSetted)
            { //ilk tıklamada
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, soldierLayerMask);

                if (hit.collider != null)
                {

                    if (gridManager.IsCellType(hit.transform.position, CellContent.Soldier))
                    {
                        selectedSoldier = hit.transform.gameObject;
                        _soldierSetted = true;

                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) && _soldierSetted)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, soldierLayerMask);

                if (hit.collider != null)
                {

                    if (gridManager.IsCellType(hit.transform.position, CellContent.Empty) || gridManager.IsCellType(hit.transform.position, CellContent.Enemy))
                    {
                     
                        _soldierSetted = false;
                        _aStar = new AStar(gridManager.GetWalkableGrids());
                        // Hedef belirlendi, askerin yolu hesaplanır
                        Vector2 start = selectedSoldier.transform.position;
                        Vector2 end = hit.transform.position;
                        

                        List<Vector2> path = _aStar.FindPath(start, end);
                        gridManager.UpdateCellContent(start,CellContent.Empty);
                        gridManager.UpdateCellContent(end,CellContent.Soldier);
                        PathCreated.Invoke(path);
                        _activePaths[selectedSoldier] = path;
                    }
                    else
                    {
                        _soldierSetted = false;
                        selectedSoldier = null;
                    }
                }

            }

        }





    }
}