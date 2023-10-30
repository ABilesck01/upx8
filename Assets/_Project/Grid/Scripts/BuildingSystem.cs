using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace upx.level
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private BuildingType building;
        [SerializeField] private BuildingType.Dir dir = BuildingType.Dir.Down;

        [SerializeField] private Vector2Int buildingPosition = Vector2Int.zero;

        [SerializeField] private int gridWidth = 10;
        [SerializeField] private int gridHeight = 10;
        [SerializeField] private float gridSpacing = 10f;
        [Space]
        [SerializeField] private bool getMousePosition = false;
        [SerializeField] private LayerMask buildLayer;
        [Space]
        [SerializeField] private BuildState buildState;
        [Space]
        public LayerMask UILayer;

        private Grid<GridObject> grid;

        private Camera cam;
        internal static BuildingSystem instance;

        public static event EventHandler<BuildingSystem> OnStartLevel;
        public static event EventHandler<BuildingType> OnChangeBuildingType;
        public static event EventHandler<BuildingType> OnStartBuilding;
        public static event EventHandler<BuildingType> OnBuild;
        public static event EventHandler OnStartDemolish;
        public static event EventHandler<BuildingType> OnDemolish;

        #region Unity

        private void Start()
        {
            instance = this;

            cam = GameObject.Find("AR Camera").GetComponent<Camera>();

            grid = new Grid<GridObject>(gridWidth, gridHeight, gridSpacing, transform.position,
            (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z), true);
        }

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        #endregion

        #region setters

        public void SetBuilding(BuildingType building)
        {
            this.building = building;
            OnChangeBuildingType?.Invoke(this, building);
        }

        #endregion

        #region Getters

        public Vector3 GetWorldPosition(Vector2Int coordinate)
        {
            return grid.GetWorldPosition(coordinate.x, coordinate.y);
        }

        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            grid.GetXZ(worldPosition, out int x, out int y);
            Debug.Log($"Grid Position {worldPosition} - {x} - {y}");
            return new Vector2Int(x, y);
        }

        #endregion

        #region controller

        [ContextMenu("Start build")]
        public void StartBuild()
        {
            buildState = BuildState.Build;
            OnStartBuilding?.Invoke(this, building);
        }

        public void StartDemolish()
        {
            buildState = BuildState.Demolish;
            OnStartDemolish?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Interactions

        public void Build()
        {
            List<Vector2Int> gridPosList = building.GetGridPositionList(new Vector2Int(buildingPosition.x, buildingPosition.y), dir);
            bool canBuild = true;

            foreach (Vector2Int pos in gridPosList)
            {
                if (!grid.GetValue(pos.x, pos.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int offset = building.GetRotationOffset(dir);
                Vector3 placedPosition = grid.GetWorldPosition(buildingPosition.x, buildingPosition.y) +
                    new Vector3(offset.x, 0, offset.y) * grid.GetCellSize();



                PlacedObject placedObject = PlacedObject.Create(
                    placedPosition, 
                    new Vector2Int(buildingPosition.x, buildingPosition.y), 
                    dir, 
                    building,
                    transform);

                foreach (Vector2Int item in gridPosList)
                {
                    grid.GetValue(item.x, item.y).SetPlacedObject(placedObject);
                }
            }

            OnBuild?.Invoke(this, building);
            buildState = BuildState.Normal;
            buildingPosition = Vector2Int.zero;
        }

        public void BuildAtPosition(Vector2Int position, BuildingType building)
        {
            List<Vector2Int> gridPosList = building.GetGridPositionList(new Vector2Int(position.x, position.y), dir);
            bool canBuild = true;

            foreach (Vector2Int pos in gridPosList)
            {
                if (!grid.GetValue(pos.x, pos.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int offset = building.GetRotationOffset(dir);
                Vector3 placedPosition = grid.GetWorldPosition(position.x, position.y) +
                    new Vector3(offset.x, 0, offset.y) * grid.GetCellSize();



                PlacedObject placedObject = PlacedObject.Create(
                    placedPosition,
                    new Vector2Int(position.x, position.y),
                    dir,
                    building,
                    transform);

                foreach (Vector2Int item in gridPosList)
                {
                    grid.GetValue(item.x, item.y).SetPlacedObject(placedObject);
                }
            }

            OnBuild?.Invoke(this, building);
            buildState = BuildState.Normal;
            buildingPosition = Vector2Int.zero;
        }

        public void Rotate()
        {
            dir = BuildingType.GetNextDir(dir);
        }

        private void Demolish()
        {
            GridObject gridObject = grid.GetValue(buildingPosition.x, buildingPosition.y);
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if(placedObject != null )
            {
                placedObject.DestroySelf();

                List<Vector2Int> gridPosList = placedObject.GetGridPositionList();
                foreach (Vector2Int item in gridPosList)
                {
                    grid.GetValue(item.x, item.y).ClearPlacedObject();
                }
            }

            OnDemolish?.Invoke(this, placedObject.GetBuildingType());
            buildState = BuildState.Normal;
        }

        [ContextMenu("Target Focus")]
        public void OnTargetFocus()
        {
            OnStartLevel?.Invoke(this, this);
        }

        #endregion

        #region Mouse Positions

        public Quaternion GetPlacedObjectDirection(out Vector3 position)
        {
            if (building != null)
            {
                Vector2Int rotationOffset = building.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(buildingPosition.x, buildingPosition.y) +
                    new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                position = placedObjectWorldPosition;

                return Quaternion.Euler(0, building.GetRotationAngle(dir), 0);
            }
            else
            {
                position = Vector3.zero;
                return Quaternion.identity;
            }
        }

        public bool IsPointerOverUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            List<RectTransform> uiResults = new List<RectTransform>();

            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.TryGetComponent<RectTransform>(out RectTransform rect))
                {
                    uiResults.Add(rect);
                }
            }

            return uiResults.Count > 0;
        }

        public Vector3 GetMouseGridPosition()
        {
            Vector2Int rotationOffset = building.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(buildingPosition.x, buildingPosition.y) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }

        public void GoRight()
        {
            Debug.Log("Right");
            if (buildingPosition.x >= gridWidth - 1) return;
            buildingPosition.x++;
        }

        public void GoLeft()
        {
            Debug.Log("Left");
            if (buildingPosition.x <= 0) return;
            buildingPosition.x--;
        }


        public void GoUp()
        {
            Debug.Log("Up");
            if (buildingPosition.y >= gridHeight - 1) return;
            buildingPosition.y++;
        }

        public void GoDown()
        {
            Debug.Log("Down");
            if (buildingPosition.y <= 0) return;
            buildingPosition.y--;
        }

        #endregion



    }
}

public enum BuildState
{
    Normal,
    Build,
    Demolish
}

