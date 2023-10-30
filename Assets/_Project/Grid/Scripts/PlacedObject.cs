using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace upx.level
{
    public class PlacedObject : MonoBehaviour
    {
        public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin,
        BuildingType.Dir dir, BuildingType building, Transform parent)
        {
            Transform placedObjectTransform = Instantiate(
                building.prefab,
                worldPosition,
                Quaternion.Euler(0, building.GetRotationAngle(dir), 0)).transform;

            placedObjectTransform.parent = parent;
            placedObjectTransform.localScale = Vector3.one;
            PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

            placedObject.buildingTypeSO = building;
            placedObject.origin = origin;
            placedObject.dir = dir;

            return placedObject;
        }

        private BuildingType buildingTypeSO;
        private Vector2Int origin;
        private BuildingType.Dir dir;

        public List<Vector2Int> GetGridPositionList()
        {
            return buildingTypeSO.GetGridPositionList(origin, dir);
        }

        public BuildingType GetBuildingType() => buildingTypeSO;

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
