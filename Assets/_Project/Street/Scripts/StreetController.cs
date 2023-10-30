using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using upx.level;

namespace upx.Game
{
    public class StreetController : PlacedObject
    {
        [SerializeField] private Transform[] waypoints;

        public Transform[] GetWaypoints()
        {
            return waypoints;
        }
    }
}
