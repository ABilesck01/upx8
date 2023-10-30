using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace upx.level
{
    public class Visual : MonoBehaviour
    {
        [SerializeField] private Transform visual;

        private Vector3 initPosition;

        private void OnEnable()
        {
            BuildingSystem.OnStartBuilding += BuildingSystem_OnChangeBuilding;
            BuildingSystem.OnBuild += BuildingSystem_OnBuild;
        }
        private void OnDisable()
        {
            BuildingSystem.OnStartBuilding -= BuildingSystem_OnChangeBuilding;
            BuildingSystem.OnBuild += BuildingSystem_OnBuild;
        }

        private void Start()
        {
            initPosition = transform.position;
        }

        private void BuildingSystem_OnChangeBuilding(object sender, BuildingType e)
        {
            if (e == null) return;

            visual = Instantiate(e.visual, Vector3.zero, Quaternion.identity, transform);
            visual.position = initPosition;
            visual.localEulerAngles = Vector3.zero;
        }

        private void BuildingSystem_OnBuild(object sender, BuildingType e)
        {
            if(visual == null) return;

            transform.position = initPosition;
            Destroy(visual.gameObject);
            visual = null;
        }


        private void Update()
        {
            VisualFollow();
        }

        private void VisualFollow()
        {
            if (!visual) return;


            Vector3 targetPosition = BuildingSystem.instance.GetMouseGridPosition();    
            targetPosition.y = 0f;

            Quaternion targetDirection =
                BuildingSystem.instance.GetPlacedObjectDirection(out Vector3 pos);
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetDirection, Time.deltaTime * 15f);
        }

    }
}
