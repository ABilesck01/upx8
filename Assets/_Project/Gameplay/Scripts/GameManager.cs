using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using upx.level;

namespace upx.Game
{
    public class GameManager : MonoBehaviour
    {
        [System.Serializable]
        public struct GameSettings
        {
            public Vector2Int origin;
            public Vector2Int destination;
            [Range(0.5f,4)] public float desiredSpeed;
        }
        [System.Serializable]
        public struct Streets
        {
            public BuildingType startStreet;
            public BuildingType endStreet;
        }

        [SerializeField] private GameObject carObject;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Streets streets;
        [Space]
        [SerializeField] EndGameView endGameView;

        private BuildingSystem buildingSystem;

        public static bool isPlayingLevel;

        private void OnEnable()
        {
            BuildingSystem.OnStartLevel += BuildingSystem_OnStartLevel;
            CarBehaviour.OnCarStop += CarBehaviour_OnCarStop;
        }

        private void OnDisable()
        {
            BuildingSystem.OnStartLevel -= BuildingSystem_OnStartLevel;
            CarBehaviour.OnCarStop -= CarBehaviour_OnCarStop;
        }

        private void BuildingSystem_OnStartLevel(object sender, BuildingSystem e)
        {
            this.buildingSystem = e;
            buildingSystem.BuildAtPosition(gameSettings.origin, streets.startStreet);
            buildingSystem.BuildAtPosition(gameSettings.destination, streets.endStreet);
        }

        private void CarBehaviour_OnCarStop(object sender, CarBehaviour.OnCarStopEventArgs e)
        {
            Vector2Int carPosition = buildingSystem.GetGridPosition(e.position);
            endGameView.ShowEndScreen(carPosition == gameSettings.destination, gameSettings.desiredSpeed == e.normalizedSpeed);
        }

        public void PlayLevel()
        {
            Instantiate(carObject, buildingSystem.GetWorldPosition(gameSettings.origin), buildingSystem.transform.rotation, buildingSystem.transform);
        }
    }
}
