using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace upx.level
{
    public class GridObject : MonoBehaviour
    {
        private Grid<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedObject(PlacedObject t)
        {
            Debug.Log(t.gameObject.name);
            placedObject = t;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }
    }
}
