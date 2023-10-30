using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using upx.level;
using upx.ui;

namespace upx.Game
{
    public class GameView : MonoBehaviour
    {
        [System.Serializable]
        public struct NormalStateUI
        {
            public GameObject container;
            public BuildingList buildingList;
            public Button btnDemolish;
            public Button btnPlay;
        }

        [System.Serializable]
        public struct BuildStateUI
        {
            public GameObject container;
            public Button btnRotate;
            public Button btnBuild;
            public Button btnRight;
            public Button btnLeft;
            public Button btnUp;
            public Button btnDown;
        }

        [System.Serializable]
        public struct BuildingList
        {
            public Transform container;
            public BuildingViewItem buildingViewItemTemplate;
            public BuildingType[] buildingTypes;
        }

        [SerializeField] private GameObject ArVisualizer;
        [SerializeField] private NormalStateUI normalStateUI;
        [SerializeField] private BuildStateUI buildStateUI;
        [SerializeField] private Button btnBack;

        private GameManager gameManager;

        private BuildingSystem buildingSystem;

        private void OnEnable()
        {
            BuildingSystem.OnStartLevel += BuildingSystem_OnStartLevel;
            BuildingSystem.OnStartBuilding += BuildingSystem_OnStartBuilding;
            BuildingSystem.OnBuild += BuildingSystem_OnBuild;
        }

        private void OnDisable()
        {
            BuildingSystem.OnStartLevel -= BuildingSystem_OnStartLevel;
            BuildingSystem.OnStartBuilding -= BuildingSystem_OnStartBuilding;
            BuildingSystem.OnBuild -= BuildingSystem_OnBuild;
        }

        private void BuildingSystem_OnStartLevel(object sender, BuildingSystem e)
        {
            buildingSystem = e;
            ArVisualizer.SetActive(false);
            normalStateUI.container.SetActive(true);
            FillBuildingList();
        }

        private void BuildingSystem_OnStartBuilding(object sender, BuildingType e)
        {
            normalStateUI.container.SetActive(false);
            buildStateUI.container.SetActive(true);
        }

        private void BuildingSystem_OnBuild(object sender, BuildingType e)
        {
            normalStateUI.container.SetActive(true);
            buildStateUI.container.SetActive(false);
        }

        private void FillBuildingList()
        {
            foreach (BuildingType item in normalStateUI.buildingList.buildingTypes)
            {
                var i = Instantiate(normalStateUI.buildingList.buildingViewItemTemplate, normalStateUI.buildingList.container);
                i.SetBuildingType(item, this.buildingSystem);
            }
        }

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            buildStateUI.btnRotate.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.Rotate();
            });

            buildStateUI.btnBuild.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.Build();
            });

            buildStateUI.btnRight.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.GoRight();
            });

            buildStateUI.btnLeft.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.GoLeft();
            });

            buildStateUI.btnUp.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.GoUp();
            });

            buildStateUI.btnDown.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.GoDown();
            });

            normalStateUI.btnDemolish.onClick.AddListener(() =>
            {
                if (buildingSystem == null) return;
                buildingSystem.StartDemolish();
            });

            normalStateUI.btnPlay.onClick.AddListener(() =>
            {
                normalStateUI.container.SetActive(false);
                gameManager.PlayLevel();
            });
            btnBack.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("main");
                SceneManager.LoadScene(LevelSelectionController.sceneName, LoadSceneMode.Additive);
            });
        }
    }
}
