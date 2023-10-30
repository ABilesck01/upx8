using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace upx.Game
{
    public class CarBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private LayerMask streetLayer;
        [SerializeField] private Transform carView;

        private Transform[] waypointsToFollow;
        private int waypointIndex;
        private Transform myTransform;
        private Vector3 direction;

        public class OnCarStopEventArgs : EventArgs
        {
            public Vector3 position;
            public float normalizedSpeed;
        }

        public static event EventHandler<OnCarStopEventArgs> OnCarStop;

        private void Awake()
        {
            myTransform = transform;
        }

        private void Start()
        {   
            GetNewWaypointList();
        }

        private void GetNewWaypointList()
        {
            RaycastHit hit;
            Ray ray = new Ray(carView.position, -carView.up);
            if (Physics.Raycast(ray, out hit, 10f, streetLayer))
            {
                if(hit.transform.TryGetComponent(out VariableStreetController variableController))
                {
                    this.speed = variableController.GetCalculatedSpeed();
                    waypointsToFollow = variableController.GetWaypoints();
                    myTransform.position = waypointsToFollow[0].position;
                    myTransform.rotation = waypointsToFollow[0].rotation;
                    waypointIndex = 1;
                }
                else if (hit.transform.TryGetComponent(out StreetController streetController))
                {
                    waypointsToFollow = streetController.GetWaypoints();
                    myTransform.position = waypointsToFollow[0].position;
                    myTransform.rotation = waypointsToFollow[0].rotation;
                    waypointIndex = 1;
                }
            }
            else
            {
                waypointIndex = 0;
                waypointsToFollow = null;
                OnCarStop?.Invoke(this, new OnCarStopEventArgs
                {
                    position = transform.position,
                    normalizedSpeed = speed
                });
            }
        }

        private void Update()
        {
            if(waypointsToFollow ==  null) return;
            if(waypointsToFollow.Length <= 0) return;

            direction = (waypointsToFollow[waypointIndex].position - myTransform.position).normalized;
            myTransform.position += speed * Time.deltaTime * direction;
            if((myTransform.position - waypointsToFollow[waypointIndex].position).sqrMagnitude <= 0.01)
            {
                if(waypointIndex >= waypointsToFollow.Length - 1)
                {
                    waypointIndex = 0;
                    GetNewWaypointList();
                }
                else
                {
                    waypointIndex++;
                    myTransform.forward = waypointsToFollow[waypointIndex].forward;
                }
            }
        }
    }
}
