using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using upx.level;

namespace upx.Game
{
    public class BuildingViewItem : MonoBehaviour
    {
        private BuildingType buildingType;
        private BuildingSystem buildingSystem;

        private Button button;
        private TextMeshProUGUI txtName;

        private void Awake()
        {
            button = GetComponent<Button>();
            txtName = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetBuildingType(BuildingType buildingType, BuildingSystem buildingSystem)
        {
            this.buildingType = buildingType;
            this.buildingSystem = buildingSystem;

            txtName.text = buildingType.buildingName;

            button.onClick.AddListener(() =>
            {
                this.buildingSystem.SetBuilding(this.buildingType);
                this.buildingSystem.StartBuild();
            });
        }
    }
}
